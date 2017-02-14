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
			RaycastHit2D[] hits = Physics2D.BoxCastAll (transform.position, new Vector2 (gameObject.HalfWidth () * 2, range), 0, fwd.normalized, range, 1 << LayerMask.NameToLayer ("Metal"));
			foreach (RaycastHit2D hit in hits) {
				Rigidbody2D rb = hit.collider.GetComponent<Rigidbody2D> ();

				if (rb) {
					attractedBodies.AddExclusive (rb);
					rb.AddForce (fwd * (push ? 10 : -10));
				}
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
			//r.velocity = Vector2.zero;
		}
		attractedBodies.Clear ();
	}

	public bool IsActive {
		get {
			return isActive;
		}
	}

	public float ActivationTime {
		get {
			return 0;
		}
	}
}
