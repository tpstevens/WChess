using System;

namespace WChessConsole
{
	class Utilities
	{
		public static int Abs(int i)
		{
			return (i > 0) ? i : -1 * i;
		}

		public static void ForceExit()
		{
			Environment.Exit(1);
		}

		public static string GetTimeStamp(string format = null)
		{
			return DateTime.Now.ToString(format != null ? format : "yyyy-MM-dd_hh-mm-ss");
		}
	}
}
