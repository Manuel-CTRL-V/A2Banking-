using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ATM.Shared.DTOs.BackOffice;
using BackOffice.Helpers;
using BackOffice.Services.Implementations;

namespace BackOffice.Forms
{
    public partial class StatisticsForm : Form
    {
        public StatisticsForm()
        {
            InitializeComponent();
            ApplyTheme();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ConfigureGrids();
            ConfigureCharts();

            dtpFrom.Value = DateTime.Today.AddDays(-30);
            dtpTo.Value = DateTime.Today.AddDays(1);

            LoadStatistics();
        }

        private void ApplyTheme()
        {
            bool isDark = BackOfficeColors.CurrentTheme == ThemeType.Dark;

            this.BackColor = BackOfficeColors.Background;

            lblStatus.ForeColor = isDark ? Color.FromArgb(0, 200, 83) : Color.FromArgb(112, 121, 112);

            btnRefresh.BackColor = isDark ? Color.FromArgb(50, 50, 50) : Color.White;
            btnRefresh.ForeColor = isDark ? Color.White : Color.Black;
            btnRefresh.IconColor = isDark ? Color.White : Color.Black;

            chartByType.BackColor = isDark ? Color.FromArgb(40, 40, 40) : Color.White;
            chartByAccount.BackColor = isDark ? Color.FromArgb(40, 40, 40) : Color.White;

            dgvTransactions.BackgroundColor = isDark ? Color.FromArgb(30, 30, 30) : Color.FromArgb(240, 241, 236);
            dgvTransactions.DefaultCellStyle.BackColor = isDark ? Color.FromArgb(40, 40, 40) : Color.White;
            dgvTransactions.DefaultCellStyle.ForeColor = isDark ? Color.White : Color.Black;
            dgvTransactions.ColumnHeadersDefaultCellStyle.BackColor = isDark ? Color.FromArgb(45, 45, 45) : Color.FromArgb(89, 128, 90);
            dgvTransactions.ColumnHeadersDefaultCellStyle.ForeColor = isDark ? Color.White : Color.White;
            dgvTransactions.GridColor = isDark ? Color.FromArgb(60, 60, 60) : Color.LightGray;

            dgvAccounts.BackgroundColor = isDark ? Color.FromArgb(30, 30, 30) : Color.FromArgb(240, 241, 236);
            dgvAccounts.DefaultCellStyle.BackColor = isDark ? Color.FromArgb(40, 40, 40) : Color.White;
            dgvAccounts.DefaultCellStyle.ForeColor = isDark ? Color.White : Color.Black;
            dgvAccounts.ColumnHeadersDefaultCellStyle.BackColor = isDark ? Color.FromArgb(45, 45, 45) : Color.FromArgb(89, 128, 90);
            dgvAccounts.ColumnHeadersDefaultCellStyle.ForeColor = isDark ? Color.White : Color.White;
            dgvAccounts.GridColor = isDark ? Color.FromArgb(60, 60, 60) : Color.LightGray;

            foreach (var label in new[] { label6, label7, label8, label9 })
            {
                label.ForeColor = isDark ? Color.FromArgb(180, 180, 180) : Color.FromArgb(64, 64, 64);
            }
            foreach (var label in new[] { lblTotalAccountsVal, lblTotalDepositsVal, lblTotalWithdrawsVal, lblTotalCommissionsVal })
            {
                label.ForeColor = isDark ? Color.White : Color.FromArgb(41, 79, 45);
            }

            kryptonGroup1.StateCommon.Back.Color1 = isDark ? Color.FromArgb(45, 45, 45) : Color.White;
            kryptonGroup1.StateCommon.Border.Color1 = isDark ? Color.FromArgb(60, 60, 60) : Color.White;

            tabControl1.BackColor = isDark ? Color.FromArgb(40, 40, 40) : Color.White;
            tabPage1.BackColor = isDark ? Color.FromArgb(35, 35, 35) : Color.FromArgb(240, 241, 236);
            tabPage2.BackColor = isDark ? Color.FromArgb(35, 35, 35) : Color.FromArgb(240, 241, 236);
            tabPage3.BackColor = isDark ? Color.FromArgb(35, 35, 35) : Color.FromArgb(240, 241, 236);
        }

        private void ConfigureGrids()
        {
            dgvTransactions.AutoGenerateColumns = false;
            dgvTransactions.Columns.Clear();
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TransactionDate",
                HeaderText = "Fecha",
                Width = 110,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd" }
            });
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TransactionType",
                HeaderText = "Tipo",
                Width = 120
            });
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Quantity",
                HeaderText = "Cantidad",
                Width = 90
            });
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TotalAmount",
                HeaderText = "Monto total",
                Width = 140,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });

            dgvAccounts.AutoGenerateColumns = false;
            dgvAccounts.Columns.Clear();
            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "StatusName",
                HeaderText = "Estado",
                Width = 110
            });
            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "AccountType",
                HeaderText = "Tipo de cuenta",
                Width = 140
            });
            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Quantity",
                HeaderText = "Cantidad",
                Width = 90
            });
            dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TotalBalance",
                HeaderText = "Saldo total",
                Width = 140,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });
        }

        private void ConfigureCharts()
        {
            ConfigureBarChart();
            ConfigurePieChart();
        }

        // Gráfica de barras: montos por tipo de transacción

        private void ConfigureBarChart()
        {
            chartByType.Titles.Clear();
            chartByType.Titles.Add(new Title(
                "Montos por tipo de transacción",
                Docking.Top,
                new Font("Segoe UI", 10f, FontStyle.Bold),
                Color.FromArgb(33, 37, 41)));

            chartByType.ChartAreas.Clear();
            var area = new ChartArea("main");
            area.AxisX.MajorGrid.LineColor = Color.FromArgb(220, 220, 220);
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(220, 220, 220);
            area.AxisY.LabelStyle.Format = "C0";
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 8f);
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 8f);
            area.BackColor = Color.White;
            area.BorderColor = Color.FromArgb(200, 200, 200);
            area.BorderWidth = 1;
            chartByType.ChartAreas.Add(area);

            chartByType.Legends.Clear();
            chartByType.BackColor = Color.White;

            // Series se crean vacías — se llenan en UpdateBarChart()
            chartByType.Series.Clear();
            AddBarSeries("Depósitos", Color.FromArgb(40, 167, 69));
            AddBarSeries("Retiros", Color.FromArgb(220, 53, 69));
            AddBarSeries("Transferencias", Color.FromArgb(0, 123, 255));
        }

        private void AddBarSeries(string name, Color color)
        {
            var s = new Series(name)
            {
                ChartType = SeriesChartType.Bar,
                Color = color,
                IsValueShownAsLabel = true,
                LabelFormat = "C0",
                Font = new Font("Segoe UI", 8f)
            };
            chartByType.Series.Add(s);
        }

        private void UpdateBarChart(StatisticsSummary summary)
        {
            if (summary == null) return;

            chartByType.Series["Depósitos"].Points.Clear();
            chartByType.Series["Retiros"].Points.Clear();
            chartByType.Series["Transferencias"].Points.Clear();

            chartByType.Series["Depósitos"].Points.AddXY(
                "Depósitos", (double)summary.TotalDeposits);
            chartByType.Series["Retiros"].Points.AddXY(
                "Retiros", (double)summary.TotalWithdraws);
            chartByType.Series["Transferencias"].Points.AddXY(
                "Transferencias", (double)summary.TotalTransfers);
        }

        // Gráfica de torta: cuentas por tipo

        private void ConfigurePieChart()
        {
            chartByAccount.Titles.Clear();
            chartByAccount.Titles.Add(new Title(
                "Distribución de cuentas por tipo",
                Docking.Top,
                new Font("Segoe UI", 10f, FontStyle.Bold),
                Color.FromArgb(33, 37, 41)));

            chartByAccount.ChartAreas.Clear();
            var area = new ChartArea("main");
            area.BackColor = Color.White;
            chartByAccount.ChartAreas.Add(area);

            chartByAccount.Legends.Clear();
            var legend = new Legend("main")
            {
                Docking = Docking.Bottom,
                Alignment = StringAlignment.Center,
                Font = new Font("Segoe UI", 8f)
            };
            chartByAccount.Legends.Add(legend);
            chartByAccount.BackColor = Color.White;

            // Serie única de tipo Pie
            chartByAccount.Series.Clear();
            var s = new Series("Cuentas")
            {
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = true,
                LabelFormat = "#,##0",
                Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                Legend = "main"
            };
            s["PieLabelStyle"] = "Outside";
            s["PieLineColor"] = "Black";
            chartByAccount.Series.Add(s);
        }

        private void UpdatePieChart(System.Collections.Generic.List<AccountStatItem> accounts)
        {
            chartByAccount.Series["Cuentas"].Points.Clear();

            if (accounts == null || accounts.Count == 0) return;

            // Agrupar por tipo sumando las cantidades de todos los estados
            var totals = new System.Collections.Generic.Dictionary<string, int>();
            foreach (var item in accounts)
            {
                if (!totals.ContainsKey(item.AccountType))
                    totals[item.AccountType] = 0;
                totals[item.AccountType] += item.Quantity;
            }

            var palette = new[]
            {
                Color.FromArgb(0,  123, 255),
                Color.FromArgb(40, 167, 69),
                Color.FromArgb(255,193,   7),
                Color.FromArgb(220, 53,  69)
            };

            int i = 0;
            foreach (var kvp in totals)
            {
                if (kvp.Value == 0) { i++; continue; }
                var pt = new DataPoint();
                pt.SetValueXY(kvp.Key, kvp.Value);
                pt.Label = kvp.Key + "\n" + kvp.Value;
                pt.Color = palette[i % palette.Length];
                pt.LegendText = kvp.Key + " (" + kvp.Value + ")";
                chartByAccount.Series["Cuentas"].Points.Add(pt);
                i++;
            }
        }

        // ── Cargar y actualizar todo ──────────────────────────────────

        private void LoadStatistics()
        {
            lblStatus.Text = "Cargando estadísticas...";

            try
            {
                Cursor = Cursors.WaitCursor;

                var request = new StatisticsRequest
                {
                    FromDate = dtpFrom.Value.Date,
                    ToDate = dtpTo.Value.Date
                };

                var response = AppServices.ApiClient.GetStatistics(request);

                UpdateSummaryCards(response.Summary);
                dgvTransactions.DataSource = response.Transactions;
                dgvAccounts.DataSource = response.Accounts;
                UpdateBarChart(response.Summary);
                UpdatePieChart(response.Accounts);

                Cursor = Cursors.Default;
                lblStatus.Text = "Datos actualizados: " +
                                 DateTime.Now.ToString("HH:mm:ss");
            }
            catch (BackOfficeApiException ex)
            {
                Cursor = Cursors.Default;
                lblStatus.Text = "Error: " + ex.Message;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                lblStatus.Text = "Error inesperado: " + ex.Message;
            }
        }

        private void UpdateSummaryCards(StatisticsSummary summary)
        {
            if (summary == null) return;

            lblTotalAccountsVal.Text = summary.ActiveAccounts + " / " + summary.TotalAccounts;
            lblTotalDepositsVal.Text = summary.TotalDeposits.ToString("C");
            lblTotalWithdrawsVal.Text = summary.TotalWithdraws.ToString("C");
            lblTotalCommissionsVal.Text = summary.TotalCommissions.ToString("C");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadStatistics();
        }

        private void exportToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1 && dgvTransactions.Rows.Count > 0)
                DataGridViewExporter.ExportToExcel(dgvTransactions, "Transacciones");
            else if (tabControl1.SelectedTab == tabPage2 && dgvAccounts.Rows.Count > 0)
                DataGridViewExporter.ExportToExcel(dgvAccounts, "Cuentas");
            else
                MessageBox.Show("No hay datos para exportar en la pestaña actual.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void exportToPdfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1 && dgvTransactions.Rows.Count > 0)
                DataGridViewExporter.ExportToPdf(dgvTransactions, "Transacciones");
            else if (tabControl1.SelectedTab == tabPage2 && dgvAccounts.Rows.Count > 0)
                DataGridViewExporter.ExportToPdf(dgvAccounts, "Cuentas");
            else
                MessageBox.Show("No hay datos para exportar en la pestaña actual.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
