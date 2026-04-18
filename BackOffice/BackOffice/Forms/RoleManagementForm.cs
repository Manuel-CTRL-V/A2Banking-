using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ATM.Shared.DTOs.Maintenance;
using BackOffice.Services.Implementations;

namespace BackOffice.Forms
{
    public partial class RoleManagementForm : Form
    {
        private RoleDto _selected;
        private List<PermissionDto> _allPermissions = new List<PermissionDto>();
        public RoleManagementForm()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadRoles();
            ClearForm();

            dgvRoles.SelectionChanged += DgvRoles_SelectionChanged;
        }

        private void LoadRoles()
        {
            try
            {
                var response = AppServices.ApiClient.GetRoles();

                // Poblar catálogo de permisos en el CheckedListBox
                _allPermissions = response.AllPermissions;
                clbPermissions.Items.Clear();
                foreach (var p in _allPermissions)
                    clbPermissions.Items.Add(p.Description ?? p.PermissionKey);

                dgvRoles.DataSource = response.Roles;
                lblStatus.Text = response.Roles.Count + " rol(es) encontrado(s).";
            }
            catch (BackOfficeApiException ex)
            {
                lblStatus.Text = "Error: " + ex.Message;
            }
        }

        // Selección en el grid

        private void DgvRoles_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRoles.SelectedRows.Count == 0) return;

            _selected = dgvRoles.SelectedRows[0].DataBoundItem as RoleDto;
            if (_selected == null) return;

            txtRoleName.Text = _selected.RoleName;
            txtDescription.Text = _selected.Descriptions ?? "";
            lblError.Visible = false;

            // Marcar permisos del rol en el CheckedListBox
            for (int i = 0; i < _allPermissions.Count; i++)
            {
                bool hasPermission = _selected.Permissions.Contains(
                    _allPermissions[i].PermissionKey);
                clbPermissions.SetItemChecked(i, hasPermission);
            }

            // Proteger roles del sistema
            bool isSystem = _selected.RoleId <= 2;
            btnDelete.Enabled = !isSystem;
        }

        // Obtener permisos seleccionados

        private List<string> GetCheckedPermissions()
        {
            var keys = new List<string>();
            for (int i = 0; i < clbPermissions.CheckedIndices.Count; i++)
            {
                int idx = (int)clbPermissions.CheckedIndices[i];
                if (idx < _allPermissions.Count)
                    keys.Add(_allPermissions[idx].PermissionKey);
            }
            return keys;
        }

        // Guardar

        private void btnSave_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;

            if (string.IsNullOrWhiteSpace(txtRoleName.Text))
            {
                lblError.Text = "El nombre del rol es obligatorio.";
                lblError.Visible = true;
                return;
            }

            var permissionKeys = GetCheckedPermissions();

            try
            {
                Cursor = Cursors.WaitCursor;

                if (_selected == null)
                {
                    AppServices.ApiClient.CreateRole(new CreateRoleRequest
                    {
                        RoleName = txtRoleName.Text.Trim(),
                        Descriptions = txtDescription.Text.Trim(),
                        PermissionKeys = permissionKeys
                    });
                    lblStatus.Text = "Rol creado correctamente.";
                }
                else
                {
                    AppServices.ApiClient.UpdateRole(new UpdateRoleRequest
                    {
                        RoleId = _selected.RoleId,
                        RoleName = txtRoleName.Text.Trim(),
                        Descriptions = txtDescription.Text.Trim(),
                        PermissionKeys = permissionKeys
                    });
                    lblStatus.Text = "Rol actualizado correctamente.";
                }

                Cursor = Cursors.Default;
                LoadRoles();
                ClearForm();
            }
            catch (BackOfficeApiException ex)
            {
                Cursor = Cursors.Default;
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }
        }

        // Eliminar

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selected == null) return;

            var confirm = MessageBox.Show(
                "¿Desea eliminar el rol \"" + _selected.RoleName + "\"?\n" +
                "Se eliminarán también todos sus permisos asignados.",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                AppServices.ApiClient.DeleteRole(_selected.RoleId);
                lblStatus.Text = "Rol eliminado correctamente.";
                LoadRoles();
                ClearForm();
            }
            catch (BackOfficeApiException ex)
            {
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }
        }

        private void btnNew_Click(object sender, EventArgs e) => ClearForm();

        private void ClearForm()
        {
            _selected = null;
            txtRoleName.Text = "";
            txtDescription.Text = "";
            lblError.Visible = false;
            btnDelete.Enabled = false;

            // Desmarcar todos los permisos
            for (int i = 0; i < clbPermissions.Items.Count; i++)
                clbPermissions.SetItemChecked(i, false);

            dgvRoles.ClearSelection();
        }
    }
}