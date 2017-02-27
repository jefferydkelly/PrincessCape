using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager {

	private static AudioManager instance = null;
	private AudioSource source;
	static bool isClosing = false;
	private AudioManager() {
		GameObject amObj = new GameObject ("Audio Manager");
		source = amObj.AddComponent<AudioSource> ();
	}

	public void PlaySound(AudioClip ac) {
		if (GameManager.Instance.IsInCutscene) {
			AudioListener.pause = false;
		}
		source.clip = ac;
		source.Play ();
	}

	public bool Paused {
		get {
			return !source.isPlaying;
		}

		set {
			AudioListener.pause = value;
		}
	}

	public void Destroy() {
        GameObject.Destroy(source.gameObject);
        isClosing = true;
        instance = null;
	}

    public GameObject AttachedObject
    {
        get
        {
            if (source.gameObject)
            {
                return source.gameObject;
            } else
            {
                return null;
            }
        }
    }

	public static AudioManager Instance {
		get {
			if (GameManager.Instance != null) {
				if (instance == null) {
                    instance = new AudioManager();
				}

				return instance;
			}
			return null;
		}
	}
}
