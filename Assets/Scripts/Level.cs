using UnityEngine;

public static class Level
{
	public static int Lives { get; set; } = 3;
	public static int CollectedSafes { get; set; } = 0;
	public static int TotalSafes { get; set; } = 5;

	public static void CollectSafe()
	{
		CollectedSafes += 1;
		Debug.Log($"collected safe -> {CollectedSafes}");
		if (CollectedSafes == TotalSafes)
		{
			WinLevel();
		}
	}

	public static void LoseLevel()
	{
		Lives -= 1;
		if (Lives == 0)
		{
			// reset from start
		}
	}

	public static void WinLevel()
	{
		// ...
	}
}
