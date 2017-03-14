using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : JDMappableObject, InteractiveObject {
	SpriteRenderer myRenderer;
	Collider2D myCollider;
	bool ladderAbove;
	bool ladderBelow;
	bool collidingWithPlayer;
	void Start() {
		myRenderer = GetComponent<SpriteRenderer> ();
		CheckForConnections ();
		myCollider = GetComponent<Collider2D> ();

	}
	public void Interact() {
		Player player = GameManager.Instance.Player;
		if (!player.IsClimbing) {
			myRenderer.color = Color.white;
			player.IsClimbing = true;
            Vector3 pos;
			pos = player.transform.position;
			pos.x = transform.position.x;
		
			if (pos.y >= transform.position.y + gameObject.HalfHeight () + player.HalfHeight - 0.15f) {
				myCollider.isTrigger = true;
				pos.y = transform.position.y + gameObject.HalfHeight () + player.HalfHeight - 1f;
			}
            pos.z = 0;
			player.transform.position = pos;
			myRenderer.color = Color.white;
			UIManager.Instance.ShowInteraction ("Get Off");
			Input.ResetInputAxes ();
		}

	}

	public void Highlight() {
		if (!GameManager.Instance.Player.IsClimbing) {
			myRenderer.color = Color.blue;
			UIManager.Instance.ShowInteraction ("Climb");
		}
	}

	public void Dehighlight() {
		myRenderer.color = Color.white;
		if (!GameManager.Instance.Player.IsClimbing) {
			UIManager.Instance.HideInteraction ();
		}
	}

	public void CheckForConnections() {
		RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.down, gameObject.HalfHeight () + 0.1f, 1 << LayerMask.NameToLayer ("Interactive"));
		if (hit.collider != null) {
			ladderBelow = hit.collider.CompareTag ("Ladder");
			if (ladderBelow) {
				hit.collider.GetComponent<LadderController> ().LinkLadder (true);
			}
		}

		hit = Physics2D.Raycast (transform.position, Vector2.up, gameObject.HalfHeight () + 0.1f, 1 << LayerMask.NameToLayer ("Interactive"));
		if (hit.collider != null) {
			ladderAbove = hit.collider.CompareTag ("Ladder");
			if (ladderAbove) {
				hit.collider.GetComponent<LadderController> ().LinkLadder (false);
			}
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
			Player player = GameManager.Instance.Player;

			if (player.Position.y >= transform.position.y) {
				Vector3 pos = player.Position;
				pos.y = gameObject.transform.position.y + gameObject.HalfHeight () + player.HalfHeight + 0.01f;
				player.transform.position = pos;
			}
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

	bool Highlighted {
		get {
			return myRenderer.color == Color.blue;
		}
	}

	void OnMouseEnter() {
		if (!GameManager.Instance.IsPaused) {
			Highlight ();
		}
	}

	void OnMouseExit() {
		if (!GameManager.Instance.IsPaused) {
			Dehighlight ();
		}
	}

	void LinkLadder(bool above) {
		if (above) {
			ladderAbove = true;
		} else {
			ladderBelow = true;
		}
	}
	void OnMouseOver() {
		if (!GameManager.Instance.IsPaused) {
			if (Highlighted) {
				if (GameManager.Instance.InPlayerInteractRange (gameObject)) {
					if (GameManager.Instance.Player.Controller.Interact) {
						Interact ();
						Input.ResetInputAxes ();
					}
				} else {
					Dehighlight ();
				}
			}
		}
	}
}
