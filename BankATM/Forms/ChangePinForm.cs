using ATM.Kiosk;
using ATM.Kiosk.Business.Exceptions;
using ATM.Kiosk.Business.Strategies;
using ATM.Kiosk.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankATM.Forms
{
    public partial class ChangePinForm : Form
    {
        private readonly ChangePinStrategy _strategy;

        // Almacenamiento interno de los tres campos
        private string _currentPin = "";
        private string _newPin = "";
        private string _confirmPin = "";

        // Fase activa: 0=actual, 1=nuevo, 2=confirmar
        private int _phase = 0;

        public ChangePinForm()
        {
            InitializeComponent();
            _strategy = new ChangePinStrategy(AppServices.ApiClient);

            // Conectar teclado numérico
            btn0.Click += NumPad_Click; btn1.Click += NumPad_Click;
            btn2.Click += NumPad_Click; btn3.Click += NumPad_Click;
            btn4.Click += NumPad_Click; btn5.Click += NumPad_Click;
            btn6.Click += NumPad_Click; btn7.Click += NumPad_Click;
            btn8.Click += NumPad_Click; btn9.Click += NumPad_Click;

            btnDelete.Click += BtnDelete_Click;
            btnConfirm.Click += BtnConfirm_Click;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UIHelper.StyleDangerButton(btnConfirm);
            UpdatePhaseUI();
            UpdateDots();
        }

        // ── Teclado numérico ──────────────────────────────────────────

        private void NumPad_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var digit = btn?.Text;

            if (string.IsNullOrEmpty(digit))
                return;

            switch (_phase)
            {
                case 0:
                    if (_currentPin.Length < 6)
                    {
                        _currentPin += digit;
                        UpdateDots();
                    }
                    break;

                case 1:
                    if (_newPin.Length < 6)
                    {
                        _newPin += digit;
                        UpdateDots();
                    }
                    break;

                case 2:
                    if (_confirmPin.Length < 6)
                    {
                        _confirmPin += digit;
                        UpdateDots();
                    }
                    break;
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            switch (_phase)
            {
                case 0:
                    if (_currentPin.Length > 0)
                    {
                        _currentPin = _currentPin.Substring(0, _currentPin.Length - 1);
                        UpdateDots();
                    }
                    break;

                case 1:
                    if (_newPin.Length > 0)
                    {
                        _newPin = _newPin.Substring(0, _newPin.Length - 1);
                        UpdateDots();
                    }
                    break;

                case 2:
                    if (_confirmPin.Length > 0)
                    {
                        _confirmPin = _confirmPin.Substring(0, _confirmPin.Length - 1);
                        UpdateDots();
                    }
                    break;
            }
        }

        // ── Avanzar fase con el botón Confirmar ───────────────────────

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;

            if (_phase == 0)
            {
                if (_currentPin.Length < 4)
                {
                    lblError.Text = "El PIN actual debe tener al menos 4 dígitos.";
                    lblError.Visible = true;
                    return;
                }

                _phase = 1;
                UpdatePhaseUI();
                UpdateDots();
                return;
            }

            if (_phase == 1)
            {
                if (_newPin.Length < 4)
                {
                    lblError.Text = "El nuevo PIN debe tener al menos 4 dígitos.";
                    lblError.Visible = true;
                    return;
                }

                _phase = 2;
                UpdatePhaseUI();
                UpdateDots();
                return;
            }

            // Fase 2: confirmar y enviar
            if (_confirmPin.Length < 4)
            {
                lblError.Text = "Confirme el nuevo PIN.";
                lblError.Visible = true;
                return;
            }

            if (_newPin != _confirmPin)
            {
                lblError.Text = "Los PINs nuevos no coinciden. Intente de nuevo.";
                lblError.Visible = true;

                // Reiniciar solo los dos últimos campos
                _newPin = "";
                _confirmPin = "";
                _phase = 1;

                UpdatePhaseUI();
                UpdateDots();
                return;
            }

            ExecuteChangePin();
        }

        private void ExecuteChangePin()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                _strategy.Execute(_currentPin, _newPin);
                Cursor = Cursors.Default;

                MessageBox.Show(
                    "Su PIN fue cambiado exitosamente.",
                    "PIN actualizado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (BusinessException ex)
            {
                Cursor = Cursors.Default;
                lblError.Text = ex.Message;
                lblError.Visible = true;

                // Si el PIN actual es incorrecto, volver al inicio
                if (ex.Code == BusinessErrorCode.WrongPin)
                {
                    _currentPin = "";
                    _newPin = "";
                    _confirmPin = "";
                    _phase = 0;

                    UpdatePhaseUI();
                    UpdateDots();
                }
            }
            catch (ApiException ex) when (ex.IsWrongPIN)
            {
                Cursor = Cursors.Default;
                lblError.Text = "PIN actual incorrecto. Intente de nuevo.";
                lblError.Visible = true;

                _currentPin = "";
                _newPin = "";
                _confirmPin = "";
                _phase = 0;

                UpdatePhaseUI();
                UpdateDots();
            }
            catch (ApiException ex)
            {
                Cursor = Cursors.Default;
                lblError.Text = ex.IsConnectionError
                    ? "Sin conexión con el servidor."
                    : ex.Message;
                lblError.Visible = true;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                lblError.Text = "Error inesperado: " + ex.Message;
                lblError.Visible = true;
            }
        }

        // UI helpers
        private void UpdatePhaseUI()
        {
            switch (_phase)
            {
                case 0:
                    lblInstruction.Text = "I n g r e s e   s u   P I N   A C T U A L";
                    break;

                case 1:
                    lblInstruction.Text = "I n g r e s e   e l   N U E V O   P I N";
                    break;

                case 2:
                    lblInstruction.Text = "C O N F I R M E   e l   n u e v o   P I N";
                    break;
            }
        }

        private void UpdateDots()
        {
            string pin = "";

            switch (_phase)
            {
                case 0:
                    pin = _currentPin;
                    break;
                case 1:
                    pin = _newPin;
                    break;
                case 2:
                    pin = _confirmPin;
                    break;
            }

            lblPinNumbers.Text = string.Join(" ", new string('•', pin.Length).ToCharArray());
        }

        private void iconButton1_Click(object sender, EventArgs e) => this.Close();

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
