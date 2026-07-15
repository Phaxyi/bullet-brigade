using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BulletBrigade {
	/// <summary>
	/// Handles levels. Persists throughout scenes.
	/// </summary>
	public class Level : MonoBehaviour
	{
		public static Action LevelChanged;

		public static int CurrLevel { get; private set; }
		public static int Lives { get; private set; }
		public static int CollectedSafes { get; private set; }
		public static int TotalSafes { get; private set; }
		public static int KilledEnemies { get; private set; }
		public static int TotalEnemies { get; private set; }

		private void Awake()
		{
			Enemy.EnemyDied += () => KilledEnemies++;
			Safe.SafeCollected += () => CollectedSafes++;
			Safezone.TouchedExitZone += OnSafezoneEntered;

			DontDestroyOnLoad(gameObject);
		}

		public static void StartNewGame()
		{
			Lives = 3;
			StartLevel(0);
		}

		private static void StartLevel(int newLevel)
		{
			CurrLevel = newLevel;
			AsyncOperation operation = SceneManager.LoadSceneAsync(newLevel.ToString(), LoadSceneMode.Single);

			operation.completed += (x) =>
			{
				TotalEnemies = GameObject.Find("/Enemies").transform.childCount;
				TotalSafes = GameObject.Find("/Safes").transform.childCount;
				CollectedSafes = 0;
				KilledEnemies = 0;

				LevelChanged?.Invoke();
			};
		} 

		private static void EndLevel(bool win)
		{
			if (win)
			{
				StartLevel(CurrLevel + 1);
				return;
			}

			Lives--;
			if (Lives == 0)
			{
				StartNewGame();
				return;
			}
			StartLevel(CurrLevel);
		}

		private static void OnSafezoneEntered()
		{
			if (CollectedSafes == TotalSafes && KilledEnemies == TotalEnemies)
			{
				EndLevel(true);
			}
		}
	}
}