using ATM.Kiosk.Business.Auth;
using ATM.Kiosk.Business.Context;
using ATM.Kiosk.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankATM.Forms
{
    public partial class MainMenuForm : Form
    {
        private readonly TransactionContext _txContext;
        private readonly AuthService _authService;
        public MainMenuForm(TransactionContext txContext, AuthService authService)
        {
            InitializeComponent();

            _txContext = txContext;
            _authService = authService;
            
            ATMColors.ThemeChanged += OnThemeChanged;
            ApplyTheme();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadSessionData();
            timerClock.Start();
            timerIdle.Start();
        }

        // ── Carga de datos de sesión ──────────────────────────────────

        private void LoadSessionData()
        {
            var session = SessionManager.Instance.Current;
            if (session == null) { GoToLogin(); return; }

            var firstName = session.HolderName.Split(' ')[0];
            lblGreeting.Text = "HOLA, " + firstName.ToUpper();
            //lblAccountType.Text = session.AccountType.ToUpper(); 
            UpdateClock();
        }

        private void timerClock_Tick(object sender, EventArgs e)
        {
            UpdateClock();
        }

        private void timerIdle_Tick(object sender, EventArgs e)
        {
            if (!SessionManager.Instance.CheckExpired()) return;

            timerClock.Stop();
            timerIdle.Stop();

            MessageBox.Show(
                "Su sesión expiró por inactividad.",
                "Sesión expirada",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            _authService.Logout("Timeout");
            GoToLogin();
        }
        private void UpdateClock()
        {
            lblDateTime.Text = DateTime.Now.ToString("hh:mm tt, dd MMM").ToUpper();
        }

        private void btnDeposit_Click(object sender, EventArgs e)
        {
            SessionManager.Instance.RecordActivity();
            using (var form = new DepositForm(_txContext))
                form.ShowDialog(this);
        }

        private void btnWithdraw_Click(object sender, EventArgs e)
        {
            SessionManager.Instance.RecordActivity();
            using (var form = new WithdrawForm(_txContext))
                form.ShowDialog(this);
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            SessionManager.Instance.RecordActivity();
            using (var form = new TransferForm(_txContext))
                form.ShowDialog(this);
        }

        private void btnBalance_Click(object sender, EventArgs e)
        {
            SessionManager.Instance.RecordActivity();
            using (var form = new BalanceForm(_txContext))
                form.ShowDialog(this);
        }

        private void btnChangePin_Click(object sender, EventArgs e)
        {
            SessionManager.Instance.RecordActivity();
            using (var form = new ChangePinForm())
                form.ShowDialog(this);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea cerrar la sesión?", "Salir",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                timerClock.Stop();
                timerIdle.Stop();
                _authService.Logout("UserLogout");
                GoToLogin();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            SessionManager.Instance.RecordActivity();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // Navegación 

        private void GoToLogin()
        {
            this.Hide();
            var login = new LoginForm();
            login.Show();
            this.Close();
        }

        private void MainMenuForm_Load(object sender, EventArgs e)
        {
            btnDeposit.Click += btnDeposit_Click;
            btnWithdraw.Click += btnWithdraw_Click;
            btnTransfer.Click += btnTransfer_Click;
            btnBalance.Click += btnBalance_Click;
            btnChangePin.Click += btnChangePin_Click;
            btnExit.Click += btnExit_Click;
            btnSettings.Click += btnSettings_Click;
            btnMiniStatement.Click += btnMiniStatement_Click;
        }

        private void btnExit2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea cerrar la sesión?", "Salir",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                timerClock.Stop();
                timerIdle.Stop();
                _authService.Logout("UserLogout");
                GoToLogin();
            }
        }

        private void btnTheme_Click(object sender, EventArgs e)
        {
            ATMColors.ToggleTheme();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            SessionManager.Instance.RecordActivity();
            using (var form = new SettingsForm(_txContext))
                form.ShowDialog(this);
        }

        private void btnMiniStatement_Click(object sender, EventArgs e)
        {
            SessionManager.Instance.RecordActivity();
            using (var form = new MiniStatementForm(_txContext))
                form.ShowDialog(this);
        }

        private void OnThemeChanged(ThemeType theme)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnThemeChanged(theme)));
                return;
            }
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            bool isDark = ATMColors.CurrentTheme == ThemeType.Dark;

            this.BackColor = ATMColors.Background;
            kryptonPanel1.StateCommon.Color1 = ATMColors.MenuBackground;
            kryptonPanel1.StateCommon.Color2 = ATMColors.BackgroundDeeper;

            label1.BackColor = isDark ? Color.FromArgb(60, 60, 60) : Color.FromArgb(208, 252, 206);

            label2.ForeColor = ATMColors.TextPrimary;
            lblGreeting.ForeColor = ATMColors.TextPrimary;
            lblDateTime.ForeColor = ATMColors.TextPrimary;

            pictureBox1.Visible = !isDark;

            btnExit2.IconColor = ATMColors.TextPrimary;

            btnTheme.IconChar = isDark ? FontAwesome.Sharp.IconChar.Sun : FontAwesome.Sharp.IconChar.Moon;
            btnTheme.IconColor = ATMColors.TextPrimary;

            btnSettings.IconColor = ATMColors.TextPrimary;
            btnMiniStatement.IconColor = ATMColors.TextPrimary;

            var buttonBackColor = isDark ? Color.FromArgb(50, 50, 50) : Color.FromArgb(221, 228, 225);
            var buttonBackColorHover = isDark ? Color.FromArgb(70, 70, 70) : Color.FromArgb(200, 215, 205);
            var buttonBackColorPressed = isDark ? Color.FromArgb(40, 40, 40) : Color.FromArgb(190, 205, 195);
            var buttonTextColor = isDark ? Color.White : Color.Black;

            ApplyKryptonButtonTheme(btnDeposit, buttonBackColor, buttonBackColorHover, buttonBackColorPressed, buttonTextColor);
            ApplyKryptonButtonTheme(btnBalance, buttonBackColor, buttonBackColorHover, buttonBackColorPressed, buttonTextColor);
            ApplyKryptonButtonTheme(btnWithdraw, buttonBackColor, buttonBackColorHover, buttonBackColorPressed, buttonTextColor);
            ApplyKryptonButtonTheme(btnTransfer, buttonBackColor, buttonBackColorHover, buttonBackColorPressed, buttonTextColor);
            ApplyKryptonButtonTheme(btnChangePin, buttonBackColor, buttonBackColorHover, buttonBackColorPressed, buttonTextColor);

            btnExit.StateCommon.Back.Color1 = isDark ? Color.FromArgb(255, 80, 80) : Color.Salmon;
            btnExit.StateCommon.Back.Color2 = isDark ? Color.FromArgb(200, 60, 60) : Color.FromArgb(221, 228, 225);
            btnExit.StateCommon.Content.ShortText.Color1 = Color.White;
            btnExit.StateCommon.Border.Color1 = isDark ? Color.FromArgb(60, 60, 60) : Color.FromArgb(200, 200, 200);
            btnExit.StateCommon.Border.Color2 = isDark ? Color.FromArgb(60, 60, 60) : Color.FromArgb(200, 200, 200);
            btnExit.StateTracking.Back.Color1 = isDark ? Color.FromArgb(255, 100, 100) : Color.Salmon;
            btnExit.StateTracking.Back.Color2 = isDark ? Color.FromArgb(220, 80, 80) : Color.FromArgb(221, 228, 225);
            btnExit.StateTracking.Border.Color1 = isDark ? Color.FromArgb(80, 80, 80) : Color.FromArgb(180, 180, 180);
            btnExit.StateTracking.Border.Color2 = isDark ? Color.FromArgb(80, 80, 80) : Color.FromArgb(180, 180, 180);
            btnExit.StatePressed.Back.Color1 = isDark ? Color.FromArgb(200, 50, 50) : Color.FromArgb(200, 100, 100);
            btnExit.StatePressed.Back.Color2 = isDark ? Color.FromArgb(180, 40, 40) : Color.FromArgb(190, 95, 95);
            btnExit.StatePressed.Border.Color1 = isDark ? Color.FromArgb(70, 70, 70) : Color.FromArgb(170, 170, 170);
            btnExit.StatePressed.Border.Color2 = isDark ? Color.FromArgb(70, 70, 70) : Color.FromArgb(170, 170, 170);
        }

        private void ApplyKryptonButtonTheme(Krypton.Toolkit.KryptonButton btn, Color backColor, Color backColorHover, Color backColorPressed, Color textColor)
        {
            bool isDark = ATMColors.CurrentTheme == ThemeType.Dark;

            btn.StateCommon.Back.Color1 = backColor;
            btn.StateCommon.Back.Color2 = backColor;
            btn.StateCommon.Content.ShortText.Color1 = textColor;
            btn.StateCommon.Border.Color1 = isDark ? Color.FromArgb(60, 60, 60) : Color.FromArgb(200, 200, 200);
            btn.StateCommon.Border.Color2 = isDark ? Color.FromArgb(60, 60, 60) : Color.FromArgb(200, 200, 200);

            btn.StateTracking.Back.Color1 = backColorHover;
            btn.StateTracking.Back.Color2 = backColorHover;
            btn.StateTracking.Content.ShortText.Color1 = textColor;
            btn.StateTracking.Border.Color1 = isDark ? Color.FromArgb(80, 80, 80) : Color.FromArgb(180, 180, 180);
            btn.StateTracking.Border.Color2 = isDark ? Color.FromArgb(80, 80, 80) : Color.FromArgb(180, 180, 180);

            btn.StatePressed.Back.Color1 = backColorPressed;
            btn.StatePressed.Back.Color2 = backColorPressed;
            btn.StatePressed.Content.ShortText.Color1 = textColor;
            btn.StatePressed.Border.Color1 = isDark ? Color.FromArgb(70, 70, 70) : Color.FromArgb(170, 170, 170);
            btn.StatePressed.Border.Color2 = isDark ? Color.FromArgb(70, 70, 70) : Color.FromArgb(170, 170, 170);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            timerClock?.Stop();
            timerIdle?.Stop();
            ATMColors.ThemeChanged -= OnThemeChanged;
            base.OnFormClosing(e);
        }
    }
}
