using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateableLauncher : MonoBehaviour, ActivateableObject {

	[SerializeField]
	GameObject projectile;
	bool isActive = false;
	[SerializeField]
	bool startActive = false;
	[SerializeField]
	float launchForce = 10f;
	public float timeToFire = 1f;
	[SerializeField]
	AimDirection direction;
	Vector2 fwd = new Vector2(1,0);
	Timer fireTimer;
	SpriteRenderer myRenderer;
	[SerializeField]
	bool isActivationInverted = false;
	bool cooled = false;
    float angle = 0;
	// Use this for initialization
	void Start () {
		myRenderer = GetComponent<SpriteRenderer> ();
		WaitDelegate wd;
		wd = () => {
			if (IsActive) {
				Fire();
			} else {
				cooled = true;
				fireTimer.Paused = true;
				myRenderer.color = Color.blue;
			}
		};

		fireTimer = new Timer (wd, timeToFire, true);

		if (startActive) {
			Activate ();
		} else {
			Deactivate ();
			myRenderer.color = Color.blue;
		}

		float sqrtHalf = 1.0f / Mathf.Sqrt(2);
		switch (direction)
		{
		case AimDirection.Right:
			fwd = new Vector2(1, 0);
			myRenderer.flipX = true;
			break;
		case AimDirection.Up:
			fwd = new Vector2(0, 1);
			transform.Rotate (Vector3.forward, -90);
			break;
		case AimDirection.Left:
			fwd = new Vector2(-1, 0);
			break;
		case AimDirection.Down:
			fwd = new Vector2(0, -1);
			transform.Rotate (Vector3.forward, 90);
			break;
		}
        angle = fwd.GetAngle().ToDegrees();
	}

	void Update() {
		float time = 1 - fireTimer.RunPercent;
		if (IsActive && myRenderer.color != Color.red) {
			myRenderer.color = Color.Lerp (Color.gray, Color.red, time * time);
		} else if (!IsActive && myRenderer.color != Color.blue) {
			myRenderer.color = Color.Lerp (Color.gray, Color.blue, time * time);
		}
	}

	void Fire()
	{
		GameObject temp = Instantiate(projectile);
        Vector3 pos = transform.position;
        pos.z += 1;
        temp.transform.position = pos;// transform.position;// + fwd * (gameObject.HalfWidth () + temp.HalfWidth () + 0.25f);
		temp.transform.RotateAround (temp.transform.position, Vector3.forward, Mathf.Atan2 (fwd.y, fwd.x) * Mathf.Rad2Deg);
		temp.GetComponent<Rigidbody2D>().AddForce(fwd*launchForce,ForceMode2D.Impulse);
		myRenderer.color = Color.gray;
	}

	void OnDestroy() {
        if (fireTimer != null)
        {
            fireTimer.Stop();
        }
	}
	public void Activate() {
		if (!fireTimer.Activated || cooled) {
			myRenderer.color = Color.red;
			cooled = false;
			Fire ();
			fireTimer.Reset ();
			fireTimer.Start ();
		}
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

    public float Angle
    {
        get
        {
            return angle;
        }
    }
}
