using System;
using System.Text.RegularExpressions;
using System.Threading;
using ATM.Kiosk.Business.Exceptions;
using ATM.Kiosk.Services.Implementations;
using DPFP.Capture;
using DPFP;

namespace ATM.Kiosk.Business.Biometric
{
    /// <summary>
    /// Implementación REAL del servicio biométrico usando el SDK
    /// del DigitalPersona 4500.
    ///
    /// ESTADO ACTUAL: El cuerpo de los métodos está comentado
    /// porque el assembly DPFP no está referenciado todavía.
    /// La estructura y el flujo están completos — solo falta
    /// descomentar cuando el dispositivo esté disponible y
    /// se agregue la referencia al SDK (DPFP.dll).
    ///
    /// Flujo de verificación con el SDK real:
    ///   1. Crear Capture con DPFP.Capture.Capture()
    ///   2. Suscribirse a OnComplete y OnReaderDisconnect
    ///   3. capture.StartCapture() — bloqueante via ManualResetEvent
    ///   4. En OnComplete: extraer FeatureSet del Sample
    ///   5. Verification.Verify(featureSet, template, ref result)
    ///   6. capture.StopCapture() y devolver result.Verified
    /// </summary>
    public class BiometricService : IBiometricService
    {
        private readonly LocalLogger _logger = LocalLogger.Instance;
        private readonly TimeSpan    _captureTimeout = TimeSpan.FromSeconds(60);

        public bool IsDeviceAvailable
        {
            get
            {
                //var capture = new DPFP.Capture.Capture();
                // return capture.EventHandler.Is
                _logger.LogWarning("BiometricService.IsDeviceAvailable: SDK no referenciado aún.");
                return false;
            }
        }

        public bool VerifyFingerprint(byte[] storedTemplate)
        {
            if (storedTemplate == null || storedTemplate.Length == 0)
                throw new BusinessException(
                    BusinessErrorCode.BiometricFailed,
                    "No hay template biométrico registrado para esta cuenta.");

            // ── Implementación real con SDK ────────────────────────────
            // Descomentar cuando DPFP.dll esté referenciado:
            //
            var ready = new ManualResetEvent(false);
            bool verified = false;
            string errorMsg = null;

            // Deserializar el template almacenado
            var template = new DPFP.Template();
            template.DeSerialize(storedTemplate);

            var capture = new DPFP.Capture.Capture();

            capture.EventHandler = new CaptureHandler(
                onComplete: (sender, sample) =>
                {
                    try
                    {
                        // Extraer features del sample capturado
                        var extractor = new DPFP.Processing.FeatureExtraction();
                        var feedback = DPFP.Capture.CaptureFeedback.None;
                        var featureSet = new DPFP.FeatureSet();
                        extractor.CreateFeatureSet(sample, DPFP.Processing.DataPurpose.Verification,
                                                   ref feedback, ref featureSet);

                        if (feedback != DPFP.Capture.CaptureFeedback.Good)
                        {
                            errorMsg = "Calidad de muestra insuficiente.";
                            ready.Set();
                            return;
                        }

                        // Comparar con el template almacenado
                        var verifier = new DPFP.Verification.Verification();
                        var result = new DPFP.Verification.Verification.Result();
                        verifier.Verify(featureSet, template, ref result);
                        verified = result.Verified;
                    }
                    catch (Exception ex)
                    {
                        errorMsg = ex.Message;
                    }
                    finally
                    {
                        ready.Set();
                    }
                },
                onReaderDisconnect: (sender, readerSerial) =>
                {
                    errorMsg = "El lector fue desconectado.";
                    ready.Set();
                }
            );

            capture.StartCapture();

            bool capturedInTime = ready.WaitOne(_captureTimeout);
            capture.StopCapture();

            if (!capturedInTime)
                throw new BusinessException(
                    BusinessErrorCode.BiometricFailed,
                    "Tiempo de espera agotado. Intente de nuevo.");

            if (errorMsg != null)
                throw new BusinessException(
                    BusinessErrorCode.BiometricDeviceError, errorMsg);

            _logger.LogInfo("Verificación biométrica completada. Resultado: " + verified);
            return verified;

            _logger.LogWarning(
                "BiometricService.VerifyFingerprint: SDK no referenciado. " +
                "Usar BiometricServiceStub durante el desarrollo.");

            throw new BusinessException(
                BusinessErrorCode.BiometricDeviceError,
                "El SDK del dispositivo biométrico no está disponible.");
        }
    }

    public class CaptureHandler : DPFP.Capture.EventHandler
    {

        private readonly Action<object, Sample> _onComplete;
        private readonly Action<object, string> _onReaderDisconnect;

        public CaptureHandler(
            Action<object, Sample> onComplete,
            Action<object, string> onReaderDisconnect)
        {
            _onComplete = onComplete;
            _onReaderDisconnect = onReaderDisconnect;
        }

        public void OnComplete(object capture, string readerSerial, Sample sample)
        {
            _onComplete?.Invoke(capture, sample);
        }

        public void OnReaderDisconnect(object capture, string readerSerial)
        {
            _onReaderDisconnect?.Invoke(capture, readerSerial);
        }

        public void OnFingerGone(object capture, string readerSerial) { }

        public void OnFingerTouch(object capture, string readerSerial) { }

        public void OnReaderConnect(object capture, string readerSerial) { }
        public void OnSampleQuality(object capture, string readerSerial, CaptureFeedback feedback) { }
    }

}
