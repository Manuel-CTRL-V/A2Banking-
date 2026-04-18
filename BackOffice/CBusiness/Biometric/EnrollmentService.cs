using System;
using System.Threading;
using DPFP;
using DPFP.Capture;
using DPFP.Processing;
using DPFP.Verification;

namespace Business.Biometric
{
    /// <summary>
    /// Maneja el enrollment biométrico real con el DigitalPersona 4500.
    /// A diferencia del ATM que solo verifica, el back-office REGISTRA
    /// la huella por primera vez.
    ///
    /// El SDK hace varias capturas (generalmente 4) y las combina
    /// para generar un template robusto.
    ///
    /// Uso:
    ///   var svc = new EnrollmentService();
    ///   svc.OnStatusChanged += (msg) => lblStatus.Text = msg;
    ///   svc.OnCompleted     += (template) => { /* guardar template */ };
    ///   svc.Start();
    ///   // ... usuario apoya el dedo varias veces ...
    ///   // OnCompleted se dispara cuando el template está listo
    ///   svc.Stop();
    /// </summary>
    public class EnrollmentService
    {
        // Eventos para actualizar la UI sin bloquearla
        public event Action<string>  OnStatusChanged;
        public event Action<byte[]>  OnCompleted;
        public event Action<string>  OnError;
        public event Action<int, int> OnProgressChanged;

        public Capture    _capture;
        public Enrollment _enrollment;
        private bool       _isRunning;

        public bool IsRunning { get { return _isRunning; } }
        private int _totalFeatures;
        public void Start()
        {
            _enrollment = new Enrollment();
            _capture    = new Capture();
            _isRunning  = true;

            _totalFeatures = (int)_enrollment.FeaturesNeeded;
            _capture.EventHandler = new CaptureHandler(this);
            _capture.StartCapture();

            RaiseStatus("Listo. Apoya el dedo en el lector.");
        }

        public void Stop()
        {
            _isRunning = false;
            try
            {
                _capture?.StopCapture();
                _capture?.Dispose();
            }
            catch { }
        }

        internal void ProcessSample(Sample sample)
        {
            if (!_isRunning) return;

            try
            {
                // Extraer features del sample capturado
                var extractor  = new FeatureExtraction();
                var feedback   = CaptureFeedback.None;
                var featureSet = new FeatureSet();

                extractor.CreateFeatureSet(
                    sample, DataPurpose.Enrollment,
                    ref feedback, ref featureSet);

                if (feedback != CaptureFeedback.Good)
                {
                    RaiseStatus("Calidad insuficiente. Intenta de nuevo.");
                    return;
                }

                // Agregar al proceso de enrollment
                _enrollment.AddFeatures(featureSet);

                var remaining = _enrollment.FeaturesNeeded;

                if (remaining > 0)
                {
                    RaiseStatus("Bien. Faltan " + remaining + " captura(s) más.");

                }
                else
                {
                    // Enrollment completo — generar template
                    var template = _enrollment.Template;

                    // Serializar el template a bytes para enviarlo a la BD
                    var bytes = new byte[template.Bytes.Length];
                    template.Bytes.CopyTo(bytes, 0);

                    Stop();
                    RaiseStatus("Enrollment completado exitosamente.");
                    OnCompleted?.Invoke(bytes);
                }
            }
            catch (Exception ex)
            {
                RaiseStatus("Error: " + ex.Message);
                OnError?.Invoke(ex.Message);
            }
        }

        internal void HandleReaderDisconnect()
        {
            Stop();
            RaiseStatus("El lector fue desconectado.");
            OnError?.Invoke("El lector DigitalPersona fue desconectado.");
        }

        private void RaiseStatus(string message)
        {
            OnStatusChanged?.Invoke(message);
        }
    }

    /// <summary>
    /// Handler de eventos del SDK DigitalPersona.
    /// Recibe los callbacks del hardware y los delega al EnrollmentService.
    /// </summary>
    internal class CaptureHandler : DPFP.Capture.EventHandler
    {
        private readonly EnrollmentService _service;

        public CaptureHandler(EnrollmentService service)
        {
            _service = service;
        }

        public void OnComplete(object capture, string readerSerial, Sample sample)
        {
            _service.ProcessSample(sample);
        }

        public void OnFingerGone(object capture, string readerSerial) { }

        public void OnFingerTouch(object capture, string readerSerial) { }

        public void OnReaderConnect(object capture, string readerSerial) { }

        public void OnReaderDisconnect(object capture, string readerSerial)
        {
            _service.HandleReaderDisconnect();
        }

        public void OnSampleQuality(object capture, string readerSerial,
                                    CaptureFeedback feedback) { }
    }
}
