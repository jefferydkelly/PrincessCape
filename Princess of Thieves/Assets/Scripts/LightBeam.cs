using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBeam : MonoBehaviour {
	Vector2 fwd;
	float closestDistance = Mathf.Infinity;
	GameObject closest;
	LightBeam myChild = null;
	Vector2 reflectDirection = Vector2.zero;
	// Use this for initialization
	void Start () {
		
	}

	void OnTriggerEnter2D(Collider2D col) {
		ReflectiveObject ro = col.GetComponent<ReflectiveObject> ();
		if (ro != null && !(col.OnLayer("Player") && !GameManager.Instance.Player.IsUsingReflectCape)) {
			Vector2 dif = col.transform.position - transform.position;
			float dot = fwd.Dot (dif);

			if (dot < closestDistance) {
				closest = col.gameObject;
				closestDistance = dot;
				RemoveChildren ();
				myChild = Instantiate (gameObject).GetComponent<LightBeam>();
				reflectDirection = ro.SurfaceForward;

				float rot = (reflectDirection.GetAngle() - fwd.GetAngle()) * 2;
				myChild.transform.position = col.transform.position + new Vector3 (Mathf.Cos (fwd.GetAngle() - rot), Mathf.Sin (fwd.GetAngle() - rot)) * 10.5f;
				myChild.transform.Rotate (Vector3.forward, rot.ToDegrees());
			}

		}
	}

	void OnTriggerStay2D(Collider2D col) {
		ReflectiveObject ro = col.GetComponent<ReflectiveObject> ();
		if (ro != null) {
			if (closest != null && col.CompareTag("Player") && closest.CompareTag("Player")) {
				if (GameManager.Instance.Player.IsUsingReflectCape && ro.SurfaceForward != reflectDirection) {
					RemoveChildren ();
					myChild = Instantiate (gameObject).GetComponent<LightBeam> ();
					reflectDirection = ro.SurfaceForward;

					float rot = fwd.GetAngle () + (reflectDirection.GetAngle () - fwd.GetAngle ()) * 2;
					myChild.transform.position = col.transform.position + new Vector3 (Mathf.Cos (rot), Mathf.Sin (rot)) * 11;
					myChild.transform.Rotate (Vector3.forward, rot.ToDegrees ());
					Debug.Break ();
				} else if (!GameManager.Instance.Player.IsUsingReflectCape) {
					RemoveChildren ();
					closestDistance = Mathf.Infinity;
					closest = null;
				}

			} else if (!(col.OnLayer ("Player") && !GameManager.Instance.Player.IsUsingReflectCape)) {
				Vector2 dif = col.transform.position - transform.position;
				float dot = fwd.Dot (dif);

				if (dot < closestDistance && dot > 0) {
					closest = col.gameObject;
					closestDistance = dot;
					RemoveChildren ();
					myChild = Instantiate (gameObject).GetComponent<LightBeam> ();
					reflectDirection = ro.SurfaceForward;

					float rot = fwd.GetAngle () + (reflectDirection.GetAngle () - fwd.GetAngle ()) * 2;
					myChild.transform.position = col.transform.position + new Vector3 (Mathf.Cos (rot), Mathf.Sin (rot)) * 11;
					myChild.transform.Rotate (Vector3.forward, rot.ToDegrees ());
				}

			}
		}
	}

	void OnTriggerExit2D(Collider2D col) {
		if (col.gameObject.Equals (closest)) {
			RemoveChildren ();
			closestDistance = Mathf.Infinity;
			closest = null;
		}
	}


	void RemoveChildren() {
		if (myChild != null) {
			myChild.RemoveChildren ();
			Destroy (myChild.gameObject);
		}
	}

	public Vector2 Forward {
		get {
			return fwd;
		}

		set {
			fwd = value;
		}
	}
}
