using UnityEngine;
using UnityEngine.SceneManagement;

namespace BulletBrigade
{
	public class Setup : MonoBehaviour
	{
		private void Start()
		{
			DontDestroyOnLoad(gameObject);
			SceneManager.LoadSceneAsync("Title", LoadSceneMode.Single);
		}
	}
}