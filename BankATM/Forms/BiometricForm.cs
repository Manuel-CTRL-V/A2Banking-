using ATM.Kiosk;
using ATM.Kiosk.Business.Context;
using ATM.Kiosk.Business.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankATM.Forms
{
    public partial class BiometricForm : Form
    {
        private readonly AuthService _authService;
        public BiometricForm(AuthService authService)
        {
            InitializeComponent();
            _authService = authService;
        }
        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);
            await StartVerificationAsync();
        }

        private async Task StartVerificationAsync()
        {
            lblStatus.Text = "Coloque su dedo en el lector...";
            btnCancel.Enabled = true;
            progressBar.Value = 0;

            try
            {
                await Task.Delay(1500); // deja que el usuario lea

                // Ejecutar en segundo plano (IMPORTANTE)
                await Task.Run(() => _authService.VerifyBiometric());

                lblStatus.Text = "Huella detectada...";
                progressBar.Value = 30;

                await Task.Delay(1500);

                lblStatus.Text = "Verificando identidad...";
                progressBar.Value = 60;

                await Task.Delay(1500);

                _authService.CompleteAuth();

                lblStatus.Text = "Identidad verificada.";
                progressBar.Value = 100;

                await Task.Delay(2000);

                var menu = new MainMenuForm(AppServices.Transactions, AppServices.Auth);
                menu.Show();
                this.Hide();
            }
            catch (BusinessException ex) when (ex.Code == BusinessErrorCode.BiometricFailed)
            {
                lblStatus.Text = "Verificación fallida. Intente de nuevo.";
                lblStatus.ForeColor = ATM.Kiosk.Helpers.ATMColors.TextError;
                progressBar.Value = 0;

                var retry = MessageBox.Show(
                    "No se pudo verificar su huella. ¿Desea intentar de nuevo?",
                    "Verificación fallida",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (retry == DialogResult.Yes)
                {
                    lblStatus.ForeColor = ATM.Kiosk.Helpers.ATMColors.TextPrimary;
                    await StartVerificationAsync(); // 👈 importante usar await
                }
                else
                {
                    GoToLogin();
                }
            }
        }
        private void GoToLogin()
        {
            var login = new LoginForm();
            login.Show();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) => GoToLogin();
    }
}
