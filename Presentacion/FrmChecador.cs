using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DatosRH;
using DatosRH.DAO;
using DatosRH.DTO;
using DPFP;

namespace Presentacion
{
    public partial class FrmChecador : Form, DPFP.Capture.EventHandler
    {
        private DPFP.Capture.Capture Capturer;
        private DPFP.Verification.Verification Verificator;
        delegate void Function();
        

        public FrmChecador()
        {
            InitializeComponent();
        }

        protected virtual void Init()
        {
            try
            {
                Capturer = new DPFP.Capture.Capture();				// Create a capture operation.

                if (null != Capturer)
                    Capturer.EventHandler = this;					// Subscribe for capturing events.
                else
                    lblReport.Text = "No se puede iniciar la operación de captura";
            }
            catch
            {
                MessageBox.Show("Can't initiate capture operation!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            Verificator = new DPFP.Verification.Verification();		// Create a fingerprint template verificator
        }

        protected void Start()
        {
            if (null != Capturer)
            {
                try
                {
                    Capturer.StartCapture();
                    lblReport.Text= "Using the fingerprint reader, scan your fingerprint.";
                }
                catch
                {
                    lblReport.Text = "Can't initiate capture!";
                }
            }
        }

        protected void Stop()
        {
            if (null != Capturer)
            {
                try
                {
                    Capturer.StopCapture();
                }
                catch
                {
                    lblReport.Text = "Can't terminate capture!";
                }
            }
        }


        public void OnComplete(object Capture, string ReaderSerialNumber, DPFP.Sample Sample)
        {
            this.Invoke(new Function(delegate () {
                lblReport.Text = "La muestra de la huella ha sido capturada.";
                lblStatus.Text = "Coloca tu dedo para registrar entrada o salida.";
                Process(Sample);
            }));
            
        }

        public void OnFingerGone(object Capture, string ReaderSerialNumber)
        {
            this.Invoke(new Function(delegate () {
                lblReport.Text = "El dedo fué removido del sensor.";
            }));
            
        }

        public void OnFingerTouch(object Capture, string ReaderSerialNumber)
        {
            this.Invoke(new Function(delegate () {
                lblReport.Text = "El sensor ha sido tocado.";
            }));
            
        }
        public void OnReaderConnect(object Capture, string ReaderSerialNumber)
        {
            this.Invoke(new Function(delegate () {
                lblReport.Text = "El sensor esta conectado.";
            }));
        }

        public void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
        {
            this.Invoke(new Function(delegate () {
                lblReport.Text = "El sensor esta desconectado.";
            }));
        }

        public void OnSampleQuality(object Capture, string ReaderSerialNumber, DPFP.Capture.CaptureFeedback CaptureFeedback)
        {
            //if (CaptureFeedback == DPFP.Capture.CaptureFeedback.Good)
            //    MakeReport("The quality of the fingerprint sample is good.");
            //else
            //    MakeReport("The quality of the fingerprint sample is poor.");
        }

        protected DPFP.FeatureSet ExtractFeatures(DPFP.Sample Sample, DPFP.Processing.DataPurpose Purpose)
        {
            DPFP.Processing.FeatureExtraction Extractor = new DPFP.Processing.FeatureExtraction();  // Create a feature extractor
            DPFP.Capture.CaptureFeedback feedback = DPFP.Capture.CaptureFeedback.None;
            DPFP.FeatureSet features = new DPFP.FeatureSet();
            Extractor.CreateFeatureSet(Sample, Purpose, ref feedback, ref features);            // TODO: return features as a result?
            if (feedback == DPFP.Capture.CaptureFeedback.Good)
                return features;
            else
                return null;
        }

        protected void Process(DPFP.Sample Sample)
        {
            // Process the sample and create a feature set for the enrollment purpose.
            DPFP.FeatureSet features = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Verification);

            // Check quality of the sample and start verification if it's good
            // TODO: move to a separate task
            if (features != null)
            {
                // Compare the feature set with our template
                DPFP.Verification.Verification.Result result = new DPFP.Verification.Verification.Result();
                DPFP.Template template = new DPFP.Template();
                Stream stream;
                List<Personal> empleados = new List<Personal>();
                PersonalDAO dao = new PersonalDAO();
                ChecadasDAO chk = new ChecadasDAO();
                empleados = dao.GetAll();
                foreach(var empleado in empleados)
                {
                    if(empleado.Huella != null)
                    {
                        stream = new MemoryStream(empleado.Huella);
                        template = new DPFP.Template(stream);
                        Verificator.Verify(features, template, ref result);
                        if (result.Verified)
                        {
                            Checada checada = new Checada
                            {
                                FechaHora = DateTime.Now,
                                Empleado = empleado.Id,
                            };

                            this.Invoke(new Function(delegate () {
                                chk.Add(checada);
                                MostrarAccesoCorrecto(empleado);
                            }));
                            break;
                        }
                        else
                        {
                            this.Invoke(new Function(delegate () {
                                MostrarAccesoIncorrecto("Acceso Incorrecto");
                            }));
                            
                        }
                    }
                }
            }
        }

        private void MostrarAccesoCorrecto(Personal empleado)
        {
            lblMsg.ForeColor = Color.Green;
            lblMsg.Text = "Acceso Correcto";
            lblDatos.Text = empleado.Num + Environment.NewLine + empleado.Apellidos + Environment.NewLine + empleado.Nombre;
            lblMsg.Visible = true;
            lblDatos.Visible = true;
            timer2.Start();
        }

        private void MostrarAccesoIncorrecto(string msg)
        {
            lblMsg.ForeColor = Color.Red;
            lblMsg.Text = msg;
            lblMsg.Visible = true;
            timer2.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblReloj.Text = DateTime.Now.ToLongTimeString();
        }

        private void FrmChecador_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "Coloca tu dedo para registrar entrada o salida.";
            lblReloj.Text = DateTime.Now.ToLongTimeString();
            timer1.Start();
            Init();
            Start();  
        }

        private void FrmChecador_FormClosed(object sender, FormClosedEventArgs e)
        {
            Stop();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();
            lblDatos.Text = "";
            lblMsg.Text = "";
            lblMsg.Visible = false;
            lblDatos.Visible = false;
        }

        private void lblBack_Click(object sender, EventArgs e)
        {
            FrmMain main = new FrmMain();
            main.Show();
            this.Close();
        }
    }
}
