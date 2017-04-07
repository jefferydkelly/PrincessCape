using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBeam : MonoBehaviour {
    [SerializeField]
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
        scale = new Vector3(transform.localScale.x, 1, 1);
    }
    private void Update()
    {
        if (closest && reflectedOff.IsReflecting)
        {
            if (fwd.Dot(reflectedOff.SurfaceForward) < Mathf.Cos(Mathf.PI / 4))
            {

                if (!reflectedOff.SurfaceForward.VectorsEqual(reflectDirection))
                {
                    Reflect(reflectedOff);
                }
            } else
            {
                RemoveChildren();
            }
        } else
        {
            RemoveChildren();
        }
    }
    void OnTriggerEnter2D(Collider2D col) {
		ReflectiveObject ro = col.GetComponent<ReflectiveObject> ();
       
		if (ro != null && ro.IsReflecting) {
            if (fwd.Dot(ro.SurfaceForward) < Mathf.Cos(Mathf.PI / 4))
            {
                Vector2 dif = col.transform.position - source;
                float dot = fwd.Dot(dif);
                if (dot > 0 && dot < closestDistance)
                {
                    closest = col.gameObject;
                    reflectedOff = closest.GetComponent<ReflectiveObject>();
                    closestDistance = dot;
                    Resize();
                }
            }

		} else if (col.OnLayer("Platforms") || col.CompareTag("Block"))
        {
            Vector2 dif = col.transform.position - source;
            float dot = fwd.Dot(dif);
            
            if (dot > 0 && dot < closestDistance)
            {
                closest = null;
                reflectedOff = null;
                closestDistance = dot;
                Resize();
            }
        }
	}

    void OnTriggerStay2D(Collider2D col) {
        ReflectiveObject ro = col.GetComponent<ReflectiveObject> ();
	
		if (ro != null && ro.IsReflecting) {
			if (closest != col.gameObject) {
                if (fwd.Dot(ro.SurfaceForward) < Mathf.Cos(Mathf.PI / 4)) { 
                    Vector2 dif = col.transform.position - source;
				    float dot = fwd.Dot (dif);

                    if (dot > 0 && dot < closestDistance)
                    {
                        closest = col.gameObject;
                        reflectedOff = closest.GetComponent<ReflectiveObject>();
                        closestDistance = dot;
                        Resize();
                    }
                }
			}
		}
        else if (col.OnLayer("Platforms") || col.CompareTag("Block"))
        {
            Vector2 dif = col.transform.position - source;
            float dot = fwd.Dot(dif);
            if (dot > 0 && dot < closestDistance)
            {
                closest = null;
                reflectedOff = null;
                closestDistance = dot;
                Resize();
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
        
        float rot = (ro.SurfaceForward.GetAngle() - (-fwd).GetAngle());
        if (ro.IsReflecting) {
            RemoveChildren ();
			myChild = Instantiate (gameObject).GetComponent<LightBeam> ();
            myChild.maxRange = maxRange;
            
			reflectDirection = ro.SurfaceForward;
            myChild.Source = ro.GameObject.transform.position;
            
            myChild.transform.parent = transform.parent;
      
            myChild.Forward = ro.SurfaceForward.Rotated(rot);
            
            myChild.transform.Rotate(Vector3.forward,  myChild.Forward.GetAngle().ToDegrees());

            
            myChild.scale = scale;
            myChild.closestDistance = maxRange;
            RaycastHit2D hit = Physics2D.BoxCast(myChild.source, Vector2.one, 0, myChild.fwd, maxRange, 1 << LayerMask.NameToLayer("Platforms") | 1 << LayerMask.NameToLayer("Reflective") | 1 << LayerMask.NameToLayer("Interactive"));
            if (hit && !hit.collider.gameObject.name.Contains("Sign") && hit.collider.gameObject != closest)
            {
                myChild.scale.x = hit.distance;
                myChild.closestDistance = hit.distance;
            }

            
            myChild.Resize();
        } else {
			RemoveChildren ();
			closestDistance = Mathf.Infinity;
			closest = null;
			reflectDirection = Vector2.zero;
		}
	}

	public void RemoveChildren() {
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
        scale.x = closestDistance;
        transform.localScale = scale;
        
        Vector3 pos = source + (Vector3)(fwd * (scale.x + 1) / 2);
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
