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
using Microsoft.Win32;

namespace Steves_Encode64_Utility
{
	public partial class FormMain : Form
	{
		private const int maxFileSize = 10485760;

		public FormMain()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Loads a file and displays the base 64 encoded value.
		/// </summary>
		/// <param name="filePath">The path to the file to load.</param>
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

		/// <summary>
		/// Returns a string with the base 64 encoded content of a file.
		/// </summary>
		/// <param name="filePath">The path to the file to load.</param>
		/// <returns>A string with the base 64 encoded file contents.</returns>
		private string Base64Encode(string filePath)
		{
			byte[] buffer;

			using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
			{
				int length = Convert.ToInt32(fs.Length);
				buffer = new byte[length];

				fs.Read(buffer, 0, length);
			}

			return Convert.ToBase64String(buffer);
		}

		/// <summary>
		/// Returns the MIME type of a file.
		/// </summary>
		/// <param name="filePath">The path to the file to check.</param>
		/// <returns>A string with the MIME type.</returns>
		private string GetMimeType(string filePath)
		{
			string mimeType = "application/unknown";
			string ext = Path.GetExtension(filePath).ToLower();

			RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(ext);

			if (regKey != null && regKey.GetValue("Content Type") != null)
			{
				mimeType = regKey.GetValue("Content Type").ToString();
			}

			return mimeType;
		}

		/// <summary>
		/// Check if the file is larger than recommended and warn the user.
		/// </summary>
		/// <param name="filePath">The path to the file to check.</param>
		/// <returns>True if the file is accepted, false if the user rejected it.</returns>
		private bool CheckFileSize(string filePath)
		{
			FileInfo fi = new FileInfo(filePath);

			if (fi.Length > maxFileSize)
			{
				DialogResult dr = MessageBox.Show(
					this,
					"The file you are attempting to process is quite large, are you sure you want to do this?",
					this.Text,
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
			if (openFileDialog.ShowDialog() != DialogResult.Cancel)
			{
				ProcessFile(openFileDialog.FileName);
			}
		}

		private void buttonCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(textEncodedString.Text, TextDataFormat.Text);
		}

		private void linkWebSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://stevenbenner.com/?utm_source=apps&utm_medium=encode64&utm_campaign=Apps%3A%2BEncode64%20Tool");
		}

		private void FormMain_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Link;
			}
		}

		private void FormMain_DragDrop(object sender, DragEventArgs e)
		{
			this.Activate();

			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

			if (files.Length != 1)
			{
				MessageBox.Show(
					this,
					"Sorry, one file at a time please!",
					this.Text,
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				return;
			}

			ProcessFile(files[0]);
		}

		#endregion
	}
}
