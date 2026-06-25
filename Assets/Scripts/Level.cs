using UnityEngine;

public static class Level
{
	private static int Lives = 3;
	private static int CollectedSafes = 0;
	private static int TotalSafes = 5;

	public static void CollectSafe()
	{
		CollectedSafes += 1;
		Debug.Log($"im rich -> {CollectedSafes}");
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
