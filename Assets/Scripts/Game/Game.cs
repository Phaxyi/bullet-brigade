using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace BulletBrigade {
	/// <summary>
	/// Handles levels. Persists throughout scenes.
	/// </summary>
	public class Game : MonoBehaviour
	{
		public static Action BeforeLevelChanged;
		public static Action AfterLevelChanged;
		public static Action AfterGameEnded;
		public static Game instance;

		public static bool IsGameActive { get; private set; }
		public static bool IsGamePaused { get; private set; }
		public static bool IsSingleLevel { get; private set; } // true, if from level select menu
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
		private CanvasGroup _pauseCanvasGroup;
		private PlayerInput _input;

		private void Awake()
		{
			// Ridiculous duplication check
			if (instance != null) Destroy(instance.gameObject);
			instance = this;
			DontDestroyOnLoad(gameObject);

			Enemy.EnemyDied += () => KilledEnemies++;
			Safe.SafeCollected += () => CollectedSafes++;
			Player.PlayerDied += () => EndLevel(false);
			Safezone.EnteredExitZone += OnExitZoneEnter;

			_transition = transform.Find("Canvas/Transition").GetComponent<TransitionUI>();
			_titleScreenScr = gameObject.GetComponent<TitleScreen>();
			_pauseCanvasGroup = transform.Find("Canvas/PauseScreen").GetComponent<CanvasGroup>();

			_input = GameObject.Find("/InputObj").GetComponent<PlayerInput>();
			_input.onActionTriggered += OnEscKey;
			_input.onActionTriggered += OnEnterKey;
		}

		private void OnDestroy()
		{
			_input.onActionTriggered -= OnEscKey;
			_input.onActionTriggered -= OnEnterKey;
		}

		public void StartNewGame(int selectedLevel = -1)
		{
			IsSingleLevel = selectedLevel != -1;
			IsGameActive = true;
			IsGamePaused = false;
			Hearts = 3;
			Score = 0;

			_titleScreenScr.enabled = false;
			StartLevel(IsSingleLevel ? selectedLevel : 0);
		}

		private void EndGame()
		{
			IsGameActive = false;

			AsyncOperation operation = SceneManager.LoadSceneAsync("Title", LoadSceneMode.Single);
			operation.completed += (x) =>
			{
				_pauseCanvasGroup.alpha = 0;
				_titleScreenScr.enabled = true;
				AfterGameEnded?.Invoke();
			};
		}

		private void WinGame()
		{
			StartCoroutine(_transition.ShowTransition(
				"YOU WIN!" + (IsSingleLevel ? " (a single level)" : ""), Color.seaGreen, 2
			));
			EndGame();
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
				Score += Mathf.Max(60, 240 - (Time.time - LevelStartTime)) * Hearts/3; // basic score calc

				if (IsSingleLevel || CurrentLevel + 1 == Database.LevelData.Length)
				{
					WinGame();
					return;
				}
				StartLevel(CurrentLevel + 1);
				return;
			}

			Hearts--;
			if (Hearts == 0)
			{
				EndGame();
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

		// INPUT
		public void OnEscKey(InputAction.CallbackContext context)
		{
			if (context.action.name != "EscKey") return;

			if (!IsGameActive) return;

			if (IsGamePaused)
			{
				IsGamePaused = false;
				_pauseCanvasGroup.alpha = 0;
				Time.timeScale = 1;
				return;
			}

			IsGamePaused = true;
			_pauseCanvasGroup.alpha = 1;
			Time.timeScale = 0;
		}

		public void OnEnterKey(InputAction.CallbackContext context)
		{
			if (context.action.name != "EnterKey"
				|| !(IsGameActive && IsGamePaused)) return;

			Time.timeScale = 1;
			EndGame();
		}
	}
}