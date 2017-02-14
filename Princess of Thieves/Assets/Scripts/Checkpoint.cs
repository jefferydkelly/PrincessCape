﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Checkpoint : JDMappableObject {
    //Whether or not this checkpoint has been activated
    bool activated = false;
    SpriteRenderer myRenderer;
    public Sprite activatedSprite;
    public Sprite inactiveSprite;
    //List with all checkpoints in the scene
    public static GameObject[] checkpoints;

    private GameObject childAnimation;
	[SerializeField]
	AudioClip activateClip;
	[SerializeField]
	bool isFirst = false;
	// Use this for initialization
	void Awake () {
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        myRenderer = GetComponent<SpriteRenderer>();
        if(transform.GetChild(0))
            childAnimation = transform.GetChild(0).gameObject;
		SceneManager.sceneLoaded += OnLevelLoaded;
	}

	void OnLevelLoaded(Scene scene, LoadSceneMode lsm) {
		activated = false;
	}
	void ActivateCheckpoint()
    {
		if (!Activated) {
			AudioManager.Instance.PlaySound (activateClip);
			foreach (GameObject cp in checkpoints) {
				cp.GetComponent<Checkpoint> ().Activated = false;
			}

			Activated = true;
		}
    }

    public bool Activated
    {
        get
        {
            return activated;
        }

        set
        {
            activated = value;
            if (value)
            {
                childAnimation.SetActive(true);
            }
            myRenderer.sprite = activated ? activatedSprite : inactiveSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ActivateCheckpoint();
        }
    }

    public static Vector3 ActiveCheckpointPosition
    {
        get
        {
            if (checkpoints != null)
            {
                foreach (GameObject cp in checkpoints)
                {
                    if (cp.GetComponent<Checkpoint>().activated)
                    {
                        return cp.transform.position;
                    }
                }
            }

            return Vector3.zero;
        }
    }

	public bool IsFirst {
		get {
			return isFirst;
		}
	}
}
