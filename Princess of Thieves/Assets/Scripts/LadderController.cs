using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : JDMappableObject {
	bool ladderAbove;
	bool ladderBelow;
	bool collidingWithPlayer;
	void Start() {
		CheckForConnections ();
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

        if (GameManager.Instance.Player != null)
        {
            if (collidingWithPlayer && GameManager.Instance.Player.IsClimbing)
            {
                GameManager.Instance.Player.IsClimbing = false;
            }
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

			if (player.IsClimbing && player.Position.y >= transform.position.y) {
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

	void LinkLadder(bool above) {
		if (above) {
			ladderAbove = true;
		} else {
			ladderBelow = true;
		}
	}
}
