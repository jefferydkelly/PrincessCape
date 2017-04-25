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
    bool playerWasReflectingBefore = false;

    //ideally quicker
    const float mathPi = Mathf.PI / 4;
    LayerMask layerM = 1 << LayerMask.NameToLayer("Platforms") | 1 << LayerMask.NameToLayer("Reflective") | 1 << LayerMask.NameToLayer("Player");
    private void Start()
    {
        scale = new Vector3(transform.localScale.x, 1, 1);
        closest = null;
    }
    private void Update()
    {
        if (closest && reflectedOff != null && reflectedOff.IsReflecting)
        {
            if (fwd.Dot(reflectedOff.SurfaceForward) < Mathf.Cos(mathPi))
            {
                float dot = Mathf.Round(fwd.Dot(closest.transform.position - source) * 100) / 100;
                //Vector2 result = reflectedOff.SurfaceForward ^ reflectDirection;
                if (reflectedOff.SurfaceForward != reflectDirection || (Mathf.Abs(dot - closestDistance) > 0.1f && dot < maxRange))
                {
                    closestDistance = dot;
                    Reflect(reflectedOff);
                }
            }
        } else if (myChild)
        {   
            RemoveChildren();
            Resize();
        }
    }
    void OnTriggerEnter2D(Collider2D col) {
        if (!col.OnLayer("Light"))
        {
           
            ReflectiveObject ro = col.GetComponent<ReflectiveObject>();
            Vector2 dif = col.transform.position - source;

            float dot = fwd.Dot(dif);
          
            if (dot > 0.01 && dot < closestDistance)
            {
                if (col.CompareTag("Player"))
                {
                    playerWasReflectingBefore = ro.IsReflecting;
                }
                if (ro != null && ro.IsReflecting)
                {

                    if (fwd.Dot(ro.SurfaceForward) <= Mathf.Cos(mathPi))
                    { 
                        closest = col.gameObject;
                        reflectedOff = closest.GetComponent<ReflectiveObject>();
                        closestDistance = Mathf.Round(dot * 100) / 100;
                        Resize();
                        Reflect(reflectedOff);
                    }

                }
                else if (col.OnLayer("Platforms") || col.CompareTag("Block"))
                {
                    closest = col.gameObject;
                    reflectedOff = null;
                    closestDistance = Mathf.Round(dot * 100) / 100;
                    Resize();
                }
            }
        }
	}

    void OnTriggerStay2D(Collider2D col) {

        if (!col.OnLayer("Light"))
        {
            Vector2 dif = col.transform.position - source;
            float dot = fwd.Dot(dif);

            if (dot > 0.01 && dot < closestDistance)
            {
                ReflectiveObject ro = col.GetComponent<ReflectiveObject>();

                if (ro != null && ro.IsReflecting)
                {
                    
                    if (closest != col.gameObject || (col.CompareTag("Player") && !playerWasReflectingBefore))
                    {
                        if (fwd.normalized.Dot(ro.SurfaceForward) <= Mathf.Cos(mathPi))
                        {
                            closest = col.gameObject;
                            reflectedOff = ro;
                            closestDistance = Mathf.Round(dot * 100) / 100;
                            Resize();
                            Reflect(reflectedOff);
                        }
                    }


                }

                if (col.CompareTag("Player"))
                {
                    playerWasReflectingBefore = ro.IsReflecting;
                }
                else if (col.OnLayer("Platforms") || col.CompareTag("Block"))
                {
                    closest = col.gameObject;
                    reflectedOff = null;
                    closestDistance = Mathf.Round(dot * 100) / 100;
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

            if (col.CompareTag("Player"))
            {
                playerWasReflectingBefore = false;
            }
        }
	}

	void Reflect(ReflectiveObject ro) {
      
        if (ro != null && ro.IsReflecting) {
            RemoveChildren ();
			myChild = Instantiate (gameObject).GetComponent<LightBeam> ();
            myChild.maxRange = maxRange;
            
			reflectDirection = ro.SurfaceForward;
            myChild.Source = closest.transform.position;
            
            myChild.transform.parent = transform.parent;

            if (ro.GameObject.CompareTag("Player"))
            {
                myChild.Forward = ro.SurfaceForward;
                myChild.transform.rotation = Quaternion.AngleAxis(ro.SurfaceForward.GetAngle().ToDegrees(), Vector3.forward);
            }
            else
            {
                float dif = ro.SurfaceForward.GetAngle() - fwd.GetAngle();
                dif = (dif.ToDegrees() % 90).ToRadians();
                Vector2 temp = fwd.Rotated(dif * 2);
                myChild.Forward = temp;
                myChild.transform.rotation = Quaternion.AngleAxis(temp.GetAngle().ToDegrees(), Vector3.forward);
            }
            
            myChild.scale = scale;
            myChild.closestDistance = maxRange;
            myChild.Resize();
        } else {
			RemoveChildren ();
			closestDistance = maxRange;
			closest = null;
			reflectDirection = Vector2.zero;
		}
	}

	public void RemoveChildren() {
        
        //closest = null;
        //closestDistance = maxRange;
        //reflectDirection = Vector2.zero;
		if (myChild != null) {
			myChild.RemoveChildren ();
			Destroy (myChild.gameObject);
			myChild = null;
            //closest = null;
            //closestDistance = maxRange;
            Resize();
        }
	}

    public void Reset()
    {
        closestDistance = maxRange;
        Resize();
    }
    void Resize()
    {
       
        RaycastHit2D hit = Physics2D.BoxCast(source, Vector2.one, 0, fwd, maxRange, layerM);
        if (hit)
        {
            closest = hit.collider.gameObject;
            closestDistance = Mathf.Round((source - closest.transform.position).magnitude * 100) / 100;
            ReflectiveObject ro = closest.GetComponent<ReflectiveObject>();

            if (ro != null && ro.IsReflecting)
            {
                reflectDirection = ro.SurfaceForward;
                reflectedOff = ro;
            } else
            {
                reflectedOff = null;
            }

        } else
        {
            closest = null;
            reflectedOff = null;
            closestDistance = maxRange;
        }
        scale.x = closestDistance;
        transform.localScale = scale;
        
        Vector3 pos = source + (Vector3)(fwd * scale.x / 2);
        pos.z = 1;
        transform.position = pos;
    }

	public Vector2 Forward {
		get {
			return fwd;
		}

		set {
			fwd = value;

            if (fwd.x > .99 && fwd.x < 1)
            {
                fwd.x = 1;
            }
            else if (fwd.x < -0.99 && fwd.x > -1)
            {
                fwd.x = -1;
            }
            else if ((fwd.x < 0.01 && fwd.x > 0) || (fwd.x > 0.01 && fwd.x < 0))
            {
                fwd.x = 0;
            }

            if (fwd.y > .99 && fwd.y < 1)
            {
                fwd.y = 1;
            }
            else if (fwd.y < -0.99 && fwd.y > -1)
            {
                fwd.y = -1;
            }
            else if ((fwd.y < 0.01 && fwd.y > 0) || (fwd.y > 0.01 && fwd.y < 0))
            {
                fwd.y = 0;
            }
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
