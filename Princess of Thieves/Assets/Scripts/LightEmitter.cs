using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEmitter : MonoBehaviour, ActivateableObject {

	[SerializeField]
	float range;
	[SerializeField]
	AimDirection direction;
	Vector2 fwd;
	[SerializeField]
	bool startActive = false;
	bool isActive = false;
	[SerializeField]
	bool isActivationInverted = false;
	LightBeam myLight;

    Animator myAnimator;
	// Use this for initialization
	void Start () {
        myAnimator = GetComponent<Animator>();
        
        if (startActive)
        {
            myAnimator.SetTrigger("Activated");
        } else
        {
            myAnimator.SetTrigger("Deactivated");
        }
        float sqrtHalf = 1 / Mathf.Sqrt (2);
    
		switch (direction)
		{
		case AimDirection.Right:
			fwd = new Vector2(1, 0);
			break;
		case AimDirection.UpRight:
			fwd = new Vector2 (sqrtHalf, sqrtHalf);
			transform.Rotate (Vector3.forward, 45);
			break;
		case AimDirection.Up:
			fwd = new Vector2(0, 1);
            transform.Rotate(Vector3.forward, 90);
			break;
		case AimDirection.UpLeft:
			fwd = new Vector2(-sqrtHalf, sqrtHalf);
			transform.Rotate (Vector3.forward, 135);
			break;
		case AimDirection.Left:
			fwd = new Vector2(-1, 0);
                transform.Rotate(Vector3.forward, 180);
			break;
		case AimDirection.DownLeft:
			fwd = new Vector2(-sqrtHalf, -sqrtHalf);
			transform.Rotate (Vector3.forward, 225);
			break;
		case AimDirection.Down:
			fwd = new Vector2(0, -1);
			transform.Rotate (Vector3.forward, 270);
			break;
		case AimDirection.DownRight:
			fwd = new Vector2(sqrtHalf, -sqrtHalf);
			transform.Rotate (Vector3.forward, 315);
			break;
		}

		myLight = GetComponentInChildren<LightBeam> ();
		myLight.transform.localScale = new Vector3 (range, 1, 1);
		myLight.transform.localPosition = new Vector3 ((range + 1) / 2f, 0, 1);
		myLight.Forward = fwd;
		myLight.Source = transform.position;
		isActive = startActive;
		myLight.gameObject.SetActive (startActive);
        myLight.maxRange = range;
	}

	public void Activate() {
		isActive = true;
        myLight.gameObject.SetActive(true);
        myLight.Reset();
        myAnimator.SetTrigger("Activated");

    }

	public void Deactivate() {
		isActive = false;
        myLight.RemoveChildren();
        myLight.gameObject.SetActive(false);
        myAnimator.SetTrigger("Deactivated");
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

	public bool IsInverted {
		get {
			return isActivationInverted;
		}
	}
}
