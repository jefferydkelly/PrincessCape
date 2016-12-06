using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {
    //Whether or not this checkpoint has been activated
    bool activated = false;
    SpriteRenderer myRenderer;
    public Sprite activatedSprite;
    public Sprite inactiveSprite;
    //List with all checkpoints in the scene
    public static GameObject[] checkpoints;

	// Use this for initialization
	void Start () {
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        myRenderer = GetComponent<SpriteRenderer>();
	}
	
	void ActivateCheckpoint()
    {
        foreach(GameObject cp in checkpoints)
        {
            cp.GetComponent<Checkpoint>().Activated = false;
        }

        Activated = true;
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
}
