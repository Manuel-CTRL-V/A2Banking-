using Krypton.Toolkit;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BackOffice.Helpers
{
    public static class KryptonThemeHelper
    {
        public static void ApplyThemeToForm(Form form)
        {
            bool isDark = BackOfficeColors.CurrentTheme == ThemeType.Dark;

            form.BackColor = BackOfficeColors.Background;

            foreach (Control control in form.Controls)
            {
                ApplyThemeToControl(control, isDark);
            }
        }

        public static void ApplyThemeToControl(Control control, bool isDark)
        {
            if (control is KryptonGroup kryptonGroup)
            {
                ApplyThemeToKryptonGroup(kryptonGroup, isDark);
                foreach (Control child in kryptonGroup.Panel.Controls)
                {
                    ApplyThemeToControl(child, isDark);
                }
            }
            else if (control is KryptonPanel kryptonPanel)
            {
                ApplyThemeToKryptonPanel(kryptonPanel, isDark);
                foreach (Control child in kryptonPanel.Controls)
                {
                    ApplyThemeToControl(child, isDark);
                }
            }
            else if (control is KryptonTextBox kryptonTextBox)
            {
                ApplyThemeToKryptonTextBox(kryptonTextBox, isDark);
            }
            else if (control is KryptonButton kryptonButton)
            {
                ApplyThemeToKryptonButton(kryptonButton, isDark);
            }
            else if (control is KryptonComboBox kryptonComboBox)
            {
                ApplyThemeToKryptonComboBox(kryptonComboBox, isDark);
            }
            else if (control is DataGridView dataGridView)
            {
                ApplyThemeToDataGridView(dataGridView, isDark);
            }
            else if (control is Label label)
            {
                ApplyThemeToLabel(label, isDark);
            }
            else if (control is Button button)
            {
                ApplyThemeToButton(button, isDark);
            }
            else if (control is ComboBox comboBox)
            {
                ApplyThemeToComboBox(comboBox, isDark);
            }
            else if (control is FontAwesome.Sharp.IconButton iconButton)
            {
                ApplyThemeToIconButton(iconButton, isDark);
            }
            else if (control is Panel panel)
            {
                panel.BackColor = isDark ? Color.FromArgb(40, 40, 40) : Color.White;
                foreach (Control child in panel.Controls)
                {
                    ApplyThemeToControl(child, isDark);
                }
            }
            else if (control.HasChildren)
            {
                foreach (Control child in control.Controls)
                {
                    ApplyThemeToControl(child, isDark);
                }
            }
        }

        private static void ApplyThemeToKryptonGroup(KryptonGroup group, bool isDark)
        {
            if (isDark)
            {
                group.StateCommon.Back.Color1 = Color.FromArgb(45, 45, 45);
                group.StateCommon.Back.Color2 = Color.FromArgb(35, 35, 35);
                group.StateCommon.Border.Color1 = Color.FromArgb(60, 60, 60);
                group.StateCommon.Border.Color2 = Color.FromArgb(50, 50, 50);
            }
            else
            {
                group.StateCommon.Back.Color1 = Color.White;
                group.StateCommon.Back.Color2 = Color.White;
                group.StateCommon.Border.Color1 = Color.White;
                group.StateCommon.Border.Color2 = Color.White;
            }
        }

        private static void ApplyThemeToKryptonPanel(KryptonPanel panel, bool isDark)
        {
            if (isDark)
            {
                panel.StateCommon.Color1 = Color.FromArgb(35, 35, 35);
                panel.StateCommon.Color2 = Color.FromArgb(25, 25, 25);
            }
            else
            {
                panel.StateCommon.Color1 = Color.FromArgb(234, 239, 236);
                panel.StateCommon.Color2 = Color.White;
            }
        }

        private static void ApplyThemeToKryptonTextBox(KryptonTextBox textBox, bool isDark)
        {
            if (isDark)
            {
                textBox.StateCommon.Back.Color1 = Color.FromArgb(50, 50, 50);
                textBox.StateCommon.Border.Color1 = Color.FromArgb(60, 60, 60);
                textBox.StateCommon.Border.Color2 = Color.FromArgb(70, 70, 70);
                textBox.StateCommon.Content.Color1 = Color.FromArgb(220, 220, 220);
            }
            else
            {
                textBox.StateCommon.Back.Color1 = Color.FromArgb(220, 229, 218);
                textBox.StateCommon.Border.Color1 = Color.FromArgb(241, 244, 242);
                textBox.StateCommon.Border.Color2 = Color.FromArgb(255, 128, 128);
                textBox.StateCommon.Content.Color1 = Color.FromArgb(112, 121, 112);
            }
        }

        private static void ApplyThemeToKryptonButton(KryptonButton button, bool isDark)
        {
            if (isDark)
            {
                button.StateCommon.Back.Color1 = Color.FromArgb(0, 200, 83);
                button.StateCommon.Back.Color2 = Color.FromArgb(0, 180, 70);
                button.StateCommon.Border.Color1 = Color.FromArgb(0, 150, 60);
                button.StateCommon.Border.Color2 = Color.FromArgb(0, 150, 60);
                button.StateCommon.Content.ShortText.Color1 = Color.FromArgb(20, 20, 20);
                button.StateCommon.Content.ShortText.Color2 = Color.FromArgb(20, 20, 20);
            }
            else
            {
                button.StateCommon.Back.Color1 = Color.FromArgb(57, 95, 59);
                button.StateCommon.Back.Color2 = Color.FromArgb(57, 95, 59);
                button.StateCommon.Border.Color1 = Color.White;
                button.StateCommon.Border.Color2 = Color.White;
                button.StateCommon.Content.ShortText.Color1 = Color.White;
                button.StateCommon.Content.ShortText.Color2 = Color.White;
            }
        }

        private static void ApplyThemeToKryptonComboBox(KryptonComboBox comboBox, bool isDark)
        {
            if (isDark)
            {
                comboBox.StateCommon.ComboBox.Border.Color1 = Color.FromArgb(60, 60, 60);
                comboBox.StateCommon.ComboBox.Border.Color2 = Color.FromArgb(70, 70, 70);
                comboBox.StateCommon.ComboBox.Content.Color1 = Color.FromArgb(220, 220, 220);
            }
            else
            {
                comboBox.StateCommon.ComboBox.Border.Color1 = Color.FromArgb(180, 190, 175);
                comboBox.StateCommon.ComboBox.Border.Color2 = Color.FromArgb(160, 175, 150);
                comboBox.StateCommon.ComboBox.Content.Color1 = Color.FromArgb(57, 95, 59);
            }
        }

        private static void ApplyThemeToDataGridView(DataGridView dgv, bool isDark)
        {
            if (isDark)
            {
                dgv.BackgroundColor = Color.FromArgb(30, 30, 30);
                dgv.DefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
                dgv.DefaultCellStyle.ForeColor = Color.FromArgb(220, 220, 220);
                dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 150, 80);
                dgv.DefaultCellStyle.SelectionForeColor = Color.White;
                dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 45);
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgv.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(35, 35, 35);
                dgv.RowHeadersDefaultCellStyle.ForeColor = Color.White;
                dgv.GridColor = Color.FromArgb(60, 60, 60);
            }
            else
            {
                dgv.BackgroundColor = Color.FromArgb(240, 241, 236);
                dgv.DefaultCellStyle.BackColor = Color.FromArgb(192, 201, 191);
                dgv.DefaultCellStyle.ForeColor = Color.FromArgb(41, 79, 45);
                dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(64, 73, 65);
                dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(234, 243, 232);
                dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(89, 128, 90);
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(234, 243, 232);
                dgv.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(89, 128, 90);
                dgv.RowHeadersDefaultCellStyle.ForeColor = Color.FromArgb(234, 243, 232);
                dgv.GridColor = Color.FromArgb(0, 33, 7);
            }
        }

        private static void ApplyThemeToLabel(Label label, bool isDark)
        {
            label.ForeColor = isDark
                ? (label.Font.Bold ? Color.White : Color.FromArgb(180, 180, 180))
                : (label.Font.Bold ? Color.FromArgb(41, 79, 45) : Color.FromArgb(64, 64, 64));
        }

        private static void ApplyThemeToButton(Button button, bool isDark)
        {
            if (isDark)
            {
                button.BackColor = Color.FromArgb(50, 50, 50);
                button.ForeColor = Color.White;
                button.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 60);
            }
            else
            {
                button.BackColor = Color.White;
                button.ForeColor = Color.Black;
                button.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            }
        }

        private static void ApplyThemeToComboBox(ComboBox comboBox, bool isDark)
        {
            if (isDark)
            {
                comboBox.BackColor = Color.FromArgb(50, 50, 50);
                comboBox.ForeColor = Color.White;
            }
            else
            {
                comboBox.BackColor = Color.FromArgb(220, 229, 218);
                comboBox.ForeColor = Color.FromArgb(57, 95, 59);
            }
        }

        private static void ApplyThemeToIconButton(FontAwesome.Sharp.IconButton button, bool isDark)
        {
            if (isDark)
            {
                button.BackColor = Color.FromArgb(50, 50, 50);
                button.ForeColor = Color.White;
                button.IconColor = Color.White;
            }
            else
            {
                button.BackColor = Color.White;
                button.ForeColor = Color.Black;
                button.IconColor = Color.Black;
            }
        }
    }
}
