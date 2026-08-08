namespace BulletBrigade
{
	/// <summary>
	/// Holds unchanging data e.g. level messages
	/// </summary>
	
	// TODO: change struct to readonly
	public struct LevelData
	{
		public string Name;
		public string Desc;
		public string TransitionMsg;
		public int KillsNeeded;  // -1 for all enemies
	}

	public static class Database
	{
		public static readonly LevelData[] LevelData = {
			new()
			{
				Name = "START",
				Desc = "the start of Hell",
				TransitionMsg = "are you ready for whats to come?",
				KillsNeeded = -1,
			},
			new()
			{
				Name = "one",
				Desc = "asdasdadad",
				TransitionMsg = "ardgdsgdsgfdgdfsdgme?",
				KillsNeeded = -1,
			},
			new()
			{
				Name = "two",
				Desc = "asdasdadad",
				TransitionMsg = "ardgdsgdsgfdgdfsdgme?",
				KillsNeeded = -1,
			},
			new()
			{
				Name = "three",
				Desc = "asdasdadad",
				TransitionMsg = "ardgdsgdsgfdgdfsdgme?",
				KillsNeeded = 0,
			},
		};
	}
}