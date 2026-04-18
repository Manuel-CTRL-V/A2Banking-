using ATM.Kiosk.Business.Auth;
using ATM.Kiosk.Business.Context;
using ATM.Kiosk.Services.Exceptions;
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
    public partial class WithdrawForm : Form
    {
        private readonly TransactionContext _txContext;
        public WithdrawForm(TransactionContext txContext)
        {
            InitializeComponent();

            _txContext = txContext;

            var session = SessionManager.Instance.Current;
            if (session != null)
                lblBalance.Text = "Saldo disponible: " + session.CurrentBalance.ToString("C");

            txtAmount.KeyPress += OnlyDecimals_KeyPress;
        }

        private void btnQuick500_Click(object sender, EventArgs e) => SetAmount(500);
        private void btnQuick1000_Click(object sender, EventArgs e) => SetAmount(1000);
        private void btnQuick2000_Click(object sender, EventArgs e) => SetAmount(2000);
        private void btnQuick5000_Click(object sender, EventArgs e) => SetAmount(5000);

        private void SetAmount(decimal amount)
        {
            txtAmount.Text = amount.ToString("F2");
            SessionManager.Instance.RecordActivity();
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtAmount.Text, out decimal amount))
                lblAmountDisplay.Text = amount.ToString("C");
            else
                lblAmountDisplay.Text = "RD$0.00";
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
                var result = _txContext.ExecuteWithdraw(amount);
                Cursor = Cursors.Default;

                using (var resultForm = new ResultForm(result))
                    resultForm.ShowDialog(this);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (BusinessException ex) when (ex.Code == BusinessErrorCode.InvalidAmount)
            {
                Cursor = Cursors.Default;
                lblError.Text = ex.Message;
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
                lblError.Text = "Fondos insuficientes.";
                lblError.Visible = true;
            }
            catch (ATM.Kiosk.Services.Exceptions.ApiException ex) when (ex.IsDailyLimitExceeded)
            {
                Cursor = Cursors.Default;
                lblError.Text = "Ha excedido el límite diario de retiro.";
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

    }
}

