﻿using System;
using System.IO;

namespace WChessConsole
{
	class FileUtilities
	{
		private static readonly string LOGTAG = "FILE-UTILITIES";

		/// <summary>
		/// Create the path to the log files.
		/// </summary>
		/// <param name="filename">The name of the file.</param>
		/// <returns></returns>
		public static string buildLogPath(string filename)
		{
			string path = "logs/";
			ensureFolderExists(path);
			return path + filename;
		}

		/// <summary>
		/// Create the directory containing the given file if it doesn't exist.
		/// </summary>
		/// <param name="folderPath">
		/// The folder path. Everything after the last / will be deleted.
		/// </param>
		private static void ensureFolderExists(string folderPath)
		{
			// remove all characters after the last /
			if (!folderPath.EndsWith("/"))
			{
				int slashIndex = folderPath.LastIndexOf('/');
				folderPath = ((slashIndex == -1) ? "" : folderPath.Substring(0, slashIndex + 1));
			}

			// create directory if necessary
			if (folderPath != "" && !Directory.Exists(folderPath))
			{
				try
				{
					Directory.CreateDirectory(folderPath);
					EventLog.d(LOGTAG, "Successfully created directory \"" + folderPath + "\"");
				}
				catch (Exception ex)
				{
					EventLog.e(LOGTAG, "Failed to create directory \"" + folderPath + "\": " + ex.Message);
				}
			}
		}
	}
}
