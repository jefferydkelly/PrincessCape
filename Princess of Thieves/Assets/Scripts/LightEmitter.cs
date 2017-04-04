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
	LightBeam light;
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
			transform.Rotate (Vector3.forward, -45);
			break;
		case AimDirection.Up:
			fwd = new Vector2(0, 1);
			break;
		case AimDirection.UpLeft:
			fwd = new Vector2(-sqrtHalf, sqrtHalf);
			transform.Rotate (Vector3.forward, 45);
			break;
		case AimDirection.Left:
			fwd = new Vector2(-1, 0);
			break;
		case AimDirection.DownLeft:
			fwd = new Vector2(-sqrtHalf, -sqrtHalf);
			transform.Rotate (Vector3.forward, 45);
			break;
		case AimDirection.Down:
			fwd = new Vector2(0, -1);
			transform.Rotate (Vector3.forward, 180);
			break;
		case AimDirection.DownRight:
			fwd = new Vector2(sqrtHalf, -sqrtHalf);
			transform.Rotate (Vector3.forward, -45);
			break;
		}

		light = GetComponentInChildren<LightBeam> ();
		light.transform.localScale = new Vector3 (1, range, 1);
		light.transform.localPosition = new Vector3 (0, (range + 1) / 2f, 1);
		light.Forward = fwd;
		light.Source = transform.position;
		isActive = startActive;
		light.gameObject.SetActive (startActive);
	}

	public void Activate() {
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
}
