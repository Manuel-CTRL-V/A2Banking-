using ATM.Kiosk;
using ATM.Kiosk.Business.Auth;
using ATM.Kiosk.Business.Context;
using ATM.Kiosk.Services.Exceptions;
using ATM.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATM.Kiosk.Business.Exceptions;    

namespace BankATM.Forms
{
    public partial class TransferForm : Form
    {
        private readonly TransactionContext _txContext;
        private AuthService _authService;
        public TransferForm(TransactionContext txContext)
        {
            InitializeComponent();
            _authService = AppServices.Auth;
            _txContext = txContext;

            var session = SessionManager.Instance.Current;
            if (session != null)
                lblBalance.Text = "Saldo disponible: " + session.CurrentBalance.ToString("C");

            txtToAccount.KeyPress += OnlyNumbers_KeyPress;
            txtAmount.KeyPress += OnlyDecimals_KeyPress;

        }

        private void txtToAccount_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                lblAmountDisplay.Text = amount.ToString("C");
                // Mostrar comisión estimada (solo referencial — el servidor decide)
                decimal commission = amount * 0.0015m;
                lblCommissionEstimate.Text = "Comisión estimada: " + commission.ToString("C");
            }
            else
            {
                lblAmountDisplay.Text = "RD$0.00";
                lblCommissionEstimate.Text = "";
            }
        }
        private void OnlyNumbers_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                e.Handled = true;
        }

        private void OnlyDecimals_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool isDigit = char.IsDigit(e.KeyChar);
            bool isBackspace = e.KeyChar == (char)Keys.Back;
            bool isDot = e.KeyChar == '.' && !txtAmount.Text.Contains(".");
            if (!isDigit && !isBackspace && !isDot)
                e.Handled = true;
        }

        private void btnConfirm_Click_1(object sender, EventArgs e)
        {
            lblError.Visible = false;

            if (!int.TryParse(txtToAccount.Text, out int toAccountId) || toAccountId <= 0)
            {
                lblError.Text = "Ingrese un número de cuenta destino válido.";
                lblError.Visible = true;
                return;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal amount) || amount <= 0)
            {
                lblError.Text = "Ingrese un monto válido mayor a cero.";
                lblError.Visible = true;
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                MessageBox.Show("Coloque su dedo sobre el biométrico luego de cerrar esta ventana\nPresione 'Aceptar' para proseguir", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var result = _txContext.ExecuteTransfer(amount, toAccountId);
                Cursor = Cursors.Default;

                using (var resultForm = new ResultForm(result))
                    resultForm.ShowDialog(this);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (BusinessException ex) when (ex.Code == BusinessErrorCode.SameAccountTransfer)
            {
                Cursor = Cursors.Default;
                lblError.Text = "La cuenta destino no puede ser la misma que la de origen.";
                lblError.Visible = true;
            }
            catch (BusinessException ex)
            {
                Cursor = Cursors.Default;
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }
            catch (ATM.Kiosk.Services.Exceptions.ApiException ex) when (ex.IsInsufficientFunds)
            {
                Cursor = Cursors.Default;
                lblError.Text = "Fondos insuficientes para cubrir monto y comisión.";
                lblError.Visible = true;
            }
            catch (ATM.Kiosk.Services.Exceptions.ApiException ex) when (ex.IsAccountNotFound)
            {
                Cursor = Cursors.Default;
                lblError.Text = "La cuenta destino no existe.";
                lblError.Visible = true;
            }
            catch (ATM.Kiosk.Services.Exceptions.ApiException ex) when (ex.IsDailyLimitExceeded)
            {
                Cursor = Cursors.Default;
                lblError.Text = "Ha excedido el límite diario de transferencia.";
                lblError.Visible = true;
            }
            catch (ATM.Kiosk.Services.Exceptions.ApiException ex)
            {
                Cursor = Cursors.Default;
                lblError.Text = ex.IsConnectionError ? "Sin conexión." : ex.Message;
                lblError.Visible = true;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                lblError.Text = "Error inesperado: " + ex.Message;
                lblError.Visible = true;
            }
        }

        private void btnBack_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtToAccount_TextChanged_1(object sender, EventArgs e)
        {

            try
            {
                Cursor = Cursors.WaitCursor;
                string holderName = _authService.StartAuth(Convert.ToInt32(txtToAccount.Text.Trim()));
                Cursor = Cursors.Default;

                lblTitular.Text = "Titular: " + holderName;
            }
            catch (ATM.Kiosk.Services.Exceptions.ApiException ex) when (ex.IsAccountNotFound)
            {
                Cursor = Cursors.Default;
                lblTitular.Text = "Cuenta no encontrada.";
                lblError.Visible = true;
            }
            catch (ATM.Kiosk.Services.Exceptions.ApiException ex) when (ex.IsAccountNotActive)
            {
                Cursor = Cursors.Default;
                lblTitular.Text = "La cuenta no está activa. Contacte al banco.";
                lblError.Visible = true;
            }
            catch (ATM.Kiosk.Services.Exceptions.ApiException ex) when (ex.IsConnectionError)
            {
                Cursor = Cursors.Default;
                lblTitular.Text = "Error de conexión con el servidor";   // <-- mostrar el mensaje real
                lblError.Visible = true;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                lblTitular.Text = "Error inesperado: " + ex.Message;
                lblError.Visible = true;
            }
        }
    }
}
