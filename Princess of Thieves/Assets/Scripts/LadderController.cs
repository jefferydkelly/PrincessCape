using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : JDMappableObject, InteractiveObject {
	SpriteRenderer myRenderer;
	bool ladderAbove;
	bool ladderBelow;
	bool collidingWithPlayer;
	void Start() {
		myRenderer = GetComponent<SpriteRenderer> ();

	}
	public void Interact() {
		Player player = GameManager.Instance.Player;
		if (!player.IsClimbing) {
			player.IsClimbing = true;
            Vector3 pos;
            if (GameManager.Instance.Player.transform.position.y < gameObject.transform.position.y)
			    pos = transform.position - new Vector3 (0, gameObject.HalfHeight () - player.HalfHeight);
            else
                pos = transform.position + new Vector3(0, gameObject.HalfHeight() - player.HalfHeight);
            pos.z = 0;
			player.transform.position = pos;
			myRenderer.color = Color.white;
		} else {
			player.IsClimbing = false;
		}
	}

	public void Highlight() {
		if (!GameManager.Instance.Player.IsClimbing) {
			myRenderer.color = Color.blue;
			UIManager.Instance.ShowInteraction ("Climb");
		} else {
			UIManager.Instance.ShowInteraction ("Get Off");
		}
	}

	public void Dehighlight() {
		myRenderer.color = Color.white;
		UIManager.Instance.ShowInteraction ("");
	}

	public void CheckForConnections() {
		RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.down, gameObject.HalfHeight () + 0.1f, 1 << LayerMask.NameToLayer ("Interactive"));
		if (hit.collider != null) {
			ladderBelow = hit.collider.CompareTag ("Ladder");
		}

		hit = Physics2D.Raycast (transform.position, Vector2.up, gameObject.HalfHeight () + 0.1f, 1 << LayerMask.NameToLayer ("Interactive"));
		if (hit.collider != null) {
			ladderAbove = hit.collider.CompareTag ("Ladder");
		}
	}

	void OnDestroy() {

		if (myRenderer.color == Color.blue) {
			GameManager.Instance.Player.HighlightedDestroyed (this);
		}
		if (collidingWithPlayer && GameManager.Instance.Player.IsClimbing) {
			GameManager.Instance.Player.IsClimbing = false;
			GameManager.Instance.Player.HighlightedDestroyed (this);
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag ("Player")) {
			collidingWithPlayer = true;
		}
	}

	void OnTriggerExit2D(Collider2D col) {
		if (col.CompareTag ("Player")) {
			collidingWithPlayer = false;
		}
	}
	public bool LadderAbove {
		get {
			return ladderAbove;
		}
	}
		
	public bool LadderBelow {
		get {
			return ladderBelow;
		}
	}
}
