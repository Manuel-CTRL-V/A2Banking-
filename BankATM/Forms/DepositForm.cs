using ATM.Kiosk.Business.Auth;
using ATM.Kiosk.Business.Context;
using ATM.Kiosk.Services.Exceptions;
using Krypton.Toolkit;
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
using ATM.Kiosk.Business.Exceptions;

namespace BankATM.Forms
{
    public partial class DepositForm : Form
    {
        private readonly TransactionContext _txContext;
        public DepositForm(TransactionContext txContext)
        {
            InitializeComponent();
            _txContext = txContext;
            _txContext = txContext;

            // Mostrar saldo actual desde sesión (sin ir al servidor)
            var session = SessionManager.Instance.Current;
            if (session != null)
                lblBalance.Text = "Saldo actual: " + session.CurrentBalance.ToString("C");

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
                var result = _txContext.ExecuteDeposit(amount);
                Cursor = Cursors.Default;

                using (var resultForm = new ResultForm(result))
                    resultForm.ShowDialog(this);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (BusinessException ex)
            {
                Cursor = Cursors.Default;
                lblError.Text = "Excepcion de negoio: " + ex.Message;
                lblError.Visible = true;
            }
            catch (ATM.Kiosk.Services.Exceptions.ApiException ex) when (ex.IsDailyLimitExceeded)
            {
                Cursor = Cursors.Default;
                lblError.Text = "Ha excedido el límite diario de depósito.";
                lblError.Visible = true;
            }
            catch (ATM.Kiosk.Services.Exceptions.ApiException ex)
            {
                Cursor = Cursors.Default;
                lblError.Text = ex.IsConnectionError
                    ? "Sin conexión con el servidor."
                    : ex.Message;
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

        private void DepositForm_Load(object sender, EventArgs e)
        {
            btnConfirm.StateCommon.Content.Padding = new Padding(-2, 0, 0, 0);
        }
    }
}
