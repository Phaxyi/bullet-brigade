using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BulletBrigade {
	/// <summary>
	/// Handles levels. Persists throughout scenes.
	/// </summary>
	public class Game : MonoBehaviour
	{
		public static Action BeforeLevelChanged;
		public static Action AfterLevelChanged;

		public static bool IsGameActive { get; private set; }
		public static float LevelStartTime { get; private set; }
		public static float Score { get; private set; }
		public static int CurrentLevel { get; private set; }
		public static int Hearts { get; private set; }
		public static int CollectedSafes { get; private set; }
		public static int TotalSafes { get; private set; }
		public static int KilledEnemies { get; private set; }
		public static int TotalEnemies { get; private set; }

		private TransitionUI _transition;
		private TitleScreen _titleScreenScr;

		private void Awake()
		{
			Enemy.EnemyDied += () => KilledEnemies++;
			Safe.SafeCollected += () => CollectedSafes++;
			Player.PlayerDied += () => EndLevel(false);
			Safezone.EnteredExitZone += OnExitZoneEnter;

			_transition = transform.Find("Transition").GetComponent<TransitionUI>();
			_titleScreenScr = gameObject.GetComponent<TitleScreen>();

			DontDestroyOnLoad(gameObject);
		}

		public void StartNewGame()
		{
			IsGameActive = true;
			Hearts = 3;
			Score = 0;

			_titleScreenScr.enabled = false;
			StartLevel(0);
		}

		public void ReturnToTitle()
		{
			IsGameActive = false;

			AsyncOperation operation = SceneManager.LoadSceneAsync("Title", LoadSceneMode.Single);
			operation.completed += (x) => _titleScreenScr.enabled = true;
		}

		private void StartLevel(int newLevel)
		{
			CurrentLevel = newLevel;
			BeforeLevelChanged?.Invoke();

			AsyncOperation operation = SceneManager.LoadSceneAsync(newLevel.ToString(), LoadSceneMode.Single);
			StartCoroutine(_transition.ShowTransition());

			operation.completed += (x) =>
			{
				LevelStartTime = Time.time;
				TotalEnemies = GameObject.Find("/Enemies").transform.childCount;
				TotalSafes = GameObject.Find("/Safes").transform.childCount;
				CollectedSafes = 0;
				KilledEnemies = 0;
				
				AfterLevelChanged?.Invoke();
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
				ReturnToTitle();
				return;
			}
			StartLevel(CurrentLevel);
		}

		private void OnExitZoneEnter()
		{
			if (CollectedSafes == TotalSafes && KilledEnemies == TotalEnemies)
			{
				EndLevel(true);
			}
		}
	}
}