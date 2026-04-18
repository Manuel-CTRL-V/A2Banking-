using ATM.Kiosk.Business.Context;
using ATM.Kiosk.Business.Exceptions;
using ATM.Shared.DTOs.Transactions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BankATM.Forms
{
    public partial class MiniStatementForm : Form
    {
        private readonly TransactionContext _txContext;

        public MiniStatementForm(TransactionContext txContext)
        {
            InitializeComponent();
            _txContext = txContext;
        }

        private void MiniStatementForm_Load(object sender, EventArgs e)
        {
            LoadHistory();
        }

        private void LoadHistory()
        {
            lblError.Visible = false;
            dgvHistory.Rows.Clear();

            try
            {
                var items = _txContext.ExecuteGetHistory();

                if (items == null || items.Count == 0)
                {
                    lblError.Text = "No hay movimientos recientes.";
                    lblError.Visible = true;
                    return;
                }

                var last5 = items.OrderByDescending(x => x.Timestamp).Take(5).ToList();

                foreach (var item in last5)
                {
                    string typeLabel = GetTypeLabel(item.TransactionType);
                    string amountStr = item.Amount.ToString("C");
                    string dateStr = item.Timestamp.ToString("dd/MM/yyyy HH:mm");

                    dgvHistory.Rows.Add(dateStr, typeLabel, amountStr, item.BalanceAfter.ToString("C"));
                }

                StyleDataGridView();
            }
            catch (ApiException ex)
            {
                lblError.Text = ex.IsConnectionError
                    ? "Sin conexión con el servidor."
                    : ex.Message;
                lblError.Visible = true;
            }
            catch (Exception ex)
            {
                lblError.Text = "Error: " + ex.Message;
                lblError.Visible = true;
            }
        }

        private string GetTypeLabel(string type)
        {
            switch (type?.ToLower())
            {
                case "deposit": return "Depósito";
                case "withdraw": return "Retiro";
                case "transfer": return "Transferencia";
                default: return type ?? "Desconocido";
            }
        }

        private void StyleDataGridView()
        {
            dgvHistory.DefaultCellStyle.Font = new Font("Carlito", 10F);
            dgvHistory.DefaultCellStyle.ForeColor = Color.FromArgb(64, 64, 64);
            dgvHistory.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Carlito", 10F, FontStyle.Bold);
            dgvHistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(65, 103, 67);
            dgvHistory.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(234, 243, 232);

            dgvHistory.EnableHeadersVisualStyles = false;
            dgvHistory.RowHeadersVisible = false;

            foreach (DataGridViewRow row in dgvHistory.Rows)
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(241, 244, 242);
                row.Height = 45;
            }

            dgvHistory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(226, 233, 225);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
