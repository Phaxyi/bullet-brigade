using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace BulletBrigade
{
	public class TransitionUI : MonoBehaviour
	{
		private Canvas _canvas;
		private TMP_Text _levelNumber;
		private TMP_Text _caption;
		private TMP_Text _score;

		private void Awake()
		{
			_canvas = GetComponent<Canvas>();
			_levelNumber = transform.Find("LevelNumber").GetComponent<TMP_Text>();
			_caption = transform.Find("Caption").GetComponent<TMP_Text>();
			_score = transform.Find("Score").GetComponent<TMP_Text>();
		}

		// it's cleaner to have a single function rather than listening to events in Level.cs
		// TODO: color
		public IEnumerator ShowTransition(string caption, float time = 0.75f)
		{
			_levelNumber.text = Level.CurrentLevel.ToString();
			_caption.text = caption;
			_score.text = Level.Score.ToString();

			_canvas.enabled = true;
			yield return new WaitForSeconds(time);
			_canvas.enabled = false;
		}
	}
}
