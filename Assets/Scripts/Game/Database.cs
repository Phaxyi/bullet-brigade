using System.Collections.Generic;

namespace BulletBrigade
{
	/// <summary>
	/// Holds unchanging data e.g. level messages
	/// </summary>
	public static class Database
	{
		public static readonly Dictionary<int, string> levelMsgs = new()
		{
			{0, "we're just getting started..."},
			{1, "oh, now we're talking"},
		};

		// TODO: fill these out
		public static readonly string[] ChaserMsgs = {};
	}
}