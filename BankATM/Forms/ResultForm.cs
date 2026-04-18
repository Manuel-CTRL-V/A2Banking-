using ATM.Kiosk.Business.Strategies;
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
    public partial class ResultForm : Form
    {
        private readonly TransactionResult _result;
        private int _secondsLeft = 5;
        public ResultForm(TransactionResult result)
        {
            InitializeComponent();
            _result = result;

        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ShowResult();
            timerAutoReturn.Start();
        }

        private void ShowResult()
        {
            if (_result.Success)
            {
                lblResult.Text = "OPERACIÓN EXITOSA";
                lblResult.ForeColor = ATM.Kiosk.Helpers.ATMColors.TextSuccess;
                picIcon.BackColor = ATM.Kiosk.Helpers.ATMColors.TextSuccess;

                var detail = "Nuevo saldo: " + _result.NewBalance.ToString("C");

                if (_result.CommissionCharged)
                    detail += "\nComisión aplicada: " + _result.CommissionApplied.ToString("C");

                if (_result.TransactionId > 0)
                    detail += "\nTransacción #" + _result.TransactionId;

                lblDetail.Text = detail;
            }
            else
            {
                lblResult.Text = "OPERACIÓN FALLIDA";
                lblResult.ForeColor = ATM.Kiosk.Helpers.ATMColors.TextError;
                picIcon.BackColor = ATM.Kiosk.Helpers.ATMColors.TextError;
                lblDetail.Text = _result.Message;
            }

            UpdateCountdown();
        }

        private void timerAutoReturn_Tick(object sender, EventArgs e)
        {
            _secondsLeft--;
            UpdateCountdown();

            if (_secondsLeft <= 0)
            {
                timerAutoReturn.Stop();
                ReturnToMenu();
            }
        }
        private void UpdateCountdown()
        {
            lblCountdown.Text = "Regresando al menú en " + _secondsLeft + " segundos...";
        }

        private void ReturnToMenu()
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            timerAutoReturn?.Stop();
            base.OnFormClosing(e);
        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            timerAutoReturn.Stop();
            this.DialogResult = DialogResult.Abort;
            this.Close();
        }
    }

}
