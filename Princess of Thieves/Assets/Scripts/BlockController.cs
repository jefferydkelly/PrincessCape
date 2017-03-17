using UnityEngine;
using System.Collections;
using System;


//bug
//Blocks don't move anymore ? / Can't pull or push them
public class BlockController : ResettableObject, InteractiveObject {
	bool beingPushed = false;
	SpriteRenderer myRenderer;
    public void Dehighlight()
    {
		UIManager.Instance.HideInteraction ();
		myRenderer.color = Color.white;
    }

    public void Highlight()
    {
		UIManager.Instance.ShowInteraction ("Move");
		myRenderer.color = Color.blue;
    }

	void OnMouseEnter() {
		if (GameManager.Instance.Player.CanInteract) {
			Highlight ();
		}
	}

	void OnMouseExit() {
		if (GameManager.Instance.Player.CanInteract) {
			Dehighlight ();
		}
	}

	void OnMouseOver() {
		if (GameManager.Instance.Player.CanInteract) {
			if (Highlighted) {
				if (GameManager.Instance.InPlayerInteractRange (gameObject)) {
					if (GameManager.Instance.Player.Controller.Interact) {
						Input.ResetInputAxes ();
						Interact ();

					}
				} else if (!beingPushed) {
					Dehighlight ();
				}
			}
		}
	}

    public void Interact()
    {
		if (!GameManager.Instance.IsPaused) {
			UIManager.Instance.ShowInteraction ("Let Go");
			myRenderer.color = Color.white;
        

			if (!GameManager.Instance.Player.IsPushing) { //if we are pushing
				beingPushed = true;
				GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeRotation;
				GameManager.Instance.Player.Push (this);
			} else {
				
				beingPushed = false;
				GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
				GameManager.Instance.Player.IsPushing = false;
				//GameManager.Instance.Player.Freeze (0.1f);
			}
		}
    }

    // Use this for initialization
    void Start () {
        startPosition = transform.position;
		myRenderer = GetComponent<SpriteRenderer> ();
	}

	void Update() {

		if (beingPushed) {
			RaycastHit2D hit = Physics2D.BoxCast (transform.position, new Vector2 (1, 1), 0, new Vector2 (0, -1), 1, 1 << LayerMask.NameToLayer ("Platforms"));

			if (hit.collider == null) { // we stop running into things
				LetGo();
			} else if (Input.GetMouseButtonDown (0)) {
				LetGo ();
				Input.ResetInputAxes ();
			}
		}
	}

	void LetGo() {
		GameManager.Instance.Player.IsPushing = false;
		GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
		beingPushed = false;
	}
	bool Highlighted {
		get {
			return myRenderer.color == Color.blue;
		}
	}
}
