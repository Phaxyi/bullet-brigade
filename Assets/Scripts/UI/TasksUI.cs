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

			Level.BeforeLevelChanged += RefreshUI;
			Safe.SafeCollected += UpdateSafeCount;
			Enemy.EnemyDied += UpdateEnemyCount;
		}

		private void Update()
		{
			TimeSpan passed = TimeSpan.FromSeconds(Time.time - Level.LevelStartTime);
			_timerText.text = passed.ToString("mm':'ss':'ff");
		}

		private void RefreshUI()
		{
			_levelText.text = $"LEVEL {Level.CurrentLevel}";
			// initial refresh
			UpdateSafeCount();
			UpdateEnemyCount();
		}

		private void UpdateSafeCount()
		{
			_safesText.text = $"{Level.CollectedSafes} / {Level.TotalSafes} safes";
			if (Level.CollectedSafes == Level.TotalSafes)
			{
				_safesText.DOColor(Color.darkGreen, 0.5f);
			}
		}

		private void UpdateEnemyCount()
		{
			_enemiesText.text = $"{Level.KilledEnemies} / {Level.TotalEnemies} killed";
			if (Level.KilledEnemies == Level.TotalEnemies)
			{
				_enemiesText.DOColor(Color.darkGreen, 0.5f);
			}
		}
	}

}
