using BackOffice.Helpers;
using Business.Auth;
using FontAwesome.Sharp;
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
    public partial class MainFormMenu : Form
    {
        private static IconMenuItem menuActivo = null;
        private static Form formActivo = null;
        public MainFormMenu()
        {
            InitializeComponent();

            BackOfficeColors.ThemeChanged += OnThemeChanged;
            ApplyTheme();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var admin = AdminSessionManager.Instance.CurrentAdmin;
            lblWelcome.Text = "Bienvenido, " + admin.Username;
            lblRole.Text = "Rol: " + admin.Role;
            ApplyPermissions();
        }
        /// <summary>
        /// Habilita o deshabilita cada botón del menú según
        /// los permisos del rol del admin autenticado.
        /// El nombre del permiso es el nombre exacto del formulario.
        /// </summary>
        private void ApplyPermissions()
        {
            var s = AdminSessionManager.Instance;

            menuOpenAccount.Enabled = s.HasPermission("CreateAccountForm");
            menuManageAccounts.Enabled = s.HasPermission("AccountListForm");
            menuViewAu.Enabled = s.HasPermission("AuditLogForm");
            menuViewStatistics.Enabled = s.HasPermission("StatisticsForm");
            opUserMaintenance.Enabled = s.HasPermission("UserManagementForm");
            opRoleMaintenance.Enabled = s.HasPermission("RoleManagementForm");
            menuAbout.Enabled = s.HasPermission("AboutForm");
            // btnLogout siempre habilitado — no es un permiso
        }


        private void AbrirFormulario(IconMenuItem menu, Form form)
        {
            if (menuActivo != null)
            {
                menuActivo.BackColor = BackOfficeColors.MenuBackground;
                menuActivo.ForeColor = BackOfficeColors.MenuText;
            }

            lblWelcome.Visible = false;
            lblRole.Visible = false;
            menu.BackColor = Color.FromArgb(88, 97, 88);
            menu.ForeColor = Color.FromArgb(240, 241, 236);
            menuActivo = menu;

            if (formActivo != null) formActivo.Close();

            formActivo = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.BackColor = BackOfficeColors.Background;

            panel1.Controls.Add(form);
            form.Show();

            KryptonThemeHelper.ApplyThemeToForm(form);
        }

        private void menuOpenAccount_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuOpenAccount, new CreateAccountForm());
        }

        private void menuManageAccounts_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuManageAccounts, new AccountListForm());
        }

        private void menuViewAu_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuViewAu, new AuditLogForm());
        }
        private void mnuViewStatistics_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuViewStatistics, new StatisticsForm());
        }
        private void menuAbout_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuAbout, new AboutForm());

        }
        private void opUserMaintenance_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuMaintenance, new UserManagementForm());
        }

        private void opRoleMaintenance_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuMaintenance, new RoleManagementForm());
        }
        private void iconButton1_Click(object sender, EventArgs e) => Application.Exit();

        private void btnTheme_Click(object sender, EventArgs e)
        {
            BackOfficeColors.ToggleTheme();
        }

        private void OnThemeChanged(ThemeType theme)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnThemeChanged(theme)));
                return;
            }
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            bool isDark = BackOfficeColors.CurrentTheme == ThemeType.Dark;

            this.BackColor = BackOfficeColors.Background;
            panel1.BackColor = BackOfficeColors.Background;

            menuStrip1.BackColor = BackOfficeColors.MenuBackground;
            menuStrip1.ForeColor = BackOfficeColors.MenuText;

            foreach (ToolStripItem item in menuStrip1.Items)
            {
                if (item is IconMenuItem menuItem)
                {
                    menuItem.ForeColor = BackOfficeColors.MenuText;
                    if (item != menuActivo)
                    {
                        menuItem.BackColor = BackOfficeColors.MenuBackground;
                    }
                }
            }

            lblWelcome.ForeColor = BackOfficeColors.TextPrimary;
            lblRole.ForeColor = BackOfficeColors.TextSecondary;

            iconButton1.BackColor = BackOfficeColors.MenuBackground;
            iconButton1.IconColor = isDark ? Color.White : Color.FromArgb(162, 35, 0);

            btnTheme.BackColor = BackOfficeColors.MenuBackground;
            btnTheme.IconChar = isDark ? FontAwesome.Sharp.IconChar.CloudSun : FontAwesome.Sharp.IconChar.Moon;
            btnTheme.IconColor = BackOfficeColors.TextPrimary;

            if (formActivo != null)
            {
                formActivo.BackColor = BackOfficeColors.Background;
                KryptonThemeHelper.ApplyThemeToForm(formActivo);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            BackOfficeColors.ThemeChanged -= OnThemeChanged;
            base.OnFormClosing(e);
        }
    }
}
