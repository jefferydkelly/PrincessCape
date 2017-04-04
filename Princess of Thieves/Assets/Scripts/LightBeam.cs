using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBeam : MonoBehaviour {
	Vector2 fwd;
	float closestDistance = Mathf.Infinity;
	GameObject closest;
    ReflectiveObject reflectedOff;
	LightBeam myChild = null;
	Vector3 source;
	Vector2 reflectDirection = Vector2.zero;
	float fourtyFiveCos = Mathf.Cos (Mathf.PI / 4);
	float onethirtyfiveCos = Mathf.Cos(3 * Mathf.PI / 4);

    private void Update()
    {
        if (closest && reflectedOff.IsReflecting)
        {
            float angDif = Mathf.Abs(reflectedOff.SurfaceForward.GetAngle() - fwd.GetAngle()).ToDegrees();
        
            if (!reflectedOff.SurfaceForward.VectorsEqual(reflectDirection)) {
                Reflect(reflectedOff);
            }
        } else
        {
            RemoveChildren();
        }
    }
    void OnTriggerEnter2D(Collider2D col) {
		ReflectiveObject ro = col.GetComponent<ReflectiveObject> ();
		if (ro != null && ro.IsReflecting) {
			Vector2 dif = col.transform.position - source;
			float dot = fwd.Dot (dif);
			if (dot < closestDistance) {
				closest = col.gameObject;
                reflectedOff = closest.GetComponent<ReflectiveObject>();
                closestDistance = dot;
			}

		}
	}


	void OnTriggerStay2D(Collider2D col) {
		ReflectiveObject ro = col.GetComponent<ReflectiveObject> ();
	
		if (ro != null) {
			if (closest != col.gameObject) {
				Vector2 dif = col.transform.position - source;
				float dot = fwd.Dot (dif);

				if (dot < closestDistance) {
					closest = col.gameObject;
                    reflectedOff = closest.GetComponent<ReflectiveObject>();
                    closestDistance = dot;
                }
			}
		}
	}

	void OnTriggerExit2D(Collider2D col) {
		if (col.gameObject == closest) {
			RemoveChildren ();
			closestDistance = Mathf.Infinity;
			closest = null;
			reflectDirection = Vector2.zero;
		}
	}

	void Reflect(ReflectiveObject ro) {
		if (ro.IsReflecting) {
			RemoveChildren ();
			myChild = Instantiate (gameObject).GetComponent<LightBeam> ();
			reflectDirection = ro.SurfaceForward;

			float rot = (reflectDirection.GetAngle () - fwd.GetAngle ());
			rot *= 2;
			myChild.transform.parent = transform.parent;
			myChild.transform.Rotate (Vector3.forward, rot.ToDegrees ());
			rot -= fwd.GetAngle ();
			myChild.Forward = new Vector3 (Mathf.Cos (rot), Mathf.Sin (rot));

			myChild.Source = ro.GameObject.transform.position;
		
			Vector3 cPos = ro.GameObject.transform.position + (Vector3)myChild.Forward * 11f;
			cPos.z = 1;
			myChild.transform.position = cPos;
		} else {
			RemoveChildren ();
			closestDistance = Mathf.Infinity;
			closest = null;
			reflectDirection = Vector2.zero;
		}
	}

	void RemoveChildren() {
		if (myChild != null) {
			myChild.RemoveChildren ();
			Destroy (myChild.gameObject);
			myChild = null;
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

	public Vector3 Source {
		get {
			return source;
		}

		set {
			source = value;
		}
	}
}
