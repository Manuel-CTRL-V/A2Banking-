using ATM.Shared.DTOs.BackOffice;
using BackOffice.Helpers;
using BackOffice.Services.Implementations;
using Business.Auth;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;

namespace BackOffice.Forms
{
    public partial class CreateAccountForm : Form
    {
        private List<SimpsonsCharacterDto> _characters;
        private SimpsonsCharacterDto _selected;
        private byte[] _fingerprintTemplate;
        private int _createdAccountId;

        private static readonly HttpClient _imageClient = new HttpClient();

        private const int StudentMaxAge = 25;

        public CreateAccountForm()
        {
            InitializeComponent();
            ApplyTheme();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            cmbAccountType.SelectedIndex = 0;
            btnEnroll.Enabled = false;
            btnCreate.Enabled = false;
            lblEnrollStatus.Text = "Pendiente";
        }

        private void ApplyTheme()
        {
            bool isDark = BackOfficeColors.CurrentTheme == ThemeType.Dark;

            this.BackColor = BackOfficeColors.Background;

            kryptonGroup1.StateCommon.Back.Color1 = isDark ? Color.FromArgb(45, 45, 45) : Color.White;
            kryptonGroup1.StateCommon.Border.Color1 = isDark ? Color.FromArgb(60, 60, 60) : Color.White;

            kryptonGroup2.StateCommon.Back.Color1 = isDark ? Color.FromArgb(45, 45, 45) : Color.White;
            kryptonGroup2.StateCommon.Border.Color1 = isDark ? Color.FromArgb(60, 60, 60) : Color.White;

            kryptonGroup3.StateCommon.Back.Color1 = isDark ? Color.FromArgb(45, 45, 45) : Color.White;
            kryptonGroup3.StateCommon.Border.Color1 = isDark ? Color.FromArgb(60, 60, 60) : Color.White;

            txtSearch.StateCommon.Back.Color1 = isDark ? Color.FromArgb(50, 50, 50) : Color.FromArgb(220, 229, 218);
            txtSearch.StateCommon.Border.Color1 = isDark ? Color.FromArgb(60, 60, 60) : Color.FromArgb(241, 244, 242);
            txtSearch.StateCommon.Content.Color1 = isDark ? Color.FromArgb(180, 180, 180) : Color.FromArgb(112, 121, 112);

            txtPin.StateCommon.Back.Color1 = isDark ? Color.FromArgb(50, 50, 50) : Color.FromArgb(220, 229, 218);
            txtPin.StateCommon.Border.Color1 = isDark ? Color.FromArgb(60, 60, 60) : Color.FromArgb(241, 244, 242);
            txtPin.StateCommon.Content.Color1 = isDark ? Color.FromArgb(180, 180, 180) : Color.FromArgb(112, 121, 112);

            txtPinConfirm.StateCommon.Back.Color1 = isDark ? Color.FromArgb(50, 50, 50) : Color.FromArgb(220, 229, 218);
            txtPinConfirm.StateCommon.Border.Color1 = isDark ? Color.FromArgb(60, 60, 60) : Color.FromArgb(241, 244, 242);
            txtPinConfirm.StateCommon.Content.Color1 = isDark ? Color.FromArgb(180, 180, 180) : Color.FromArgb(112, 121, 112);

            txtBalance.StateCommon.Back.Color1 = isDark ? Color.FromArgb(50, 50, 50) : Color.FromArgb(220, 229, 218);
            txtBalance.StateCommon.Border.Color1 = isDark ? Color.FromArgb(60, 60, 60) : Color.FromArgb(241, 244, 242);
            txtBalance.StateCommon.Content.Color1 = isDark ? Color.FromArgb(180, 180, 180) : Color.FromArgb(112, 121, 112);

            lblStep1.ForeColor = isDark ? Color.FromArgb(0, 200, 83) : Color.FromArgb(41, 79, 45);
            lblStep3.ForeColor = isDark ? Color.FromArgb(0, 200, 83) : Color.FromArgb(41, 79, 45);
            label1.ForeColor = isDark ? Color.FromArgb(0, 200, 83) : Color.FromArgb(41, 79, 45);
            label2.ForeColor = isDark ? Color.FromArgb(150, 150, 150) : SystemColors.ButtonShadow;
            label3.ForeColor = isDark ? Color.FromArgb(180, 180, 180) : Color.Black;
            label4.ForeColor = isDark ? Color.White : Color.Black;
            lblBalance.ForeColor = isDark ? Color.White : Color.Black;
            label5.ForeColor = isDark ? Color.FromArgb(150, 150, 150) : Color.Black;
            label6.ForeColor = isDark ? Color.FromArgb(150, 150, 150) : Color.Black;
            label8.ForeColor = isDark ? Color.FromArgb(0, 200, 83) : Color.FromArgb(41, 79, 45);
            lblSelectedChar.ForeColor = isDark ? Color.White : Color.Black;
            lblCharacterAge.ForeColor = isDark ? Color.FromArgb(150, 150, 150) : SystemColors.ButtonShadow;
            lblEnrollStatus.ForeColor = isDark ? Color.FromArgb(150, 150, 150) : Color.Black;
            lblError.ForeColor = isDark ? Color.FromArgb(255, 82, 82) : Color.Red;

            btnSearch.BackColor = isDark ? Color.FromArgb(50, 50, 50) : Color.White;
            btnSearch.ForeColor = isDark ? Color.White : Color.Black;
            btnSearch.IconColor = isDark ? Color.White : Color.Black;

            btnEnroll.BackColor = isDark ? Color.FromArgb(50, 50, 50) : Color.White;
            btnEnroll.ForeColor = isDark ? Color.FromArgb(0, 200, 83) : Color.FromArgb(41, 79, 45);
            btnEnroll.IconColor = isDark ? Color.FromArgb(0, 200, 83) : Color.FromArgb(114, 155, 114);

            btnCreate.BackColor = isDark ? Color.FromArgb(0, 200, 83) : Color.White;
            btnCreate.ForeColor = isDark ? Color.FromArgb(20, 20, 20) : Color.FromArgb(57, 95, 59);
            btnCreate.IconColor = isDark ? Color.FromArgb(20, 20, 20) : Color.FromArgb(57, 95, 59);

            btnCancel.BackColor = isDark ? Color.FromArgb(50, 50, 50) : Color.White;
            btnCancel.ForeColor = isDark ? Color.FromArgb(255, 82, 82) : Color.FromArgb(162, 35, 0);
            btnCancel.IconColor = isDark ? Color.FromArgb(255, 82, 82) : Color.FromArgb(162, 35, 0);

            cmbAccountType.BackColor = isDark ? Color.FromArgb(50, 50, 50) : Color.FromArgb(220, 229, 218);
            cmbAccountType.ForeColor = isDark ? Color.White : Color.FromArgb(57, 95, 59);

            dataGridView1.BackgroundColor = isDark ? Color.FromArgb(30, 30, 30) : Color.FromArgb(240, 241, 236);
            dataGridView1.DefaultCellStyle.BackColor = isDark ? Color.FromArgb(40, 40, 40) : Color.FromArgb(192, 201, 191);
            dataGridView1.DefaultCellStyle.ForeColor = isDark ? Color.White : Color.FromArgb(41, 79, 45);
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = isDark ? Color.FromArgb(45, 45, 45) : Color.FromArgb(89, 128, 90);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = isDark ? Color.White : Color.FromArgb(234, 243, 232);
            dataGridView1.GridColor = isDark ? Color.FromArgb(60, 60, 60) : Color.FromArgb(0, 33, 7);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;
            dataGridView1.DataSource = null;
            _selected = null;

            HideCharacterInfo();

            try
            {
                Cursor = Cursors.WaitCursor;
                _characters = AppServices.ApiClient
                                  .SearchCharacters(txtSearch.Text.Trim());
                Cursor = Cursors.Default;

                if (_characters.Count == 0)
                {
                    lblError.Text = "No se encontraron personajes con ese nombre.";
                    lblError.Visible = true;
                    return;
                }

                dataGridView1.DataSource = _characters.Select(c => new { c.CharacterId, c.Name, c.PortraitUrl, c.Age }).ToList();
                dataGridView1.Columns["PortraitUrl"].Visible = false;
            }
            catch (BackOfficeApiException ex)
            {
                Cursor = Cursors.Default;
                lblError.Text = ex.IsConnectionError
                    ? "Sin conexión con el servidor: " + ex.Message
                    : ex.Message;
                lblError.Visible = true;
            }
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            int filaSeleccionada = dataGridView1.CurrentCell?.RowIndex ?? -1;

            if (filaSeleccionada < 0 || _characters == null || filaSeleccionada >= _characters.Count)
            {
                _selected = null;
                lblSelectedChar.Text = "Seleccionado: Ninguno";
                btnEnroll.Enabled = false;
                btnCreate.Enabled = false;
                return;
            }
            else
            {
                _selected = _characters[filaSeleccionada];
                lblSelectedChar.Text = "Seleccionado: " + _selected.Name;
                btnEnroll.Enabled = true;
                btnCreate.Enabled = false;
                _fingerprintTemplate = null;
                lblEnrollStatus.Text = "Pendiente";
                LoadCharacterPortrait(_selected.PortraitUrl);
                lblCharacterAge.Text = "Edad: " + _selected.Age + " años";
                lblCharacterAge.Visible = true;

                // Validar restricción de estudiante si ya está seleccionado ese tipo
                ValidateStudentAge();
            }

        }
        private async void LoadCharacterPortrait(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                var bytes = await client.GetByteArrayAsync(url);

                using (var image = new MagickImage(bytes))
                {
                    using (var ms = new MemoryStream())
                    {
                        image.Format = MagickFormat.Png; // Convertimos a PNG
                        image.Write(ms);
                        ms.Position = 0;

                        picCharacter.Image = new Bitmap(ms);
                    }
                }
            }
        }

        private void HideCharacterInfo()
        {
            picCharacter.Image = null;
            //picCharacter.Visible = false;
            lblCharacterAge.Visible = false;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            //int idx = lstCharacters.SelectedIndex;
            //if (idx < 0 || _characters == null || idx >= _characters.Count) return;

            //_selected = _characters[idx];
            //lblSelectedChar.Text = "Seleccionado: " + _selected.Name;
            //btnEnroll.Enabled = true;
            //btnCreate.Enabled = false;
            //_fingerprintTemplate = null;
            //lblEnrollStatus.Text = "Pendiente";
        }

        private void btnEnroll_Click(object sender, EventArgs e)
        {
            if (_selected == null)
            {
                lblError.Text = "Seleccione un personaje primero.";
                lblError.Visible = true;
                return;
            }

            using (var enrollForm = new EnrollmentForm())
            {
                var result = enrollForm.ShowDialog(this);
                if (result == DialogResult.OK && enrollForm.TemplateBytes != null)
                {
                    _fingerprintTemplate = enrollForm.TemplateBytes;
                    lblEnrollStatus.Text = "Completado";
                    lblEnrollStatus.ForeColor = System.Drawing.Color.Green;

                    // Habilitar creación solo si el PIN también está ok
                    ValidateCanCreate();
                }
            }
        }

        private void txtPin_TextChanged(object sender, EventArgs e) => ValidateCanCreate();

        private void txtPinConfirm_TextChanged(object sender, EventArgs e) => ValidateCanCreate();
        private void ValidateCanCreate()
        {
            btnCreate.Enabled = _selected != null
                             && _fingerprintTemplate != null
                             && txtPin.Text.Length >= 4
                             && txtPin.Text == txtPinConfirm.Text;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {

            lblError.Visible = false;

            if (txtPin.Text != txtPinConfirm.Text)
            {
                lblError.Text = "Los PINs no coinciden.";
                lblError.Visible = true;
                return;
            }

            if (cmbAccountType.SelectedIndex == 1 && Convert.ToDecimal(txtBalance.Text) < 5000m )
            {
                lblError.Text = "Una cuenta de ahorro no puede crearse con menos de 5.000 RD$.";
                lblError.Visible = true;
                return;
            }
            if (cmbAccountType.SelectedIndex == 3 && _selected.Age >= StudentMaxAge)
            {
                lblError.Text = "No se puede crear cuenta Estudiante: " + _selected.Name + " tiene " + _selected.Age + " años.";
                lblError.Visible = true;
                return;
            }

            var admin = AdminSessionManager.Instance.CurrentAdmin;

            try
            {
                Cursor = Cursors.WaitCursor;

                // Crear cuenta en Pending
                var createReq = new CreateAccountRequest
                {
                    ApiCharacterId = _selected.CharacterId,
                    AccountTypeId = cmbAccountType.SelectedIndex + 1,
                    FullName = _selected.Name,
                    Balance = Convert.ToDecimal(txtBalance.Text)
                   
                };

                var createResp = AppServices.ApiClient
                    .CreateAccount(createReq, admin.AdminId);

                _createdAccountId = createResp.AccountId;

                // Hashear PIN y hacer enrollment
                var salt = GenerateSalt();
                var pinHash = HashPin(txtPin.Text, salt);

                var enrollReq = new EnrollBiometricRequest
                {
                    AccountId = _createdAccountId,
                    PIN_Hash = pinHash,
                    PIN_Salt = salt,
                    FingerprintTemplate = _fingerprintTemplate
                };

                AppServices.ApiClient.EnrollBiometric(enrollReq, admin.AdminId);

                Cursor = Cursors.Default;

                MessageBox.Show(
                    "Cuenta creada exitosamente.\n" +
                    "Titular: " + _selected.Name + "\n" +
                    "AccountId: " + _createdAccountId,
                    "Cuenta creada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (BackOfficeApiException ex)
            {
                Cursor = Cursors.Default;
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }
            catch (Exception ex)
            {
                throw ex;
                Cursor = Cursors.Default;
                lblError.Text = "Error inesperado: " + ex.Message;
                lblError.Visible = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private static byte[] GenerateSalt()
        {
            var salt = new byte[32];
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
                rng.GetBytes(salt);
            return salt;
        }

        private static byte[] HashPin(string pin, byte[] salt)
        {
            var pinBytes = System.Text.Encoding.UTF8.GetBytes(pin);
            var combined = new byte[pinBytes.Length + salt.Length];
            Buffer.BlockCopy(pinBytes, 0, combined, 0, pinBytes.Length);
            Buffer.BlockCopy(salt, 0, combined, pinBytes.Length, salt.Length);
            using (var sha = System.Security.Cryptography.SHA256.Create())
                return sha.ComputeHash(combined);
        }

        private void cmbAccountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateStudentAge();
            if (cmbAccountType.SelectedIndex == 1) // Cuenta de ahorros
            {
                lblBalance.Visible = true;
                txtBalance.Visible = true;
                if (string.IsNullOrEmpty(txtBalance.Text) || txtBalance.Text == "0.00")
                {
                    txtBalance.Text = "5000.00";
                }
            }
            else
            {
                lblBalance.Visible = false;
                txtBalance.Visible = false;
            }
        }
        private void ValidateStudentAge()
        {
            lblError.Visible = false;

            // AccountTypeId 4 = Estudiante (índice 3 del combo)
            bool isStudent = cmbAccountType.SelectedIndex == 3;

            if (isStudent && _selected != null && _selected.Age >= StudentMaxAge)
            {
                lblError.Text = _selected.Name + " tiene " + _selected.Age +
                                   " años. La cuenta Estudiante requiere " +
                                   "ser menor de " + StudentMaxAge + " años.";
                lblError.Visible = true;
                cmbAccountType.SelectedIndex = 0;  // revertir a Corriente
            }
        }
    }
}
