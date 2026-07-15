using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BulletBrigade {
	/// <summary>
	/// Handles levels. Persists throughout scenes.
	/// </summary>
	public class Level : MonoBehaviour
	{
		public static Action BeforeLevelChanged;

		public static float LevelStartTime { get; private set; }
		public static float Score { get; private set; }
		public static int CurrentLevel { get; private set; }
		public static int Hearts { get; private set; }
		public static int CollectedSafes { get; private set; }
		public static int TotalSafes { get; private set; }
		public static int KilledEnemies { get; private set; }
		public static int TotalEnemies { get; private set; }

		private TransitionUI _transition;

		private void Awake()
		{
			Enemy.EnemyDied += () => KilledEnemies++;
			Safe.SafeCollected += () => CollectedSafes++;
			Player.PlayerDied += () => EndLevel(false);
			Safezone.TouchedExitZone += OnSafezoneEntered;

			_transition = GameObject.Find("/Transition").GetComponent<TransitionUI>();
			DontDestroyOnLoad(gameObject);
			DontDestroyOnLoad(_transition);
		}

		public void StartNewGame()
		{
			StartCoroutine(_transition.ShowTransition("starting new game"));
			Hearts = 3;
			StartLevel(0);
		}

		private void StartLevel(int newLevel)
		{
			CurrentLevel = newLevel;
			BeforeLevelChanged?.Invoke();

			AsyncOperation operation = SceneManager.LoadSceneAsync(newLevel.ToString(), LoadSceneMode.Single);
			StartCoroutine(_transition.ShowTransition("test caption please use database"));

			operation.completed += (x) =>
			{
				LevelStartTime = Time.time;
				TotalEnemies = GameObject.Find("/Enemies").transform.childCount;
				TotalSafes = GameObject.Find("/Safes").transform.childCount;
				CollectedSafes = 0;
				KilledEnemies = 0;
				// AfterLevelChanged?.Invoke();
			};
		} 

		private void EndLevel(bool win)
		{
			if (win)
			{
				// basic score calc
				Score += Mathf.Max(60, 240 - (Time.time - LevelStartTime)) * Hearts/3;
				StartLevel(CurrentLevel + 1);
				return;
			}

			Hearts--;
			if (Hearts == 0)
			{
				StartNewGame();
				return;
			}
			StartLevel(CurrentLevel);
		}

		private void OnSafezoneEntered()
		{
			if (CollectedSafes == TotalSafes && KilledEnemies == TotalEnemies)
			{
				EndLevel(true);
			}
		}
	}
}