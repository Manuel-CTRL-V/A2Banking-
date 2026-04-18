using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BackOffice.Helpers
{
    public static class DataGridViewExporter
    {
        public static void ExportToExcel(DataGridView dgv, string title)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:x='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'>");
            sb.AppendLine("<head><meta charset='utf-8'><title>" + title + "</title></head>");
            sb.AppendLine("<body>");
            sb.AppendLine("<table border='1' style='border-collapse:collapse; font-family:Arial; font-size:12px;'>");

            sb.AppendLine("<tr style='background-color:#365C36; color:white; font-weight:bold;'>");
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                sb.AppendLine("<th>" + col.HeaderText + "</th>");
            }
            sb.AppendLine("</tr>");

            foreach (DataGridViewRow row in dgv.Rows)
            {
                sb.AppendLine("<tr>");
                foreach (DataGridViewCell cell in row.Cells)
                {
                    var value = cell.Value?.ToString() ?? "";
                    sb.AppendLine("<td>" + System.Net.WebUtility.HtmlEncode(value) + "</td>");
                }
                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</table>");
            sb.AppendLine("</body></html>");

            var tempFile = Path.GetTempFileName() + ".xls";
            File.WriteAllText(tempFile, sb.ToString(), Encoding.UTF8);

            Process.Start(new ProcessStartInfo(tempFile) { UseShellExecute = true });
        }

        public static void ExportToPdf(DataGridView dgv, string title)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns='http://www.w3.org/TR/REC-html40'>");
            sb.AppendLine("<head><meta charset='utf-8'><title>" + title + "</title>");
            sb.AppendLine("<style>");
            sb.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; }");
            sb.AppendLine("h1 { color: #365C36; }");
            sb.AppendLine("table { border-collapse: collapse; width: 100%; }");
            sb.AppendLine("th { background-color: #365C36; color: white; padding: 8px; text-align: left; }");
            sb.AppendLine("td { border: 1px solid #ddd; padding: 8px; }");
            sb.AppendLine("tr:nth-child(even) { background-color: #f2f2f2; }");
            sb.AppendLine("@media print { body { margin: 0; } }");
            sb.AppendLine("</style></head><body>");
            sb.AppendLine("<h1>" + title + "</h1>");
            sb.AppendLine("<table>");

            sb.AppendLine("<tr>");
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                sb.AppendLine("<th>" + col.HeaderText + "</th>");
            }
            sb.AppendLine("</tr>");

            foreach (DataGridViewRow row in dgv.Rows)
            {
                sb.AppendLine("<tr>");
                foreach (DataGridViewCell cell in row.Cells)
                {
                    var value = cell.Value?.ToString() ?? "";
                    sb.AppendLine("<td>" + System.Net.WebUtility.HtmlEncode(value) + "</td>");
                }
                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</table>");
            sb.AppendLine("</body></html>");

            var tempFile = Path.GetTempFileName() + ".pdf.html";
            File.WriteAllText(tempFile, sb.ToString(), Encoding.UTF8);

            Process.Start(new ProcessStartInfo(tempFile) { UseShellExecute = true });
        }
    }
}
