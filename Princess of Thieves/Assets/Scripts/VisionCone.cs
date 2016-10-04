using UnityEngine;
using System.Collections;

public class VisionCone : MonoBehaviour {
	private AlertableObject myParent;

	void Start()
	{
		myParent = GetComponentInParent<AlertableObject>();
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Player"))
		{
			myParent.Alert();
		}
	}

	public float Rotation
	{
		get
		{
			return transform.localRotation.z;
		}

		set
		{
			transform.localRotation = Quaternion.AngleAxis(value, Vector3.forward);
		}
	}
}
