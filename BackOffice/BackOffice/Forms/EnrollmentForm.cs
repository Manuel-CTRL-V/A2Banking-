using Business.Biometric;
using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;


namespace BackOffice.Forms
{
    public partial class EnrollmentForm : Form
    {
        /// <summary>
        /// Template serializado — disponible cuando DialogResult == OK.
        /// El CreateAccountForm lo lee después de cerrar este formulario.
        /// </summary>
        public byte[] TemplateBytes { get; private set; }
        private int _totalCaptures = 4;

        private readonly EnrollmentService _service = new EnrollmentService();
        public EnrollmentForm()
        {
            InitializeComponent();

            // Suscribirse a eventos del servicio para actualizar la UI
            _service.OnStatusChanged += OnStatusChanged;
            _service.OnCompleted += OnEnrollmentCompleted;
            _service.OnError += OnEnrollmentError;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            btnAccept.Enabled = false;
            _service.Start();
        }
        private void OnStatusChanged(string message)
        {
            // El SDK corre en un hilo distinto — Invoke para actualizar UI
            if (lblStatus.InvokeRequired)
            {
                lblStatus.Invoke(new Action(() =>
                {
                    lblStatus.Text = message;
                    lstEvents.Items.Insert(0, DateTime.Now.ToString("HH:mm:ss") + " — " + message);
                }));
                lblStatus.Invoke(new Action(() =>
                {
                    UpdateUI(message);
                }));
            }
            else
            {
                lblStatus.Text = message;
                lstEvents.Items.Insert(0, DateTime.Now.ToString("HH:mm:ss") + " — " + message);
            }
        }

        private void OnEnrollmentCompleted(byte[] templateBytes)
        {
            TemplateBytes = templateBytes;

            if (btnAccept.InvokeRequired)
                btnAccept.Invoke(new Action(() =>
                {
                    btnAccept.Enabled = true;
                    lblStatus.Text = "Huella registrada correctamente.";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                }));
            else
            {
                btnAccept.Enabled = true;
                lblStatus.Text = "Huella registrada correctamente.";
                lblStatus.ForeColor = System.Drawing.Color.Green;
            }
        }

        private void OnEnrollmentError(string error)
        {
            if (lblStatus.InvokeRequired)
                lblStatus.Invoke(new Action(() =>
                {
                    lblStatus.Text = "Error: " + error;
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                }));
            else
            {
                lblStatus.Text = "Error: " + error;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }
        private void UpdateUI(string message)
        {
            lblStatus.Text = message;

            int remaining = ExtractRemaining(message);

            if (remaining >= 0)
            {
                int captured = _totalCaptures - remaining;

                UpdateProgress(captured);
                UpdateFingerprintImage(captured);
            }
        }
        private void UpdateFingerprintImage(int captured)
        {
            string basePath = @"C:\fingerprint\";

            string imagePath;

            if (captured >= _totalCaptures)
                imagePath = basePath + "fingerprint_4.png";
            else
                imagePath = basePath + $"fingerprint_{captured}.png";

            picFingerprint.Image = LoadImage(imagePath);
        }
        private Image LoadImage(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return Image.FromStream(stream);
            }
        }

        private void UpdateProgress(int captured)
        {
            int percent = (captured * 100) / _totalCaptures;

            progressBar.Value = Math.Max(0, Math.Min(100, percent));
        }
        private int ExtractRemaining(string message)
        {
            var match = System.Text.RegularExpressions.Regex.Match(message, @"\d+");

            if (match.Success)
                return int.Parse(match.Value);

            return -1;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _service.Stop();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _service.OnStatusChanged -= OnStatusChanged;
            _service.OnCompleted -= OnEnrollmentCompleted;
            _service.OnError -= OnEnrollmentError;
            _service.Stop();
            base.OnFormClosing(e);
        }
    }
}
