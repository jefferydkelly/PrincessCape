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
	// Use this for initialization
	void Start () {
		myRenderer = GetComponent<SpriteRenderer> ();
		WaitDelegate wd;
		wd = () => {
			Fire();
		};

		fireTimer = new Timer (wd, timeToFire, true);

		if (startActive) {
			Activate ();
		}

	}

	void OnBecameVisible() {
		if (isActive) {
			if (fireTimer.Paused) {
				fireTimer.Paused = false;
			} else {
				fireTimer.Start ();
			}
		}
	}

	void OnBecameInvisible() {
		
		if (isActive) {
			fireTimer.Paused = true;
		}
	}
	void Fire()
	{
		GameObject temp = Instantiate(projectile);
		temp.transform.position = transform.position + fwd * (gameObject.HalfWidth () + temp.HalfWidth () + 0.25f);
		temp.GetComponent<Rigidbody2D>().AddForce(fwd*10,ForceMode2D.Impulse);
	}

	public void Activate() {
		fireTimer.Reset ();
		fireTimer.Start ();
		isActive = true;
		myRenderer.color = Color.red;
	}

	public void Deactivate() {
		fireTimer.Stop ();
		isActive = false;
		myRenderer.color = Color.white;
	}

	public bool IsActive {
		get {
			return isActive;
		}
	}
}
