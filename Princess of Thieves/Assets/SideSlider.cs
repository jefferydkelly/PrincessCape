using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideSlider : MonoBehaviour,ActivateableObject {
	SliderStatus status;
	[SerializeField]
	bool startOpen = false;
	[SerializeField]
	float travelTime = 1.0f;

	Vector3 openPos;
	Vector3 closePos;

	// Use this for initialization
	void Start () {
		if (startOpen) {
			openPos = transform.position;
			closePos = openPos - new Vector3 (gameObject.HalfWidth () * 2, 0);
			status = SliderStatus.Open;
		} else {
			closePos = transform.position;
			openPos = closePos + new Vector3 (gameObject.HalfWidth () * 2, 0);
			status = SliderStatus.Closed;
		}
	}

	public void Activate() {
		if (status == SliderStatus.Closed) {
			StartCoroutine ("Open");
		}
	}

	IEnumerator Open() {
		status = SliderStatus.Opening;

		do {
			transform.position += new Vector3(gameObject.HalfWidth() * 2.0f * Time.deltaTime / travelTime, 0);
			Debug.Log("Opening");
			yield return null;
		} while(transform.position.x < openPos.x);
		transform.position = openPos;
		status = SliderStatus.Open;
	}

	IEnumerator Close() {
		status = SliderStatus.Closing;

		do {
			transform.position -= new Vector3(gameObject.HalfWidth() * 2.0f * Time.deltaTime / travelTime, 0);
			yield return null;
		} while(transform.position.x > closePos.x);
		transform.position = closePos;
		status = SliderStatus.Closed;
	}
	public void Deactivate() {
		if (status == SliderStatus.Open) {
			StartCoroutine ("Close");
		}
	}

	public bool IsActive {
		get {
			return status == SliderStatus.Closing || status == SliderStatus.Opening;
		}
	}
}

public enum SliderStatus {
	Open,
	Opening,
	Closed,
	Closing
}
