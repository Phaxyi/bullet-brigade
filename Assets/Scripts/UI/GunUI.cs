using TMPro;
using UnityEngine;

namespace BulletBrigade {
	/// <summary>
	/// Visualises gun ammo, reloading etc.
	/// </summary>
	public class GunUI : MonoBehaviour
	{
		private Gun _gun;
		private TMP_Text _text;

		private void Awake()
		{
			_gun = GameObject.FindWithTag("Player").GetComponent<Entity>().GetComponent<Gun>();
			_text = transform.Find("Text").GetComponent<TMP_Text>();
		}

		private void Update()
		{
			bool isReloading = (Time.time - _gun.LastReloadTime) < _gun.ReloadCooldown;
			string currMagText = isReloading ? "0" : _gun.BulletsInMag.ToString();
			string reloadText = isReloading ? "<size='34'>(reloading!)</size> " : "";

			_text.text = $"{reloadText}{currMagText} / {_gun.MagazineSize}";
		}
	}
}