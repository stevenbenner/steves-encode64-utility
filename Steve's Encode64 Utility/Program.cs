/*!
 * Steve's Encode64 Utility
 * Copyright © 2012 Steven Benner.
 * https://github.com/stevenbenner/steves-encode64-utility
 * 
 * A simple tool to base64 encode files and grab the
 * encoded string. Useful for data URI creation.
 */

using System;
using System.Windows.Forms;

namespace Steves_Encode64_Utility
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new FormMain());
		}
	}
}
