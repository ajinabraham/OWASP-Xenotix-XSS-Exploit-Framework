using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace net.jsBeautifier
{
	static class Program
	{
		#region Main Method
		[STAThread]
		static void Main (string[] args)
		{
			ResourceExtractor.ensureResources();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			Program.startBeautifier(args);
		}
		#endregion

		#region Private Methods
		private static void beautifyBackground (string sourceFile, string destinationFile, int? indent, bool? bracesInNewLine, bool? preserveEmptyLines,
				bool? detectPackers, bool? keepArrayIndent)
		{
			JsBeautifier b=new JsBeautifier();

			b.BeautifierReady += delegate(object sender, EventArgs e)
			{
				if (indent.HasValue) b.setIndentation(indent.Value);
				if (bracesInNewLine.HasValue) b.setBracesNewLine(bracesInNewLine.Value);
				if (preserveEmptyLines.HasValue) b.setPreserveEmptyLines(preserveEmptyLines.Value);
				if (detectPackers.HasValue) b.setDetectPackers(detectPackers.Value);
				if (keepArrayIndent.HasValue) b.setKeepArrayIndent(keepArrayIndent.Value);

				string beautified=b.getBeautifiedScript(System.IO.File.ReadAllText(sourceFile));

				System.IO.File.WriteAllText(destinationFile, beautified);

				b.Close();
			};

			b.Shown += delegate(object sender, EventArgs e)
			{
				b.WindowState = FormWindowState.Minimized;
				b.Location = new System.Drawing.Point(800, 800);
				b.Size = new System.Drawing.Size(100, 100);
			};

			b.ShowDialog();
		}

		private static void startBeautifier (string[] args)
		{
            string sourceFile = "tmp.js", destinationFile = "beautified.js";
            int? indent = 2;
            bool? bracesInNewLine = false, preserveEmptyLines = true, detectPackers = true, keepArrayIndent = false;
            try
            {
                beautifyBackground(sourceFile, destinationFile, indent, bracesInNewLine, preserveEmptyLines, detectPackers, keepArrayIndent);
              
            }
            catch (Exception ee)
            {
                Console.WriteLine("Error:" + ee.Message.ToString());
            }
		
		}
		#endregion
	}
}
