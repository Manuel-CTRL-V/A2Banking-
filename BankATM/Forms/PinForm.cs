using ATM.Kiosk.Business.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankATM.Forms
{
    public partial class PinForm : Form
    {
        private readonly AuthService _authService;
        private readonly string _holderName;
        private int _attemptsLeft = 3;
        public PinForm(AuthService authService, string holderName)
        {
            InitializeComponent();

            txtPin.Visible = false;
            txtPin.MaxLength = 6;
            _authService = authService;
            _holderName = holderName;

            lblWelcome.Text = holderName.ToUpper();
            lblAttemptsLeft.Text = "Intentos restantes: " + _attemptsLeft;

            // El TextBox de PIN no debe mostrar los caracteres
            txtPin.UseSystemPasswordChar = true;
            txtPin.KeyPress += OnlyNumbers_KeyPress;

            // Conectar botones del teclado numérico en pantalla
            btn1.Click += NumPad_Click; btn2.Click += NumPad_Click;
            btn3.Click += NumPad_Click; btn4.Click += NumPad_Click;
            btn5.Click += NumPad_Click; btn6.Click += NumPad_Click;
            btn7.Click += NumPad_Click; btn8.Click += NumPad_Click;
            btn9.Click += NumPad_Click; btn0.Click += NumPad_Click;
        }
        private void NumPad_Click(object sender, EventArgs e)
        {
            if (txtPin.Text.Length >= 6) return;
            var btn = sender as Button;
            txtPin.Text += btn.Text;
            UpdateDots();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtPin.Text.Length > 0)
                txtPin.Text = txtPin.Text.Substring(0, txtPin.Text.Length - 1);
            UpdateDots();
        }
        // Actualiza los indicadores visuales de puntos del PIN
        private void UpdateDots()
        {
            int length = txtPin.Text.Length;
            int max = 6;

            string dots = "";

            for (int i = 0; i < max; i++)
            {
                if (i < length)
                    dots += "● "; // punto lleno
                else
                    dots += "  "; // punto vacío (opcional)
            }

            lblPinNumbers.Text = dots.Trim();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (txtPin.Text.Length < 4)
            {
                lblError.Text = "El PIN debe tener al menos 4 dígitos.";
                lblError.Visible = true;
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                _authService.VerifyPin(txtPin.Text);
                Cursor = Cursors.Default;

                // PIN correcto → ir a verificación biométrica
                var bioForm = new BiometricForm(_authService);
                bioForm.Show();
                this.Hide();
            }
            catch (ATM.Kiosk.Services.Exceptions.ApiException ex) when (ex.IsWrongPIN)
            {
                Cursor = Cursors.Default;
                _attemptsLeft--;
                txtPin.Clear();
                UpdateDots();

                if (_attemptsLeft <= 0)
                {
                    MessageBox.Show(
                        "Demasiados intentos fallidos. La sesión ha sido cancelada.",
                        "Acceso bloqueado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    GoToLogin();
                    return;
                }

                lblAttemptsLeft.Text = "Intentos restantes: " + _attemptsLeft;
                lblError.Text = "PIN incorrecto. Intente de nuevo.";
                lblError.Visible = true;
            }
            catch (ATM.Kiosk.Services.Exceptions.ApiException ex) when (ex.IsConnectionError)
            {
                Cursor = Cursors.Default;
                lblError.Text = "Sin conexión con el servidor.";
                lblError.Visible = true;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                lblError.Text = "Error: " + ex.Message;
                lblError.Visible = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) => GoToLogin();
        private void GoToLogin()
        {
            var login = new LoginForm();
            login.Show();
            this.Close();
        }
        private void OnlyNumbers_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                e.Handled = true;
        }

        private void iconButton1_Click(object sender, EventArgs e) => Application.Exit();
    }
}
