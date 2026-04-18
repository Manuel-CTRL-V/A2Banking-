using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ATM.Kiosk.Helpers
{
    public static class UIHelper
    {
        public static void StylePrimaryButton(Button btn)
        {
            btn.BackColor = ATMColors.ButtonPrimary;
            btn.ForeColor = ATMColors.ButtonPrimaryText;
            btn.Font = ATMFonts.Button;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Cursor = Cursors.Hand;
            btn.FlatAppearance.BorderSize = 0;
        }

        public static void StyleDangerButton(Button btn)
        {
            btn.BackColor = ATMColors.ButtonDanger;
            btn.ForeColor = ATMColors.ButtonDangerText;
            btn.Font = ATMFonts.Button;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Cursor = Cursors.Hand;
            btn.FlatAppearance.BorderSize = 0;
        }

        public static void StyleSuccessButton(Button btn)
        {
            btn.BackColor = ATMColors.ButtonSuccess;
            btn.ForeColor = ATMColors.ButtonSuccessText;
            btn.Font = ATMFonts.Button;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Cursor = Cursors.Hand;
            btn.FlatAppearance.BorderSize = 0;
        }

        public static void StyleSecondaryButton(Button btn)
        {
            btn.BackColor = ATMColors.ButtonSecondary;
            btn.ForeColor = ATMColors.ButtonSecondaryText;
            btn.Font = ATMFonts.Button;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Cursor = Cursors.Hand;
            btn.FlatAppearance.BorderSize = 0;
        }

        public static void ApplyThemeToForm(Form form)
        {
            form.BackColor = ATMColors.Background;
            form.ForeColor = ATMColors.TextPrimary;

            foreach (Control control in form.Controls)
            {
                ApplyThemeToControl(control);
            }
        }

        public static void ApplyThemeToControl(Control control)
        {
            if (control is Button btn)
            {
                if (btn.Tag?.ToString() == "primary")
                    StylePrimaryButton(btn);
                else if (btn.Tag?.ToString() == "danger")
                    StyleDangerButton(btn);
                else if (btn.Tag?.ToString() == "success")
                    StyleSuccessButton(btn);
                else if (btn.Tag?.ToString() == "secondary")
                    StyleSecondaryButton(btn);
                else
                    StylePrimaryButton(btn);
            }
            else if (control is Panel panel)
            {
                panel.BackColor = ATMColors.CardBackground;
                panel.ForeColor = ATMColors.TextPrimary;
                foreach (Control child in panel.Controls)
                {
                    ApplyThemeToControl(child);
                }
            }
            else if (control is Label label)
            {
                label.ForeColor = ATMColors.TextPrimary;
            }
            else if (control is TextBox textBox)
            {
                textBox.BackColor = ATMColors.InputBackground;
                textBox.ForeColor = ATMColors.InputText;
            }
            else if (control is DataGridView dataGrid)
            {
                ApplyThemeToDataGridView(dataGrid);
            }
            else if (control is MenuStrip menuStrip)
            {
                menuStrip.BackColor = ATMColors.MenuBackground;
                menuStrip.ForeColor = ATMColors.MenuText;
            }
            else if (control.HasChildren)
            {
                foreach (Control child in control.Controls)
                {
                    ApplyThemeToControl(child);
                }
            }
        }

        public static void ApplyThemeToDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = ATMColors.Background;
            dgv.DefaultCellStyle.BackColor = ATMColors.CardBackground;
            dgv.DefaultCellStyle.ForeColor = ATMColors.TextPrimary;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = ATMColors.BackgroundSecondary;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = ATMColors.TextPrimary;
            dgv.RowHeadersDefaultCellStyle.BackColor = ATMColors.BackgroundSecondary;
            dgv.RowHeadersDefaultCellStyle.ForeColor = ATMColors.TextPrimary;
            dgv.GridColor = ATMColors.Border;
            dgv.BorderStyle = BorderStyle.None;
        }

        public static void RoundCorners(Control control, int radius)
        {
            var path = new GraphicsPath();
            var rect = control.ClientRectangle;
            int d = radius * 2;

            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();

            control.Region = new Region(path);
        }

        public static void ApplyDarkModeIcon(Control control)
        {
            if (ATMColors.CurrentTheme == ThemeType.Dark && control is FontAwesome.Sharp.IconButton iconBtn)
            {
                iconBtn.IconColor = ATMColors.TextPrimary;
            }
        }
    }
}
