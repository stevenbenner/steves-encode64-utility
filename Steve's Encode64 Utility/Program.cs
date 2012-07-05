using System;
using System.Windows.Forms;

/** Steve's Encode64 Utility
 ** Copyright © 2010 Steven Benner.
 ** http://stevenbenner.com/
 * 
 * A simple tool to base64 encode files and grab the
 * encoded string. Useful for data URI creation.
 */

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
