using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using ATM.Shared.DTOs.BackOffice;
using BackOffice.Helpers;
using BackOffice.Services.Implementations;

namespace BackOffice.Forms
{
    public partial class AuditLogForm : Form
    {
        private List<AuditLogItemDto> _currentItems = new List<AuditLogItemDto>();
        public AuditLogForm()
        {
            InitializeComponent();
            dgvLogs.AutoGenerateColumns = false;
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            bool isDark = BackOfficeColors.CurrentTheme == ThemeType.Dark;

            this.BackColor = BackOfficeColors.Background;

            label1.ForeColor = isDark ? Color.FromArgb(180, 180, 180) : Color.FromArgb(64, 64, 64);
            label2.ForeColor = isDark ? Color.FromArgb(180, 180, 180) : Color.FromArgb(64, 64, 64);
            label3.ForeColor = isDark ? Color.White : Color.FromArgb(64, 64, 64);
            lblStatus.ForeColor = isDark ? Color.FromArgb(0, 200, 83) : Color.FromArgb(112, 121, 112);

            dtpFrom.CalendarTrailingForeColor = isDark ? Color.White : Color.Black;
            dtpTo.CalendarTrailingForeColor = isDark ? Color.White : Color.Black;

            cmbLevel.BackColor = isDark ? Color.FromArgb(50, 50, 50) : Color.White;
            cmbLevel.ForeColor = isDark ? Color.White : Color.FromArgb(57, 95, 59);

            btnFilter.BackColor = isDark ? Color.FromArgb(50, 50, 50) : Color.White;
            btnFilter.ForeColor = isDark ? Color.White : Color.FromArgb(57, 95, 59);
            btnFilter.IconColor = isDark ? Color.White : Color.Black;

            btnExport.BackColor = isDark ? Color.FromArgb(50, 50, 50) : Color.White;
            btnExport.ForeColor = isDark ? Color.White : Color.FromArgb(57, 95, 59);

            dgvLogs.BackgroundColor = isDark ? Color.FromArgb(30, 30, 30) : Color.FromArgb(240, 241, 236);
            dgvLogs.DefaultCellStyle.BackColor = isDark ? Color.FromArgb(40, 40, 40) : Color.FromArgb(192, 201, 191);
            dgvLogs.DefaultCellStyle.ForeColor = isDark ? Color.White : Color.FromArgb(41, 79, 45);
            dgvLogs.DefaultCellStyle.SelectionBackColor = isDark ? Color.FromArgb(0, 150, 80) : Color.FromArgb(64, 73, 65);
            dgvLogs.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvLogs.ColumnHeadersDefaultCellStyle.BackColor = isDark ? Color.FromArgb(45, 45, 45) : Color.FromArgb(89, 128, 90);
            dgvLogs.ColumnHeadersDefaultCellStyle.ForeColor = isDark ? Color.White : Color.FromArgb(234, 243, 232);
            dgvLogs.RowHeadersDefaultCellStyle.BackColor = isDark ? Color.FromArgb(35, 35, 35) : Color.FromArgb(89, 128, 90);
            dgvLogs.RowHeadersDefaultCellStyle.ForeColor = isDark ? Color.White : Color.FromArgb(234, 243, 232);
            dgvLogs.GridColor = isDark ? Color.FromArgb(60, 60, 60) : Color.FromArgb(0, 33, 7);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ConfigureFilters();
            LoadLogs();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (_currentItems == null || _currentItems.Count == 0)
            {
                MessageBox.Show("No hay registros para exportar.",
                    "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "Archivo de texto (*.txt)|*.txt";
                dialog.FileName = "AuditLog_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";

                if (dialog.ShowDialog() != DialogResult.OK) return;

                try
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("=== AUDIT LOG — A2 BANKING ===");
                    sb.AppendLine("Exportado: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    sb.AppendLine("Registros: " + _currentItems.Count);
                    sb.AppendLine(new string('=', 80));
                    sb.AppendLine();

                    foreach (var item in _currentItems)
                    {
                        sb.AppendLine(
                            "[" + item.Timestamp.ToString("yyyy-MM-dd HH:mm:ss") + "] " +
                            "[" + (item.Level ?? "").PadRight(7) + "] " +
                            "[" + (item.Source ?? "").PadRight(10) + "] " +
                            item.Message);

                        if (!string.IsNullOrEmpty(item.HolderName))
                            sb.AppendLine("  Cliente: " + item.HolderName);
                        if (!string.IsNullOrEmpty(item.AdminName))
                            sb.AppendLine("  Admin:   " + item.AdminName);
                    }

                    File.WriteAllText(dialog.FileName, sb.ToString(), Encoding.UTF8);

                    lblStatus.Text = "Exportado: " + dialog.FileName;
                    MessageBox.Show("Log exportado correctamente.", "Exportar",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al exportar: " + ex.Message,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        private void ConfigureFilters()
        {
            // Fechas por defecto: último mes
            dtpFrom.Value = DateTime.Today.AddDays(-30);
            dtpTo.Value = DateTime.Today.AddDays(1);

            // Niveles disponibles
            cmbLevel.Items.Clear();
            cmbLevel.Items.Add("Todos");
            cmbLevel.Items.Add("INFO");
            cmbLevel.Items.Add("WARNING");
            cmbLevel.Items.Add("ERROR");
            cmbLevel.SelectedIndex = 0;
        }
        private void LoadLogs()
        {
            lblStatus.Text = "Cargando...";

            try
            {
                Cursor = Cursors.WaitCursor;

                int? levelId = null;
                if (cmbLevel.SelectedIndex > 0)
                    levelId = cmbLevel.SelectedIndex;  // 1=INFO, 2=WARNING, 3=ERROR

                var request = new AuditLogRequest
                {
                    FromDate = dtpFrom.Value.Date,
                    ToDate = dtpTo.Value.Date,
                    LevelId = levelId
                };

                var response = AppServices.ApiClient.GetLogs(request);
                _currentItems = response.Items;
                dgvLogs.DataSource = _currentItems;

                Cursor = Cursors.Default;
                lblStatus.Text = _currentItems.Count + " registro(s) encontrado(s).";
            }
            catch (BackOfficeApiException ex)
            {
                Cursor = Cursors.Default;
                lblStatus.Text = "Error al cargar logs: " + ex.Message;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                lblStatus.Text = "Error inesperado: " + ex.Message;
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            LoadLogs();
        }

        private void exportToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvLogs.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DataGridViewExporter.ExportToExcel(dgvLogs, "Audit Log");
        }

        private void exportToPdfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvLogs.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DataGridViewExporter.ExportToPdf(dgvLogs, "Audit Log");
        }
    }
}
