using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentalAudioScript : MonoBehaviour {


    int keyPresses = 0;
    int passes = 0;
    AudioSource myAudio;
	// Use this for initialization
	void Start () {
        myAudio = GetComponent<AudioSource>();	
	}
	
	// Update is called once per frame
	void Update () {
        //Something is done by the player
        if (Input.anyKeyDown)
        {
            keyPresses++;
           
        }
	}

    
    void FixedUpdate()
    {
        passes+=1;
        if (passes >= 59)
        {
            Debug.Log("here");
            GenerateSoundValue(keyPresses);
            passes = 0;
            keyPresses = 0;
        }
    }
    /// <summary>
    /// Generates a percentage which will be the volume that the music track plays to.
    /// kP = KeyPresses, but localized.
    /// </summary>
    /// <param name="kP"></param>
    void GenerateSoundValue(int kP)
    {
        Debug.Log(kP);
        myAudio.volume = .25f + kP*.10f;

    }
}
