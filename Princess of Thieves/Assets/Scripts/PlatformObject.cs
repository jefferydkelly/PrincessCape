using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class PlatformObject: MonoBehaviour {
	public bool passThrough = false;
	Collider2D myCollider = null;

	void Awake()
	{
		myCollider = GetComponent<Collider2D>();
	}

	public void AllowPassThrough()
	{
		myCollider.isTrigger = true;
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (passThrough && col.CompareTag("Player"))
		{
			myCollider.isTrigger = false;
		}
	}


}
