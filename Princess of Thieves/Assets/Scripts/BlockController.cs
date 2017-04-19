using UnityEngine;
using System.Collections;
using System;


//bug
//Blocks don't move anymore ? / Can't pull or push them
public class BlockController : ResettableObject, InteractiveObject {
	bool beingPushed = false;
	protected SpriteRenderer myRenderer;
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
        if (!beingPushed)
        {
            UIManager.Instance.HideInteraction();
        }
		myRenderer.color = regularColor;
    }

    public void Highlight()
    {
		UIManager.Instance.ShowInteraction ("Move");
		myRenderer.color = highlightedColor;
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
                Debug.Log("Let it go");
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
			} else if (GameManager.Instance.Player.Controller.Interact) {
				LetGo ();
				Input.ResetInputAxes ();
			}
		} else if (Highlighted && !GameManager.Instance.IsPaused && GameManager.Instance.Player.Controller.Interact)
        {
            Interact();
        }
	}

	void LetGo() {
        UIManager.Instance.HideInteraction();
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
        Collider2D col = collision.collider;
		if (col.CompareTag ("Slider") && !beingPushed) {
			myRigidbody.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
		} else if (col.CompareTag("Player"))
        {
            Highlight();
        }
	}

	void OnCollisionExit2D(Collision2D collision) {
		if (collision.collider.CompareTag ("Slider") && !beingPushed) {
			myRigidbody.constraints |= RigidbodyConstraints2D.FreezePositionX;
		}
        else if (collision.collider.CompareTag("Player"))
        {
            Dehighlight();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && !Highlighted)
        {
            Highlight();
        }
    }

    public Vector3 Move(Vector3 movement)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(1.0f, 1.0f), 0, movement, movement.magnitude, 1 << (LayerMask.NameToLayer("Platforms") | LayerMask.NameToLayer("Interactive") | LayerMask.NameToLayer("Background")));
        if (hit && (hit.collider.OnLayer("Platforms") || hit.collider.CompareTag("Launcher") || hit.collider.CompareTag("Block")))
        {
            transform.position = hit.point - Mathf.Sign(movement.x) * new Vector2(0.5f, 0);
        } else
        {
            transform.position += movement;
        }

        Player p = GameManager.Instance.Player;
        Vector3 pos = p.transform.position;
        pos.x = transform.position.x + p.Forward.x * (p.HalfWidth + gameObject.HalfWidth());
        p.transform.position = pos;
        return transform.position;
    }
}
