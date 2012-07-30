/*!
 * Steve's Encode64 Utility
 * Copyright © 2012 Steven Benner.
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
		public FormMain()
		{
			InitializeComponent();
		}

		private void buttonBrowse_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() != DialogResult.Cancel)
			{
				textFileName.Text = openFileDialog1.FileName;

				StringBuilder sb = new StringBuilder();
				sb.Append("data:");
				sb.Append(GetMimeType(openFileDialog1.FileName));
				sb.Append(";base64,");
				sb.Append(Base64Encode(openFileDialog1.FileName));

				textEncodedString.Text = sb.ToString();
			}
		}

		private string Base64Encode(string fileToEncode)
		{
			byte[] EncodeBuffer;

			using (FileStream fs = new FileStream(fileToEncode, FileMode.Open))
			{
				int Length = Convert.ToInt32(fs.Length);
				EncodeBuffer = new byte[Length];

				fs.Read(EncodeBuffer, 0, Length);
			}

			return Convert.ToBase64String(EncodeBuffer);
		}

		private string GetMimeType(string fileName)
		{
			string mimeType = "application/unknown";
			string ext = Path.GetExtension(fileName).ToLower();

			Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);

			if (regKey != null && regKey.GetValue("Content Type") != null)
			{
				mimeType = regKey.GetValue("Content Type").ToString();
			}

			return mimeType;
		}

		private void buttonCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(textEncodedString.Text, TextDataFormat.Text);
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://stevenbenner.com/?utm_source=apps&utm_medium=encode64&utm_campaign=Apps%3A%2BEncode64%20Tool");
		}
	}
}
