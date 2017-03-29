using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanController : MonoBehaviour, ActivateableObject {

	[SerializeField]
	AimDirection direction;
	Vector2 fwd = new Vector2(1, 0);
	[SerializeField]
	float force = 10;
	[SerializeField]
	float range = 10;
	[SerializeField]
	bool isActive = false;
	bool isPushingPlayer = false;
	[SerializeField]
	bool isActivationInverted = false;
	LineRenderer lineRenderer;
	FanBlast blast;
	// Use this for initialization
	void Start () {
		float sqrtHalf = 1 / Mathf.Sqrt (2);
		switch (direction)
		{
		case AimDirection.Right:
			fwd = new Vector2(1, 0);
			break;
		case AimDirection.UpRight:
			fwd = new Vector2 (sqrtHalf, sqrtHalf);
			transform.Rotate (Vector3.fwd, -45);
			break;
		case AimDirection.Up:
			fwd = new Vector2(0, 1);
			break;
		case AimDirection.UpLeft:
			fwd = new Vector2(-sqrtHalf, sqrtHalf);
			transform.Rotate (Vector3.fwd, 45);
			break;
		case AimDirection.Left:
			fwd = new Vector2(-1, 0);
			break;
		case AimDirection.DownLeft:
			fwd = new Vector2(-sqrtHalf, -sqrtHalf);
			transform.Rotate (Vector3.fwd, 45);
			break;
		case AimDirection.Down:
			fwd = new Vector2(0, -1);
			transform.Rotate (Vector3.fwd, 90);
			break;
		case AimDirection.DownRight:
			fwd = new Vector2(sqrtHalf, -sqrtHalf);
			transform.Rotate (Vector3.fwd, -45);
			break;
		}
		blast = GetComponentInChildren<FanBlast> ();
		blast.transform.localScale = new Vector3 (1, range, 1);
		blast.transform.localPosition = new Vector3 (0, (range + 1) / 2f, 0);
		blast.Forward = fwd;
		blast.Force = force;

		blast.gameObject.SetActive (isActive);
	}

	public void Activate() {
		isActive = true;
		blast.gameObject.SetActive (true);
	}

	public void Deactivate() {
		isActive = false;
		blast.gameObject.SetActive (false);
		lineRenderer.enabled = false;
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
