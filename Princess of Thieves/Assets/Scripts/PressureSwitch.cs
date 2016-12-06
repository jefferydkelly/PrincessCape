using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PressureSwitch : ActivatorObject {

	private int numberOfThingsWeighingThisDown = 0;
    [SerializeField]
    Sprite[] switchSprites;
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
        if (triggered)
        {
            GetComponent<SpriteRenderer>().sprite = switchSprites[1];
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
                Debug.Log("Triggered");
				Activate();
                triggered = true;
                GetComponent<SpriteRenderer>().sprite = switchSprites[1];
            }
			else if (value == 0 && numberOfThingsWeighingThisDown > 0)
			{
				Deactivate();
                triggered = false;
                GetComponent<SpriteRenderer>().sprite = switchSprites[0];
            }
			numberOfThingsWeighingThisDown = value;
		}

		get
		{
			return numberOfThingsWeighingThisDown;
		}
	}
}
