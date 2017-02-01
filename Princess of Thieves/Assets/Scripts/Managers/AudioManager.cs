using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager:Object {

	private static AudioManager instance = null;
	private AudioSource source;

	private AudioManager() {
		GameObject amObj = new GameObject ("Audio Manager");
		DontDestroyOnLoad (amObj);
		source = amObj.AddComponent<AudioSource> ();
	}

	public void PlaySound(AudioClip ac) {
		source.clip = ac;
		source.Play ();
	}

	public static AudioManager Instance {
		get {
			if (instance == null) {
				instance = new AudioManager ();
			}

			return instance;
		}
	}
}
