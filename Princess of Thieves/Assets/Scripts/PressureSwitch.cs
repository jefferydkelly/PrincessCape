using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PressureSwitch : ActivatorObject {

	private int numberOfThingsWeighingThisDown = 0;

    public bool triggered = false;
	// Use this for initialization
	void Start () {
		activators = new List<ActivateableObject>();
		foreach (GameObject go in activatedObjects)
		{
			ActivateableObject ao = go.GetComponent<ActivateableObject>();

			if (ao != null)
			{
				activators.Add(ao);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();

		if (rb && rb.mass >= 1)
		{
			NumberOfThingsWeighingThisDown++;
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();

		if (rb && rb.mass >= 1)
		{
			NumberOfThingsWeighingThisDown--;
		}
	}

	private int NumberOfThingsWeighingThisDown
	{
		set
		{
			if (value > 0 && numberOfThingsWeighingThisDown == 0)
			{
				Activate();
                triggered = true;
			}
			else if (value == 0 && numberOfThingsWeighingThisDown > 0)
			{
				Deactivate();
                triggered = false;
			}
			numberOfThingsWeighingThisDown = value;
		}

		get
		{
			return numberOfThingsWeighingThisDown;
		}
	}
}
