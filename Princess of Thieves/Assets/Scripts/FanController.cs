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
	LineRenderer lineRenderer;
	// Use this for initialization
	void Start () {
		lineRenderer = GetComponent<LineRenderer> ();
		Vector2 startPos = new Vector2 (transform.position.x + gameObject.HalfWidth (), transform.position.y);
		lineRenderer.SetPositions(new Vector3[]{startPos, startPos + (fwd * range)});
	}
	
	// Update is called once per frame
	void Update () {
		if (IsActive) {
			foreach (RaycastHit2D hit in Physics2D.BoxCastAll((Vector2)transform.position + new Vector2(range / 2, 0), new Vector2(range, 1.0f), 0f, fwd)) {
				Rigidbody2D rb = hit.collider.GetComponent<Rigidbody2D> ();

				if (rb) {
					rb.AddForce (fwd * force);
				}
			}
		}
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
