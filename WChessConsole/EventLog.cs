using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace WChessConsole
{
	/// <summary>
	/// The severity of the message.
	/// </summary>
	enum LogLevel
	{
		Debug,
		Warning,
		Error,
		Assert
	}

	/// <summary>
	/// Allows the log level to be converted to a string.
	/// </summary>
	static class LogLevelExtension
	{
		public static LogLevel level = LogLevel.Debug;

		public static string ToPaddedString(this LogLevel level)
		{
			return string.Format("{0,-7}", level.ToString());
		}
	}

	/// <summary>
	/// The log message's tag.
	/// </summary>
	class LogTag
	{
		public string Value { get; }

		private LogTag(string value)
		{
			Value = value;
		}
	}

	sealed class EventLog
	{
		private class LogMessage
		{
			public readonly int tag;
			public readonly LogLevel level;
			public readonly long timeStamp;
			public readonly string message;

			public LogMessage(LogLevel level, int tag, string message)
			{
				this.level = level;
				this.tag = tag;
				this.message = message;
				timeStamp = DateTime.Now.Ticks;
			}

			public string ToFormattedString(string tagString)
			{
				DateTime dateTime = new DateTime(this.timeStamp);
				string text = level.ToString().Substring(0, 1);
				return text + " | " +
					   dateTime.ToLongTimeString() + " | " +
					   tagString + " | " +
					   message;
			}
		}

		private static volatile EventLog log;
		private static object mutex = new object();

		private bool logDisabled = false;
		private Dictionary<string, int> tagStringToInt;
		private Dictionary<int, string> tagIntToString;
		private int longestTagLength = 0;
		private int nextTagId;
		private List<LogMessage> messages;

		private EventLog()
		{
			messages = new List<LogMessage>();
			nextTagId = 0;
			tagStringToInt = new Dictionary<string, int>();
			tagIntToString = new Dictionary<int, string>();
		}

		/// <summary>
		/// A log message at the assertion level.
		/// </summary>
		/// <param name="tag">The message tag.</param>
		/// <param name="message">The message text.</param>
		public static void a(bool condition, LogTag tag, string message, bool showInConsole = true)
		{
			if (!condition)
			{
				write(LogLevel.Assert, tag, message, showInConsole);
				writeToFile(Utilities.GetTimeStamp() + ".txt");
				Debug.Assert(false);
			}
		}

		/// <summary>
		/// A log message at the assertion level.
		/// </summary>
		/// <param name="tag">The message tag.</param>
		/// <param name="message">The message text.</param>
		public static void a(bool condition, string tag, string message, bool showInConsole = true)
		{
			if (!condition)
			{
				write(LogLevel.Assert, tag, message, showInConsole);
				writeToFile(Utilities.GetTimeStamp() + ".txt");
				Debug.Assert(false);
			}
		}

		/// <summary>
		/// A log message at the debug level.
		/// </summary>
		/// <param name="tag">The message tag.</param>
		/// <param name="message">The message text.</param>
		public static void d(LogTag tag, string message, bool showInConsole = true)
		{
			write(LogLevel.Debug, tag, message, showInConsole);
		}

		/// <summary>
		/// A log message at the debug level.
		/// </summary>
		/// <param name="tag">The message tag.</param>
		/// <param name="message">The message text.</param>
		public static void d(string tag, string message, bool showInConsole = true)
		{
			write(LogLevel.Debug, tag, message, showInConsole);
		}

		/// <summary>
		/// A log message at the error level.
		/// </summary>
		/// <param name="tag">The message tag.</param>
		/// <param name="message">The message text.</param>
		public static void e(LogTag tag, string message, bool showInConsole = true)
		{
			write(LogLevel.Error, tag, message, showInConsole);
		}

		/// <summary>
		/// A log message at the error level.
		/// </summary>
		/// <param name="tag">The message tag.</param>
		/// <param name="message">The message text.</param>
		public static void e(string tag, string message, bool showInConsole = true)
		{
			write(LogLevel.Error, tag, message, showInConsole);
		}

		/// <summary>
		/// A log message at the warning level.
		/// </summary>
		/// <param name="tag">The message tag.</param>
		/// <param name="message">The message text.</param>
		public static void w(LogTag tag, string message, bool showInConsole = true)
		{
			write(LogLevel.Warning, tag, message, showInConsole);
		}

		/// <summary>
		/// A log message at the warning level.
		/// </summary>
		/// <param name="tag">The message tag.</param>
		/// <param name="message">The message text.</param>
		public static void w(string tag, string message, bool showInConsole = true)
		{
			write(LogLevel.Warning, tag, message, showInConsole);
		}

		/// <summary>
		/// Clear the log without writing it to the file.
		/// </summary>
		public static void clear(bool clearEditorConsole = true)
		{
			lock (mutex)
			{
				log = new EventLog();
			}
		}

		/// <summary>
		/// Write a log message.
		/// </summary>
		/// <param name="level">The log level.</param>
		/// <param name="tag">The message tag.</param>
		/// <param name="message">The message text.</param>
		public static void write(LogLevel level, LogTag tag, string message, bool showInConsole)
		{
			write(level, tag.Value, message, showInConsole);
		}

		/// <summary>
		/// Write a log message.
		/// </summary>
		/// <param name="level">The log level.</param>
		/// <param name="tag">The message tag.</param>
		/// <param name="message">The message text.</param>
		public static void write(LogLevel level, string tag, string message, bool showInConsole)
		{
			EventLog log = Instance();
			int tagId;

			message.Trim();
			tag.Trim();

			if (!log.tagStringToInt.TryGetValue(tag, out tagId))
			{
				tagId = log.nextTagId++;

				if (tag.Length > log.longestTagLength)
					log.longestTagLength = tag.Length;

				log.tagStringToInt.Add(tag, tagId);
				log.tagIntToString.Add(tagId, tag);
			}

			log.messages.Add(new LogMessage(level, tagId, message));

			if (showInConsole)
			{
				switch (level)
				{
					case LogLevel.Debug:
						Console.WriteLine("D: " + tag + " | " + message);
						break;
					case LogLevel.Warning:
						Console.WriteLine("W: " + tag + " | " + message);
						break;
					case LogLevel.Error:
						Console.WriteLine("E: " + tag + " | " + message);
						break;
					case LogLevel.Assert:
						Console.WriteLine("A: " + tag + " | " + message);
						break;
				}
			}
		}

		/// <summary>
		/// Write the log to a timestamped file.
		/// </summary>
		/// <param name="fileName">The name of the log file.</param>
		public static void writeToFile(string fileName)
		{
			EventLog log = Instance();
			string filePath = FileUtilities.buildLogPath(fileName);
			string spacer = "";

			if (filePath.EndsWith(".txt"))
			{
				if (File.Exists(filePath))
					File.Delete(filePath);

				File.Create(filePath).Dispose();

				EventLog.d("LOG", "Writing log to " + filePath);

				foreach (LogMessage l in log.messages)
				{
					string tag;
					log.tagIntToString.TryGetValue(l.tag, out tag);
					if (spacer.Length != log.longestTagLength - tag.Length)
						spacer = new string(' ', log.longestTagLength - tag.Length);
					File.AppendAllText(filePath, l.ToFormattedString(tag + spacer) + "\n");
				}
			}
			else
			{
				EventLog.e("LOG", "Filename \"" + filePath + "\" does not end in the .txt extension");
			}
		}

		/// <summary>
		/// Return the instance of the Log singleton, creating it if necessary.
		/// </summary>
		/// <returns>The instance of Log.</returns>
		private static EventLog Instance()
		{
			if (log == null)
			{
				lock (mutex)
				{
					log = new EventLog();
				}
			}

			return log;
		}
	}
}
