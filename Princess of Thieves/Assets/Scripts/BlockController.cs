using UnityEngine;
using System.Collections;
using System;

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

        if (GameManager.Instance.Player.IsPushing)
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
			RaycastHit2D hit = Physics2D.BoxCast (transform.position, new Vector2 (0.9f, 0.9f), Vector2.down.GetAngle (), Vector2.down, 0.5f, 1 << LayerMask.NameToLayer ("Platforms"));

			if (hit.collider == null) {
				GameManager.Instance.Player.IsPushing = false;
				GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
				beingPushed = false;

			}
		}
	}
}
