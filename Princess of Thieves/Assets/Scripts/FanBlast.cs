using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBlast : MonoBehaviour {
	Vector2 fwd;
	float force;
	// Use this for initialization
	void OnTriggerEnter2D(Collider2D col) {
		col.attachedRigidbody.AddForce (fwd * force);
	}

	void OnTriggerStay2D(Collider2D col) {
		col.attachedRigidbody.AddForce (fwd * force);
	}

	public Vector2 Forward {
		get {
			return fwd;
		}

		set {
			fwd = value;
		}
	}

	public float Force {
		get {
			return force;
		}

		set {
			force = value;
		}
	}
}
