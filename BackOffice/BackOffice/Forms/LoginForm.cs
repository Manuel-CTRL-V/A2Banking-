using BackOffice.Forms;
using BackOffice.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BackOffice.Services.Implementations;

namespace BackOffice
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            txtPassword.UseSystemPasswordChar = true;

            BackOfficeColors.ThemeChanged += OnThemeChanged;
            ApplyTheme();
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
            bool isDark = BackOfficeColors.CurrentTheme == ThemeType.Dark;

            this.BackColor = BackOfficeColors.Background;

            kryptonPanel1.StateCommon.Color1 = BackOfficeColors.MenuBackground;
            kryptonPanel1.StateCommon.Color2 = isDark ? Color.FromArgb(25, 25, 25) : Color.White;

            kryptonGroup1.StateCommon.Back.Color1 = isDark ? Color.FromArgb(45, 45, 45) : Color.White;
            kryptonGroup1.StateCommon.Back.Color2 = isDark ? Color.FromArgb(40, 40, 40) : Color.White;
            kryptonGroup1.StateCommon.Border.Color1 = isDark ? Color.FromArgb(60, 60, 60) : Color.White;

            kryptonGroup2.StateCommon.Back.Color1 = isDark ? Color.FromArgb(50, 50, 50) : Color.FromArgb(241, 244, 242);
            kryptonGroup2.StateCommon.Border.Color1 = isDark ? Color.FromArgb(60, 60, 60) : Color.White;

            kryptonGroup3.StateCommon.Back.Color1 = isDark ? Color.FromArgb(50, 50, 50) : Color.FromArgb(241, 244, 242);
            kryptonGroup3.StateCommon.Border.Color1 = isDark ? Color.FromArgb(60, 60, 60) : Color.White;

            txtUsername.StateCommon.Back.Color1 = isDark ? Color.FromArgb(50, 50, 50) : Color.FromArgb(241, 244, 242);
            txtUsername.StateCommon.Border.Color1 = isDark ? Color.FromArgb(60, 60, 60) : Color.FromArgb(241, 244, 242);
            txtUsername.StateCommon.Content.Color1 = isDark ? Color.FromArgb(180, 180, 180) : Color.Gray;

            txtPassword.StateCommon.Back.Color1 = isDark ? Color.FromArgb(50, 50, 50) : Color.FromArgb(241, 244, 242);
            txtPassword.StateCommon.Border.Color1 = isDark ? Color.FromArgb(60, 60, 60) : Color.FromArgb(241, 244, 242);
            txtPassword.StateCommon.Content.Color1 = isDark ? Color.FromArgb(180, 180, 180) : Color.Gray;

            btnLogin.StateCommon.Back.Color1 = isDark ? Color.FromArgb(0, 200, 83) : Color.FromArgb(57, 95, 59);
            btnLogin.StateCommon.Back.Color2 = isDark ? Color.FromArgb(0, 180, 70) : Color.FromArgb(57, 95, 59);
            btnLogin.StateCommon.Content.ShortText.Color1 = isDark ? Color.FromArgb(20, 20, 20) : Color.White;

            label1.ForeColor = isDark ? Color.FromArgb(180, 180, 180) : Color.FromArgb(64, 64, 64);
            label2.ForeColor = isDark ? Color.FromArgb(180, 180, 180) : Color.FromArgb(64, 64, 64);
            label3.ForeColor = isDark ? Color.White : Color.FromArgb(64, 64, 64);
            label4.ForeColor = isDark ? Color.FromArgb(180, 180, 180) : Color.FromArgb(64, 64, 64);
            label5.ForeColor = isDark ? Color.FromArgb(150, 150, 150) : Color.FromArgb(64, 64, 64);
            label6.ForeColor = isDark ? Color.FromArgb(150, 150, 150) : Color.FromArgb(64, 64, 64);

            iconButton1.IconColor = isDark ? Color.FromArgb(150, 150, 150) : Color.Gray;
            iconButton2.IconColor = isDark ? Color.FromArgb(150, 150, 150) : Color.Gray;
            iconButton3.IconColor = BackOfficeColors.TextPrimary;

            lblError.ForeColor = isDark ? Color.FromArgb(255, 82, 82) : Color.FromArgb(229, 57, 53);
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            lblError.Visible = false;

            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                lblError.Text = "Ingrese el nombre de usuario.";
                lblError.Visible = true;
                return;
            }

            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                lblError.Text = "Ingrese la contraseña.";
                lblError.Visible = true;
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                AppServices.AuthService.Login(txtUsername.Text, txtPassword.Text);
                Cursor = Cursors.Default;

                var menu = new MainFormMenu();
                this.Hide();
                menu.Show();
                //this.Close();
                
            }
            catch (BackOfficeApiException ex) when (ex.IsInvalidCredentials)
            {
                Cursor = Cursors.Default;
                lblError.Text = "Usuario o contraseña incorrectos.";
                lblError.Visible = true;
            }
            catch (BackOfficeApiException ex) when (ex.IsConnectionError)
            {
                Cursor = Cursors.Default;

                lblError.Text = "Error en el servidor, " + ex.Message + ", " + ex.ErrorCode;
                lblError.Visible = true;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                lblError.Text = "Error: " + ex.Message;
                lblError.Visible = true;
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            BackOfficeColors.ThemeChanged -= OnThemeChanged;
            base.OnFormClosing(e);
        }
    }
}
