using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace BulletBrigade
{
	public class TasksUI : MonoBehaviour
	{
		private TMP_Text _levelText, _timerText, _safesText, _enemiesText;

		private void Awake()
		{
			_levelText = transform.Find("LevelText").GetComponent<TMP_Text>();
			_timerText = transform.Find("TimerText").GetComponent<TMP_Text>();
			_safesText = transform.Find("SafesText").GetComponent<TMP_Text>();
			_enemiesText = transform.Find("EnemiesText").GetComponent<TMP_Text>();

			Game.AfterLevelChanged += RefreshUI;
			Safe.SafeCollected += UpdateSafeCount;
			Enemy.EnemyDied += UpdateEnemyCount;
		}

		private void Update()
		{
			TimeSpan passed = TimeSpan.FromSeconds(Time.time - Game.LevelStartTime);
			_timerText.text = passed.ToString("mm':'ss':'ff");
		}

		private void RefreshUI()
		{
			_levelText.text = $"LEVEL {Game.CurrentLevel}";
			// initial refresh
			UpdateSafeCount();
			UpdateEnemyCount();
		}

		private void UpdateSafeCount()
		{
			_safesText.text = $"{Game.CollectedSafes} / {Game.TotalSafes} safes";
			if (Game.CollectedSafes == Game.TotalSafes)
			{
				_safesText.DOColor(Color.darkGreen, 0.5f);
			}
		}

		private void UpdateEnemyCount()
		{
			_enemiesText.text = $"{Game.KilledEnemies} / {Game.TotalEnemies} killed";
			if (Game.KilledEnemies == Game.TotalEnemies)
			{
				_enemiesText.DOColor(Color.darkGreen, 0.5f);
			}
		}
	}

}
