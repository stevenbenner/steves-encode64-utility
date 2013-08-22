/*!
 * Steve's Encode64 Utility
 * Copyright © 2013 Steven Benner.
 * https://github.com/stevenbenner/steves-encode64-utility
 * 
 * A simple tool to base64 encode files and grab the
 * encoded string. Useful for data URI creation.
 */

using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Steves_Encode64_Utility
{
	public partial class FormMain : Form
	{
		private const int maxFileSize = 10485760;

		public FormMain()
		{
			InitializeComponent();
		}

		private void ProcessFile(string filePath)
		{
			if (!CheckFileSize(filePath)) {
				return;
			}

			textFileName.Text = filePath;

			StringBuilder sb = new StringBuilder();
			sb.Append("data:");
			sb.Append(GetMimeType(filePath));
			sb.Append(";base64,");
			sb.Append(Base64Encode(filePath));

			textEncodedString.Text = sb.ToString();
		}

		private string Base64Encode(string filePath)
		{
			byte[] buffer;

			using (FileStream fs = new FileStream(filePath, FileMode.Open))
			{
				int length = Convert.ToInt32(fs.Length);
				buffer = new byte[length];

				fs.Read(buffer, 0, length);
			}

			return Convert.ToBase64String(buffer);
		}

		private string GetMimeType(string filePath)
		{
			string mimeType = "application/unknown";
			string ext = Path.GetExtension(filePath).ToLower();

			Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);

			if (regKey != null && regKey.GetValue("Content Type") != null)
			{
				mimeType = regKey.GetValue("Content Type").ToString();
			}

			return mimeType;
		}

		private bool CheckFileSize(string filePath)
		{
			FileInfo fi = new FileInfo(filePath);

			if (fi.Length > maxFileSize)
			{
				this.Activate();
				DialogResult dr = MessageBox.Show(
					this,
					"The file you are attempting to process is quite large, are you sure you want to do this?",
					"Steve's Encode64 Utility",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Warning
				);

				if (dr == DialogResult.No)
				{
					return false;
				}
			}

			return true;
		}

		#region Event Handlers

		private void buttonBrowse_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() != DialogResult.Cancel)
			{
				ProcessFile(openFileDialog1.FileName);
			}
		}

		private void buttonCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(textEncodedString.Text, TextDataFormat.Text);
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://stevenbenner.com/?utm_source=apps&utm_medium=encode64&utm_campaign=Apps%3A%2BEncode64%20Tool");
		}

		private void FormMain_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Link;
		}

		private void FormMain_DragDrop(object sender, DragEventArgs e)
		{
			var files = (string[]) e.Data.GetData(DataFormats.FileDrop);
			if (files.Length != 1)
			{
				this.Activate();
				MessageBox.Show(
					this,
					"Sorry, one file at a time please!",
					"Steve's Encode64 Utility",
					MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation
				);
				return;
			}

			ProcessFile(files[0]);
		}

		#endregion
	}
}
