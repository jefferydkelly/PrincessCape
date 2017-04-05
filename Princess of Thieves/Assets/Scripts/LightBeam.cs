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
    public float maxRange = 0;
    Vector3 scale;

    private void Start()
    {
        scale = new Vector3(1, transform.localScale.y, 1);
    }
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
			if (dot > 0 && dot < closestDistance) {
				closest = col.gameObject;
                reflectedOff = closest.GetComponent<ReflectiveObject>();
                closestDistance = dot;
                Resize();
            }

		}
	}


	void OnTriggerStay2D(Collider2D col) {
		ReflectiveObject ro = col.GetComponent<ReflectiveObject> ();
	
		if (ro != null && ro.IsReflecting) {
			if (closest != col.gameObject) {
				Vector2 dif = col.transform.position - source;
				float dot = fwd.Dot (dif);

				if (dot > 0 && dot < closestDistance) {
					closest = col.gameObject;
                    reflectedOff = closest.GetComponent<ReflectiveObject>();
                    closestDistance = dot;
                    Resize();
                }
			}
		}
	}

	void OnTriggerExit2D(Collider2D col) {
		if (col.gameObject == closest) {
			RemoveChildren ();
			closestDistance = maxRange;
			closest = null;
			reflectDirection = Vector2.zero;
            Resize();
        }
	}

	void Reflect(ReflectiveObject ro) {
        float rot = (ro.SurfaceForward.GetAngle() - fwd.GetAngle());
        if (ro.IsReflecting) {
			RemoveChildren ();
			myChild = Instantiate (gameObject).GetComponent<LightBeam> ();
            myChild.maxRange = maxRange;
            
			reflectDirection = ro.SurfaceForward;

			
			rot *= 2;
			myChild.transform.parent = transform.parent;
			myChild.transform.Rotate (Vector3.forward, rot.ToDegrees ());
			rot -= fwd.GetAngle ();
			myChild.Forward = new Vector3 (Mathf.Cos (rot), Mathf.Sin (rot));

            myChild.Source = ro.GameObject.transform.position;

            myChild.scale = scale;
            myChild.closestDistance = maxRange;
            myChild.Resize();
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
            closest = null;
            closestDistance = maxRange;
            Resize();
        }
	}

    void Resize()
    {
        scale.y = closestDistance;
        transform.localScale = scale;
        Vector3 pos = source + (Vector3)(fwd * (scale.y + 1) / 2);
        pos.z = 1;
        transform.position = pos;
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
