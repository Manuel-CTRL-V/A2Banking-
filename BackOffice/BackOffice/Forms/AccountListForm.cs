using ATM.Shared.DTOs.BackOffice;
using BackOffice.Helpers;
using BackOffice.Services.Implementations;
using Business.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackOffice.Forms
{
    public partial class AccountListForm : Form
    {
        private List<AccountSummary> _allAccounts;

        public AccountListForm()
        {
            InitializeComponent();
            dgvAccounts.AutoGenerateColumns = false;
            ApplyTheme();
            cmbStatusFilter.SelectedIndex = 0;
            LoadAccounts();
        }

        private void ApplyTheme()
        {
            bool isDark = BackOfficeColors.CurrentTheme == ThemeType.Dark;

            this.BackColor = BackOfficeColors.Background;

            label3.ForeColor = isDark ? Color.White : Color.FromArgb(64, 64, 64);
            lblStatus.ForeColor = isDark ? Color.FromArgb(0, 200, 83) : Color.FromArgb(112, 121, 112);
            lblFilter.ForeColor = isDark ? Color.White : Color.FromArgb(64, 64, 64);

            txtFilter.BackColor = isDark ? Color.FromArgb(50, 50, 50) : Color.White;
            txtFilter.ForeColor = isDark ? Color.White : Color.Black;
            txtFilter.BorderStyle = BorderStyle.FixedSingle;

            cmbStatusFilter.BackColor = isDark ? Color.FromArgb(50, 50, 50) : Color.White;
            cmbStatusFilter.ForeColor = isDark ? Color.White : Color.Black;

            btnSuspend.BackColor = isDark ? Color.FromArgb(50, 50, 50) : Color.AntiqueWhite;
            btnSuspend.ForeColor = isDark ? Color.Orange : Color.Orange;

            btnClose.BackColor = isDark ? Color.FromArgb(50, 50, 50) : Color.FromArgb(254, 241, 237);
            btnClose.ForeColor = isDark ? Color.FromArgb(255, 82, 82) : Color.FromArgb(162, 35, 0);

            btnRefresh.BackColor = isDark ? Color.FromArgb(50, 50, 50) : Color.FromArgb(192, 201, 191);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.IconColor = isDark ? Color.White : Color.FromArgb(41, 79, 45);

            dgvAccounts.BackgroundColor = isDark ? Color.FromArgb(30, 30, 30) : Color.FromArgb(240, 241, 236);
            dgvAccounts.DefaultCellStyle.BackColor = isDark ? Color.FromArgb(40, 40, 40) : Color.FromArgb(192, 201, 191);
            dgvAccounts.DefaultCellStyle.ForeColor = isDark ? Color.White : Color.FromArgb(41, 79, 45);
            dgvAccounts.DefaultCellStyle.SelectionBackColor = isDark ? Color.FromArgb(0, 150, 80) : Color.FromArgb(64, 73, 65);
            dgvAccounts.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvAccounts.ColumnHeadersDefaultCellStyle.BackColor = isDark ? Color.FromArgb(45, 45, 45) : Color.FromArgb(89, 128, 90);
            dgvAccounts.ColumnHeadersDefaultCellStyle.ForeColor = isDark ? Color.White : Color.FromArgb(234, 243, 232);
            dgvAccounts.RowHeadersDefaultCellStyle.BackColor = isDark ? Color.FromArgb(35, 35, 35) : Color.FromArgb(89, 128, 90);
            dgvAccounts.RowHeadersDefaultCellStyle.ForeColor = isDark ? Color.White : Color.FromArgb(234, 243, 232);
            dgvAccounts.GridColor = isDark ? Color.FromArgb(60, 60, 60) : Color.FromArgb(0, 33, 7);
        }


        private void LoadAccounts()
        {
            lblStatus.Text = "";
            try
            {
                Cursor = Cursors.WaitCursor;
                var response = AppServices.ApiClient.GetAccounts();
                _allAccounts = response.Accounts ?? new List<AccountSummary>();
                ApplyFilters();
                Cursor = Cursors.Default;
                UpdateSuspendButton();
            }
            catch (BackOfficeApiException ex)
            {
                Cursor = Cursors.Default;
                lblStatus.Text = "Error al cargar cuentas: " + ex.Message;
            }
        }

        private void ApplyFilters()
        {
            if (_allAccounts == null) return;

            var filtered = _allAccounts.AsEnumerable();

            var searchText = txtFilter.Text?.ToLower() ?? "";
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filtered = filtered.Where(a =>
                    a.AccountId.ToString().Contains(searchText) ||
                    (a.HolderName?.ToLower().Contains(searchText) ?? false) ||
                    (a.AccountType?.ToLower().Contains(searchText) ?? false));
            }

            var statusFilter = cmbStatusFilter.SelectedIndex;
            if (statusFilter > 0)
            {
                var statusMap = new[] { "", "Active", "Suspended", "Closed" };
                var selectedStatus = statusMap[statusFilter];
                filtered = filtered.Where(a => a.Status == selectedStatus);
            }

            dgvAccounts.DataSource = filtered.ToList();
            lblStatus.Text = $"Total: {dgvAccounts.Rows.Count} cuentas";
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
            => ApplyFilters();

        private void cmbStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
            => ApplyFilters();

        private void exportToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvAccounts.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DataGridViewExporter.ExportToExcel(dgvAccounts, "Cuentas Bancarias");
        }

        private void exportToPdfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvAccounts.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DataGridViewExporter.ExportToPdf(dgvAccounts, "Cuentas Bancarias");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
             => LoadAccounts();

        private void btnSuspend_Click(object sender, EventArgs e)
        {
            var account = GetSelectedAccount();
            if (account == null) return;

            if (account.Status == "Active")
                ChangeStatus(account, newStatusId: 3, action: "suspender");      // → Suspended
            else if (account.Status == "Suspended")
                ChangeStatus(account, newStatusId: 2, action: "reactivar");      // → Active
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            var account = GetSelectedAccount();
            if (account == null) return;
            ChangeStatus(account, newStatusId: 4, action: "cerrar definitivamente");
        }
        private void ChangeStatus(AccountSummary account, int newStatusId, string action)
        {
            var confirm = MessageBox.Show(
                "¿Desea " + action + " la cuenta de " + account.HolderName + "?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                var admin = AdminSessionManager.Instance.CurrentAdmin;
                AppServices.ApiClient.UpdateAccountStatus(
                    new UpdateAccountStatusRequest
                    {
                        AccountId = account.AccountId,
                        NewStatusId = newStatusId
                    }, admin.AdminId);

                lblStatus.Text = "Estado actualizado correctamente.";
                LoadAccounts();
            }
            catch (BackOfficeApiException ex)
            {
                lblStatus.Text = "Error: " + ex.Message;
            }
        }
        private void UpdateSuspendButton()
        {
            var account = GetSelectedAccount();
            if (account == null)
            {
                btnSuspend.Text = "Suspender cuenta";
                btnSuspend.Enabled = false;
                btnClose.Enabled = false;
                return;
            }

            btnClose.Enabled = account.Status != "Closed";

            switch (account.Status)
            {
                case "Active":
                    btnSuspend.Text = "Suspender cuenta";
                    btnSuspend.Enabled = true;
                    break;
                case "Suspended":
                    btnSuspend.Text = "Reactivar cuenta";
                    btnSuspend.Enabled = true;
                    break;
                default:
                    // Pending o Closed — no aplica suspender/reactivar
                    btnSuspend.Text = "Suspender cuenta";
                    btnSuspend.Enabled = false;
                    break;
            }
        }
        private AccountSummary GetSelectedAccount()
        {
            if (dgvAccounts.SelectedRows.Count == 0) return null;
            return dgvAccounts.SelectedRows[0].DataBoundItem as AccountSummary;
        }

        private void dgvAccounts_SelectionChanged(object sender, EventArgs e)
        {
            UpdateSuspendButton();
        }
    }
}
