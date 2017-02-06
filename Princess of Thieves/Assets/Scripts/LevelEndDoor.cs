using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndDoor : MonoBehaviour {
	[SerializeField]
	string nextScene;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			if (nextScene == "ThanksForPlaying") {
                Destroy(TimerManager.Instance.gameObject);
                Destroy(AudioManager.Instance.AttachedObject);
				Destroy (UIManager.Instance.gameObject);
                if (CameraManager.Instance.gameObject)
                {
                    Destroy(CameraManager.Instance.gameObject);
                }
				Destroy (GameManager.Instance.Player.gameObject);
				GameManager.Instance.EndGame ();
			}

			SceneManager.LoadScene (nextScene);
		}
	}
}
