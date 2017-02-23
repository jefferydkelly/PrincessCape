using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndDoor : MonoBehaviour {
	[SerializeField]
	string nextScene;
    [SerializeField]
    bool endOfGame;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
            if (endOfGame)
            {
                Destroy(TimerManager.Instance.gameObject);
                AudioManager.Instance.Destroy();
                Destroy(UIManager.Instance.gameObject);
                if (CameraManager.Instance)
                {
                    Destroy(CameraManager.Instance.gameObject);
                }
                GameObject pgo = GameManager.Instance.Player.gameObject;
                Destroy(pgo);
                GameManager.Instance.EndGame();
            }
            SceneManager.LoadScene (nextScene);
		}
	}
}
