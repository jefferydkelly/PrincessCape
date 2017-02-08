using UnityEngine;
using System.Collections;
using System;


//bug
//Blocks don't move anymore ? / Can't pull or push them
public class BlockController : ResettableObject, InteractiveObject {
	bool beingPushed = false;
    public void Dehighlight()
    {
        UIManager.Instance.ShowInteraction("");
    }

    public void Highlight()
    {
        UIManager.Instance.ShowInteraction("Push");
    }

    public void Interact()
    {
        GameManager.Instance.Player.IsPushing = !GameManager.Instance.Player.IsPushing;

        if (GameManager.Instance.Player.IsPushing) //if we are pushing
        {
			beingPushed = true;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        } else
        {
			beingPushed = false;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }
    }

    // Use this for initialization
    void Start () {
        startPosition = transform.position;
	}

	void Update() {

		if (beingPushed) {
            Debug.Log("hello");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2 (0,-1), 1.5f, 1 << LayerMask.NameToLayer ("Platforms"));

			if (hit.collider == null) { // we stop running into things
                Debug.Log("no more objects");
                GameManager.Instance.Player.IsPushing = false;
				GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
				beingPushed = false;

			}
		}
	}
}
