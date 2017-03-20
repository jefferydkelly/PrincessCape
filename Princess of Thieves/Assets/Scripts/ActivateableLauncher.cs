using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateableLauncher : MonoBehaviour, ActivateableObject {

	[SerializeField]
	GameObject projectile;
	bool isActive = false;
	[SerializeField]
	bool startActive = false;
	public float timeToFire = 1f;
	[SerializeField]
	Vector3 fwd = new Vector3(1,0);
	Timer fireTimer;
	SpriteRenderer myRenderer;
	[SerializeField]
	bool isActivationInverted = false;
	bool cooled = false;
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
		}

	}

	void Update() {
		if (IsActive && myRenderer.color != Color.red) {
			myRenderer.color = Color.Lerp (Color.gray, Color.red, 1 - fireTimer.RunPercent);
		} else if (!IsActive && myRenderer.color != Color.blue) {
			myRenderer.color = Color.Lerp (Color.gray, Color.blue, 1 - fireTimer.RunPercent);
		}
	}

	void Fire()
	{
		GameObject temp = Instantiate(projectile);
		temp.transform.position = transform.position + fwd * (gameObject.HalfWidth () + temp.HalfWidth () + 0.25f);
		temp.transform.RotateAround (temp.transform.position, Vector3.forward, Mathf.Atan2 (fwd.y, fwd.x) * Mathf.Rad2Deg);
		temp.GetComponent<Rigidbody2D>().AddForce(fwd*10,ForceMode2D.Impulse);
		myRenderer.color = Color.gray;
	}

	void OnDestroy() {
		fireTimer.Stop ();
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
		//fireTimer.Stop ();
		isActive = false;
		//myRenderer.color = Color.blue;
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
