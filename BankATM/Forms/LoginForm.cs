using ATM.Kiosk;
using ATM.Kiosk.Business.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankATM.Forms
{
    public partial class LoginForm : Form
    {
        private AuthService _authService;
        public LoginForm()
        {
            InitializeComponent();
            txtAccountId.Focus();
            // Reiniciar servicios para limpiar estado de sesión anterior
            AppServices.Reset();
            _authService = AppServices.Auth;

            txtAccountId.KeyPress += OnlyNumbers_KeyPress;
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;

            if (string.IsNullOrWhiteSpace(txtAccountId.Text))
            {
                lblError.Text = "Ingrese un número de cuenta.";
                lblError.Visible = true;
                return;
            }

            if (!int.TryParse(txtAccountId.Text, out int accountId))
            {
                lblError.Text = "Número de cuenta inválido.";
                lblError.Visible = true;
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                string holderName = _authService.StartAuth(accountId);
                Cursor = Cursors.Default;

                // Ir a pantalla de PIN pasando el authService con estado intermedio
                var pinForm = new PinForm(_authService, holderName);
                pinForm.Show();
                this.Hide();
            }
            catch (ATM.Kiosk.Services.Exceptions.ApiException ex) when (ex.IsAccountNotFound)
            {
                Cursor = Cursors.Default;
                lblError.Text = "Cuenta no encontrada.";
                lblError.Visible = true;
            }
            catch (ATM.Kiosk.Services.Exceptions.ApiException ex) when (ex.IsAccountNotActive)
            {
                Cursor = Cursors.Default;
                lblError.Text = "La cuenta no está activa. Contacte al banco.";
                lblError.Visible = true;
            }
            catch (ATM.Kiosk.Services.Exceptions.ApiException ex) when (ex.IsConnectionError)
            {
                Cursor = Cursors.Default;
                lblError.Text = ex.Message;   // <-- mostrar el mensaje real
                lblError.Visible = true;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                lblError.Text = "Error inesperado: " + ex.Message;
                lblError.Visible = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtAccountId.Clear();
            lblError.Visible = false;
            txtAccountId.Focus();
            this.Close();
            Application.Exit();
        }
        private void OnlyNumbers_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                e.Handled = true;
        }

        private void txtAccountId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnContinue_Click(sender, EventArgs.Empty);
        }
    }
}
