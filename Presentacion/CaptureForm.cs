﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace Presentacion
{
	public partial class CaptureForm : Form, DPFP.Capture.EventHandler
	{
		delegate void Function(); // a simple delegate for marshalling calls from event handlers to the GUI thread
		public CaptureForm()
		{
			InitializeComponent();
		}

		protected virtual void Init()
		{
			try
			{
				Capturer = new DPFP.Capture.Capture();              // Create a capture operation.

				if (null != Capturer)
					Capturer.EventHandler = this;                   // Subscribe for capturing events.
				else
					SetPrompt("Can't initiate capture operation!");
			}
			catch
			{
				MessageBox.Show("Can't initiate capture operation!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		protected virtual void Process(DPFP.Sample Sample)
		{
			// Draw fingerprint sample image.
			DrawPicture(ConvertSampleToBitmap(Sample));
		}

		protected void Start()
		{
			if (null != Capturer)
			{
				try
				{
					Capturer.StartCapture();
					SetPrompt("Using the fingerprint reader, scan your fingerprint.");
				}
				catch
				{
					SetPrompt("Can't initiate capture!");
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
					SetPrompt("Can't terminate capture!");
				}
			}
		}

		#region Form Event Handlers:

		private void CaptureForm_Load(object sender, EventArgs e)
		{
			Init();
			Start();                                                // Start capture operation.
		}

		private void CaptureForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			Stop();
		}
		#endregion

		#region EventHandler Members:

		public void OnComplete(object Capture, string ReaderSerialNumber, DPFP.Sample Sample)
		{
			MakeReport("La muestra de la huella fué capturada.");
			SetPrompt("Escanea la misma huella otra vez.");
			Process(Sample);
		}

		public void OnFingerGone(object Capture, string ReaderSerialNumber)
		{
			MakeReport("El dedo fue removido del lector.");
		}

		public void OnFingerTouch(object Capture, string ReaderSerialNumber)
		{
			MakeReport("El lector ha sido tocado.");
		}

		public void OnReaderConnect(object Capture, string ReaderSerialNumber)
		{
			MakeReport("El sensor esta conectado.");
		}

		public void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
		{
			MakeReport("El sensor esta desconectado.");
		}

		public void OnSampleQuality(object Capture, string ReaderSerialNumber, DPFP.Capture.CaptureFeedback CaptureFeedback)
		{
			if (CaptureFeedback == DPFP.Capture.CaptureFeedback.Good)
				MakeReport("La calidad de la muestra de la huella es buena.");
			else
				MakeReport("La calidad de la huella no es buena.");
		}
		#endregion

		protected Bitmap ConvertSampleToBitmap(DPFP.Sample Sample)
		{
			DPFP.Capture.SampleConversion Convertor = new DPFP.Capture.SampleConversion();  // Create a sample convertor.
			Bitmap bitmap = null;                                                           // TODO: the size doesn't matter
			Convertor.ConvertToPicture(Sample, ref bitmap);                                 // TODO: return bitmap as a result
			return bitmap;
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

		protected void SetStatus(string status)
		{
			this.Invoke(new Function(delegate () {
				StatusLine.Text = status;
			}));
		}

		protected void SetPrompt(string prompt)
		{
			this.Invoke(new Function(delegate () {
				Prompt.Text = prompt;
			}));
		}
		protected void MakeReport(string message)
		{
			this.Invoke(new Function(delegate () {
				StatusText.AppendText(message + "\r\n");
			}));
		}

		private void DrawPicture(Bitmap bitmap)
		{
			this.Invoke(new Function(delegate () {
				Picture.Image = new Bitmap(bitmap, Picture.Size);   // fit the image into the picture box
			}));
		}

		private DPFP.Capture.Capture Capturer;
	}
}
