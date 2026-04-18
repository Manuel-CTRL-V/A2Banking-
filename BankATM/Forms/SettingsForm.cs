using ATM.Kiosk;
using ATM.Kiosk.Business.Context;
using ATM.Shared.DTOs.Notifications;
using System;
using System.Windows.Forms;

namespace BankATM.Forms
{
    public partial class SettingsForm : Form
    {
        private readonly TransactionContext _txContext;

        public SettingsForm(TransactionContext txContext)
        {
            InitializeComponent();
            _txContext = txContext;

            // Habilitar/deshabilitar campos al marcar checkboxes
            chkEmailNotifications.CheckedChanged += ChkEmail_CheckedChanged;
            chkSmsNotifications.CheckedChanged += ChkSms_CheckedChanged;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadSettings();
        }

        // Cargar configuración guardada

        private void LoadSettings()
        {
            try
            {
                var session = ATM.Kiosk.Business.Auth.SessionManager
                    .Instance.Current;
                if (session == null) return;

                var settings = AppServices.ApiClient
                    .GetNotificationSettings(session.AccountId);

                chkEmailNotifications.Checked = settings.EmailEnabled;
                chkSmsNotifications.Checked = settings.SmsEnabled;
                txtEmail.Text = settings.EmailAddress ?? "";
                txtPhone.Text = settings.PhoneNumber ?? "";

                UpdateFieldStates();
            }
            catch
            {
                // Si falla la carga, el form queda con defaults
            }
        }

        // Habilitar/deshabilitar campos

        private void ChkEmail_CheckedChanged(object sender, EventArgs e)
            => UpdateFieldStates();

        private void ChkSms_CheckedChanged(object sender, EventArgs e)
            => UpdateFieldStates();

        private void UpdateFieldStates()
        {
            txtEmail.Enabled = chkEmailNotifications.Checked;
            txtPhone.Enabled = chkSmsNotifications.Checked;
        }

        // Guardar

        private void btnSave_Click(object sender, EventArgs e)
        {
            lblMessage.Visible = false;

            // Validar email si está habilitado
            if (chkEmailNotifications.Checked &&
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                lblMessage.Text = "Ingrese una dirección de correo válida.";
                lblMessage.Visible = true;
                return;
            }

            // Validar teléfono si SMS está habilitado
            if (chkSmsNotifications.Checked &&
                string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                lblMessage.Text = "Ingrese un número de teléfono válido.";
                lblMessage.Visible = true;
                return;
            }

            try
            {
                var session = ATM.Kiosk.Business.Auth.SessionManager
                    .Instance.Current;

                var request = new NotificationSettingsRequest
                {
                    AccountId = session?.AccountId ?? 0,
                    EmailEnabled = chkEmailNotifications.Checked,
                    EmailAddress = chkEmailNotifications.Checked
                        ? txtEmail.Text.Trim()
                        : null,
                    SmsEnabled = chkSmsNotifications.Checked,
                    PhoneNumber = chkSmsNotifications.Checked
                        ? txtPhone.Text.Trim()
                        : null
                };

                AppServices.ApiClient.SaveNotificationSettings(request);

                MessageBox.Show(
                    "Configuración guardada correctamente.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error al guardar: " + ex.Message;
                lblMessage.Visible = true;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
