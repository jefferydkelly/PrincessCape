using UnityEngine;
using System.Collections;
using System;


//bug
//Blocks don't move anymore ? / Can't pull or push them
public class BlockController : ResettableObject, InteractiveObject {
	bool beingPushed = false;
	SpriteRenderer myRenderer;
	protected Rigidbody2D myRigidbody;
    [SerializeField]
    protected Color regularColor = Color.white;
    [SerializeField]
    protected Color highlightedColor = Color.blue;

	void Awake() {
		myRigidbody = GetComponent<Rigidbody2D> ();
	}
    public void Dehighlight()
    {
		UIManager.Instance.HideInteraction ();
		myRenderer.color = regularColor;
    }

    public void Highlight()
    {
		UIManager.Instance.ShowInteraction ("Move");
		myRenderer.color = highlightedColor;
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
			myRenderer.color = regularColor;
        

			if (!GameManager.Instance.Player.IsPushing) { //if we are pushing
				beingPushed = true;
				myRigidbody.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
				GameManager.Instance.Player.Push (this);
			} else {
				
				beingPushed = false;
				myRigidbody.constraints |= RigidbodyConstraints2D.FreezePositionX;
				GameManager.Instance.Player.IsPushing = false;
				//GameManager.Instance.Player.Freeze (0.1f);
			}
		}
    }

    // Use this for initialization
    void Start () {
        startPosition = transform.position;
		myRenderer = GetComponent<SpriteRenderer> ();
        myRenderer.color = regularColor;
	}

	void Update() {

		if (beingPushed) {
			RaycastHit2D hit = Physics2D.BoxCast (transform.position, new Vector2 (1f, 1f), 0, Vector2.down, 1.5f, 1 << LayerMask.NameToLayer ("Platforms"));

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
		myRigidbody.constraints |= RigidbodyConstraints2D.FreezePositionX;
		beingPushed = false;
	}
	bool Highlighted {
		get {
			return myRenderer.color == highlightedColor;
		}
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.collider.CompareTag ("Slider") && !beingPushed) {
			myRigidbody.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
		}
	}

	void OnCollisionExit2D(Collision2D collision) {
		if (collision.collider.CompareTag ("Slider") && !beingPushed) {
			myRigidbody.constraints |= RigidbodyConstraints2D.FreezePositionX;
		}
	}
}
