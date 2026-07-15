using BulletBrigade;
using TMPro;
using UnityEngine;

public class TasksUI : MonoBehaviour
{
	private TMP_Text _levelText;
	private TMP_Text _safesText;
	private TMP_Text _enemiesText;

	private void Awake()
	{
		_levelText = transform.Find("LevelText").GetComponent<TMP_Text>();
		_safesText = transform.Find("SafesText").GetComponent<TMP_Text>();
		_enemiesText = transform.Find("EnemiesText").GetComponent<TMP_Text>();

		Level.LevelChanged += RefreshUI;
		Safe.SafeCollected += UpdateSafeCount;
		Enemy.EnemyDied += UpdateEnemyCount;
	}

	public void RefreshUI()
	{
		_levelText.text = $"LEVEL {Level.CurrLevel}";

		// do initial refresh
		UpdateSafeCount();
		UpdateEnemyCount();
	}

	private void UpdateSafeCount()
	{
		_safesText.text = $"{Level.CollectedSafes} / {Level.TotalSafes} safes";
	}

	private void UpdateEnemyCount()
	{
		_enemiesText.text = $"{Level.KilledEnemies} / {Level.TotalEnemies} killed";
	}
}
