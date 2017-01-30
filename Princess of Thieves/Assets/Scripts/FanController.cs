using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanController : MonoBehaviour, ActivateableObject {

	[SerializeField]
	Vector2 fwd = new Vector2(1, 0);
	[SerializeField]
	float force = 10;
	[SerializeField]
	float range = 10;
	[SerializeField]
	bool isActive = false;
	bool isPushingPlayer = false;
	LineRenderer lineRenderer;
	// Use this for initialization
	void Start () {
		lineRenderer = GetComponent<LineRenderer> ();
		lineRenderer.useWorldSpace = false;
		lineRenderer.SetPositions(new Vector3[]{Vector2.zero, (fwd * range)});
	}

	// Update is called once per frame
	void Update () {
		bool stillPushingPlayer = false;
		if (IsActive) {
			foreach (RaycastHit2D hit in Physics2D.BoxCastAll(transform.position, new Vector2(1.0f, 1.0f), 0, fwd, range)) {
				Rigidbody2D rb = hit.collider.GetComponent<Rigidbody2D> ();
				if (hit.collider.CompareTag ("Player")) {
					stillPushingPlayer = true;
					if (fwd.x != 0) {
						GameManager.Instance.Player.IsPushedHorizontallyByTheWind = true;
					} else if (fwd.y != 0) {
						GameManager.Instance.Player.IsPushedVerticallyByTheWind = true;
					}
				}
				if (rb) {
					rb.AddForce (fwd * force);
				}
			}
		}

		if (!stillPushingPlayer && isPushingPlayer) {
			if (fwd.x != 0) {
				GameManager.Instance.Player.IsPushedHorizontallyByTheWind = false;
			} else if (fwd.y != 0) {
				GameManager.Instance.Player.IsPushedVerticallyByTheWind = false;
			}
		}

		isPushingPlayer = stillPushingPlayer;
	}

	public void Activate() {
		isActive = true;
	}

	public void Deactivate() {
		isActive = false;
	}

	public bool IsActive {
		get {
			return isActive;
		}
	}
}
