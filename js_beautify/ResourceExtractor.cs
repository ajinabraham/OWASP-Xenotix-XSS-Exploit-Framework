using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace net.jsBeautifier
{
	static class ResourceExtractor
	{
		#region Internal Properties
		internal static string resourceBaseDir
		{
			get
			{
				return (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\jsBeautifier");
			}
		}
		#endregion

		#region Internal Methods
		internal static void ensureResources ()
		{
			string resourceDir=ResourceExtractor.resourceBaseDir;
			if (!Directory.Exists(resourceDir))
			{
				Directory.CreateDirectory(resourceDir);
			}

			string zipFile=resourceDir + "\\jsBeautifier.zip";
			File.WriteAllBytes(zipFile, net.jsBeautifier.Properties.Resources.jsBeautifier);

			ZipInputStream zipStream=new ZipInputStream(File.OpenRead(zipFile));

			ZipEntry entry;
			string tmpEntry = String.Empty;
			while ((entry = zipStream.GetNextEntry()) != null)
			{
				string fileName = Path.GetFileName(entry.Name);

				if (fileName != String.Empty)
				{
					string fullPath = resourceDir + "\\" + entry.Name;
					fullPath = fullPath.Replace("\\ ", "\\");
					string fullDirPath = Path.GetDirectoryName(fullPath);
					if (!Directory.Exists(fullDirPath)) Directory.CreateDirectory(fullDirPath);
					FileStream streamWriter = File.Create(fullPath);
					int size = 2048;
					byte[] data = new byte[2048];
					while (true)
					{
						size = zipStream.Read(data, 0, data.Length);
						if (size > 0)
						{
							streamWriter.Write(data, 0, size);
						}
						else
						{
							break;
						}
					}
					streamWriter.Close();
				}
			}
			zipStream.Close();
		}
		#endregion
	}
}
