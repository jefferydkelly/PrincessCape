using UnityEngine;
using System.Collections;

public class RopeController : MonoBehaviour, BurnableObject {

	public GameObject suspendedObject;
	Rigidbody2D suspendedBody;
	void Start()
	{
		if (suspendedObject != null)
		{
			suspendedBody = suspendedObject.GetComponent<Rigidbody2D>();

			if (suspendedBody != null)
			{
				suspendedBody.gravityScale = 0;
			}
			suspendedObject.transform.position = transform.position - new Vector3(0, gameObject.HalfHeight() + suspendedObject.HalfHeight());
		}
	}

	public void Burn()
	{
		if (suspendedBody != null)
		{
			suspendedBody.gravityScale = 1;
		}
		Destroy(gameObject);
	}

	public void Douse()
	{
	}


}
