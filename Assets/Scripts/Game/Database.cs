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
	}

	public static class Database
	{
		public static readonly LevelData[] LevelData = {
			new()
			{
				Name = "START",
				Desc = "the start of Hell",
				TransitionMsg = "are you ready for whats to come?",
			},
			new()
			{
				Name = "one",
				Desc = "asdasdadad",
				TransitionMsg = "ardgdsgdsgfdgdfsdgme?",
			},
			new()
			{
				Name = "two",
				Desc = "asdasdadad",
				TransitionMsg = "ardgdsgdsgfdgdfsdgme?",
			},
			new()
			{
				Name = "three",
				Desc = "asdasdadad",
				TransitionMsg = "ardgdsgdsgfdgdfsdgme?",
			},
		};
	}
}