using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager {

	private static AudioManager instance = null;
	private AudioSource source;
	static bool isClosing = false;
	private AudioManager() {
		GameObject amObj = new GameObject ("Audio Manager");
		source = amObj.AddComponent<AudioSource> ();
	}

	public void PlaySound(AudioClip ac) {
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

	void OnDestroy() {
        isClosing = true;
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
			if (!isClosing) {
				if (instance == null) {
                    instance = new AudioManager();
				}

				return instance;
			}
			Debug.Log ("You're trying to reference the audio manager while the game is closing.");
			return null;
		}
	}
}
