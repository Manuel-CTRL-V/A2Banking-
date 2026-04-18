using ATM.Shared.DTOs.Maintenance;
using BackOffice.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BackOffice.Forms
{
    public partial class UserManagementForm : Form
    {
        private AdminUserDto _selected;
        private List<RoleDto> _roles = new List<RoleDto>();
        public UserManagementForm()
        {
            InitializeComponent();
            LoadRolesIntoCombo();
            LoadUsers();
            ClearForm();

            dgvUsers.SelectionChanged += DgvUsers_SelectionChanged;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
         
        }
        private void LoadRolesIntoCombo()
        {
            try
            {
                var response = AppServices.ApiClient.GetRoles();
                _roles = response.Roles;

                cmbRole.Items.Clear();
                foreach (var role in _roles)
                    cmbRole.Items.Add(role.RoleName);

                if (cmbRole.Items.Count > 0)
                    cmbRole.SelectedIndex = 0;
            }
            catch (BackOfficeApiException ex)
            {
                lblStatus.Text = "Error al cargar roles: " + ex.Message;
            }
        }

        private void LoadUsers(string search = "")
        {
            try
            {
                var response = AppServices.ApiClient.GetAdminUsers(search);
                dgvUsers.DataSource = response.Users;
                lblStatus.Text = response.Users.Count + " usuario(s) encontrado(s).";
            }
            catch (BackOfficeApiException ex)
            {
                lblStatus.Text = "Error: " + ex.Message;
            }
        }

        // ── Selección en el grid ──────────────────────────────────────

        private void DgvUsers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0) return;

            _selected = dgvUsers.SelectedRows[0].DataBoundItem as AdminUserDto;
            if (_selected == null) return;

            txtUsername.Text = _selected.Username;
            txtPassword.Text = "";
            txtPasswordConfirm.Text = "";
            chkIsActive.Checked = _selected.IsActive;
            lblError.Visible = false;

            // Seleccionar rol en combo
            for (int i = 0; i < _roles.Count; i++)
            {
                if (_roles[i].RoleId == _selected.RoleId)
                {
                    cmbRole.SelectedIndex = i;
                    break;
                }
            }

            // No permitir eliminar al usuario logueado actualmente
            
            var current = Business.Auth.AdminSessionManager
                .Instance.CurrentAdmin;
            btnDelete.Enabled = _selected.AdminId != current?.AdminId;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                lblError.Text = "El nombre de usuario es obligatorio.";
                lblError.Visible = true;
                return;
            }

            // Validar contraseña al crear o si se ingresó una nueva
            bool hasNewPassword = !string.IsNullOrEmpty(txtPassword.Text);

            if (_selected == null && !hasNewPassword)
            {
                lblError.Text = "La contraseña es obligatoria al crear un usuario.";
                lblError.Visible = true;
                return;
            }

            if (hasNewPassword && txtPassword.Text.Length < 6)
            {
                lblError.Text = "La contraseña debe tener al menos 6 caracteres.";
                lblError.Visible = true;
                return;
            }

            if (hasNewPassword && txtPassword.Text != txtPasswordConfirm.Text)
            {
                lblError.Text = "Las contraseñas no coinciden.";
                lblError.Visible = true;
                return;
            }

            if (cmbRole.SelectedIndex < 0 || _roles == null)
            {
                lblError.Text = "Seleccione un rol.";
                lblError.Visible = true;
                return;
            }

            int selectedRoleId = _roles[cmbRole.SelectedIndex].RoleId;

            try
            {
                Cursor = Cursors.WaitCursor;

                if (_selected == null)
                {
                    AppServices.ApiClient.CreateAdminUser(new CreateAdminUserRequest
                    {
                        Username = txtUsername.Text.Trim(),
                        Password = txtPassword.Text,
                        RoleId = selectedRoleId,
                        IsActive = chkIsActive.Checked
                    });
                    lblStatus.Text = "Usuario creado correctamente.";
                }
                else
                {
                    AppServices.ApiClient.UpdateAdminUser(new UpdateAdminUserRequest
                    {
                        AdminId = _selected.AdminId,
                        Username = txtUsername.Text.Trim(),
                        RoleId = selectedRoleId,
                        IsActive = chkIsActive.Checked,
                        NewPassword = hasNewPassword ? txtPassword.Text : null
                    });
                    lblStatus.Text = "Usuario actualizado correctamente.";
                }

                Cursor = Cursors.Default;
                LoadUsers();
                ClearForm();
            }
            catch (BackOfficeApiException ex)
            {
                Cursor = Cursors.Default;
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selected == null) return;

            var confirm = MessageBox.Show(
                "¿Desea eliminar al usuario \"" + _selected.Username + "\"?\n" +
                "Esta acción no se puede deshacer.",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                AppServices.ApiClient.DeleteAdminUser(_selected.AdminId);
                lblStatus.Text = "Usuario eliminado correctamente.";
                LoadUsers();
                ClearForm();
            }
            catch (BackOfficeApiException ex)
            {
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadUsers(txtSearch.Text.Trim());
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                LoadUsers(txtSearch.Text.Trim());
        }

        private void btnNew_Click(object sender, EventArgs e) => ClearForm();
        private void ClearForm()
        {
            _selected = null;
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtPasswordConfirm.Text = "";
            chkIsActive.Checked = true;
            lblError.Visible = false;
            btnDelete.Enabled = false;

            if (cmbRole.Items.Count > 0)
                cmbRole.SelectedIndex = 0;

            dgvUsers.ClearSelection();
        }
    }
}
