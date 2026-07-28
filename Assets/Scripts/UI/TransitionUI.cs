using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BulletBrigade
{
	/// <summary>
	/// Transition screens that show the level, caption and score.
	/// Also acts as game over/game complete screens (just configure it)
	/// </summary>
	public class TransitionUI : MonoBehaviour
	{
		private Canvas _canvas;
		private Graphic _canvasGraphic;
		private TMP_Text _levelNumber;
		private TMP_Text _caption;
		private TMP_Text _score;

		private void Awake()
		{
			_canvas = GetComponent<Canvas>();
			_canvasGraphic = GetComponent<Graphic>();

			_levelNumber = transform.Find("LevelNumber").GetComponent<TMP_Text>();
			_caption = transform.Find("Caption").GetComponent<TMP_Text>();
			_score = transform.Find("Score").GetComponent<TMP_Text>();
		}

		// it's cleaner to have a single function rather than listening to events in Level.cs
		public IEnumerator ShowTransition(string captionOverride = null, Color? color = null, float time = 0.7f)
		{
			int level = Game.CurrentLevel;

			_levelNumber.text = level.ToString();
			_score.text = $"score: {Mathf.Floor(Game.Score)}";
			_caption.text = captionOverride
				?? (level < Database.LevelData.Length
					? Database.LevelData[level].TransitionMsg
					: "ERROR: CAPTION NOT FOUND");

			_canvasGraphic.color = color ?? Color.brown;
			_canvas.enabled = true;
			yield return new WaitForSeconds(time);

			_canvas.enabled = false;
		}
	}
}
