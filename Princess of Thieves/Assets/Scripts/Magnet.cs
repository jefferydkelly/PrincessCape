using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour, ActivateableObject {

	[SerializeField]
	bool isActive = false;
	[SerializeField]
	bool push = false;
	[SerializeField]
	Vector2 fwd = new Vector2(0, 0);
	public float range = 10;
	List<Rigidbody2D> attractedBodies;
	// Use this for initialization
	void Start () {
		attractedBodies = new List<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isActive) {
			RaycastHit2D hit = Physics2D.Raycast (transform.position, fwd, range, 1 << LayerMask.NameToLayer ("Metal"));
			if (hit) {
				Rigidbody2D rb = hit.collider.GetComponent<Rigidbody2D> ();
				if (!attractedBodies.Contains (rb)) {
					rb.constraints = RigidbodyConstraints2D.FreezeRotation;
					attractedBodies.Add (rb);
				}
			}
			foreach (Rigidbody2D r in attractedBodies) {
				r.AddForce (fwd * (push ? 15 : -25), ForceMode2D.Force);
			}
		}
	}

	public void Activate() {
		isActive = true;
	}

	public void Deactivate() {
		isActive = false;
		foreach (Rigidbody2D r in attractedBodies) {
			r.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
			r.velocity = Vector2.zero;
		}
		attractedBodies = new List<Rigidbody2D> ();
	}

	public bool IsActive {
		get {
			return isActive;
		}
	}
}
