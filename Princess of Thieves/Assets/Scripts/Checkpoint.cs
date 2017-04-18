using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Checkpoint : JDMappableObject, ActivateableObject {
    //Whether or not this checkpoint has been activated
    bool activated = false;
    static List<UsableItem> checkpointInventory;
    static UsableItem checkpointLeftItem;
    static UsableItem checkpointRightItem;

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
		if (transform.childCount > 0) {
			childAnimation = transform.GetChild (0).gameObject;
            childAnimation.SetActive(false);
		}
		SceneManager.sceneLoaded += OnLevelLoaded;
	}

	void OnLevelLoaded(Scene scene, LoadSceneMode lsm) {
		activated = false;
	}
	public void Activate()
    {
		if (!IsActive) {
            if(childAnimation != null)
                childAnimation.SetActive(true);
            AudioManager.Instance.PlaySound (activateClip);
			foreach (GameObject cp in checkpoints) {
                
				cp.GetComponent<Checkpoint> ().Deactivate ();
			}
			activated = true;
			myRenderer.sprite = activatedSprite;
		}
        Player player = GameManager.Instance.Player;
        checkpointInventory = player.Inventory;
        checkpointLeftItem = player.LeftItem;
        checkpointRightItem = player.RightItem;
        activated = true;
    }

	public void Deactivate() {
		myRenderer.sprite = inactiveSprite;
		activated = false;
	}

    public bool IsActive
    {
        get
        {
            return activated;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("Player"))
        {
            Activate();
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

    public static void ResetPlayer()
    {
        Player player = GameManager.Instance.Player;
    }

	public bool IsFirst {
		get {
			return isFirst;
		}
	}

	public float ActivationTime {
		get {
			return 0;
		}
	}

	public bool IsInverted {
		get {
			return false;
		}
	}

    public static List<UsableItem> OldInventory
    {
        get
        {
            return checkpointInventory;
        }
    }

    public static UsableItem OldLeft
    {
        get
        {
            return checkpointLeftItem;
        }
    }

    public static UsableItem OldRight
    {
        get
        {
            return checkpointRightItem;
        }
    }
}
