using ATM.Kiosk.Business.Context;
using ATM.Kiosk.Business.Exceptions;
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
    public partial class BalanceForm : Form
    {
        private readonly TransactionContext _txContext;
        public BalanceForm(TransactionContext txContext)
        {
            InitializeComponent();
            _txContext = txContext;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadBalance();
        }

        private void LoadBalance()
        {
            lblBalance.Text = "Consultando...";
            lblError.Visible = false;

            try
            {
                var result = _txContext.ExecuteGetBalance();
                lblBalance.Text = result.NewBalance.ToString("C");
                lblStatus.Text = "Consulta exitosa"; 
                lblStatus.ForeColor = ATM.Kiosk.Helpers.ATMColors.TextSuccess;
            }
            catch (ApiException ex)
            {
                lblBalance.Text = "--";
                lblError.Text = ex.IsConnectionError
                    ? "Sin conexión con el servidor."
                    : ex.Message;
                lblError.Visible = true;
                lblStatus.Text = "Error al consultar saldo";
                lblStatus.ForeColor = ATM.Kiosk.Helpers.ATMColors.TextError;
            }
            catch (Exception ex)
            {
                lblBalance.Text = "--";
                lblError.Text = "Error: " + ex.Message;
                lblError.Visible = true;
            }
        }

        private void btnBack_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
