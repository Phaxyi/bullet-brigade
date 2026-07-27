using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BulletBrigade
{
	public class TitleLetters : MonoBehaviour
	{
		private const string Text = "Bullet_Brigade";
		private readonly List<TMP_Text> _letters = new();
		private Transform _letterToClone;
		private float _startTime;

		private void Awake()
		{
			_startTime = Time.time;
			_letterToClone = transform.Find("Letter");
			
			foreach(char letterChar in Text)
			{
				Transform clone = Instantiate(_letterToClone, transform);
				TMP_Text text = clone.GetComponent<TMP_Text>();

				clone.name = text.text = letterChar.ToString();
				_letters.Add(text);
			}

			_letterToClone.gameObject.SetActive(false); // can't destroy?
		}

		private void Update()
		{
			float diff = (Time.time - _startTime) * 1.5f;
			int i = 0;

			foreach (TMP_Text letter in _letters)
			{
				letter.fontSize = 180 + Mathf.Sin(diff + i*0.175f) * 20;
				i++;
			}
		}
	}
}
