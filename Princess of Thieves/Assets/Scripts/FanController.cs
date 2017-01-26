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
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (IsActive) {
			foreach (RaycastHit2D hit in Physics2D.RaycastAll(transform.position, fwd, range)) {
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
