using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PressureSwitch : ActivatorObject {

	private int numberOfThingsWeighingThisDown = 0;
    [SerializeField]
    Sprite[] switchSprites;
    public bool triggered = false;

    public bool needsHeavy = false;

	[SerializeField]
	AudioClip activatedSoundEffect;
	// Use this for initialization
	void Start () {
		Initialize ();
        if (triggered)
        {
            GetComponent<SpriteRenderer>().sprite = switchSprites[1];
        }
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();

        if (!needsHeavy)
        {
            if (rb && rb.mass >= 1)
            {
                NumberOfThingsWeighingThisDown++;
            }
        }
        else
        {
            if(rb && rb.gravityScale >= 4)
            {
                Debug.Log(" Heavy Triggered");
                NumberOfThingsWeighingThisDown++;
            }
        }
	}

	void OnTriggerExit2D(Collider2D col)
	{
		Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
        if (!needsHeavy)
        {
            if (rb && rb.mass >= 1 && !needsHeavy)
            {
                NumberOfThingsWeighingThisDown--;
            }
        }
	}

	private int NumberOfThingsWeighingThisDown
	{
		set
		{
			if (value > 0 && numberOfThingsWeighingThisDown == 0)
			{
                //
				AudioManager.Instance.PlaySound(activatedSoundEffect);
				Activate();
                triggered = true;
                GetComponent<SpriteRenderer>().sprite = switchSprites[1];
            }
			else if (value == 0 && numberOfThingsWeighingThisDown > 0)
			{
				AudioManager.Instance.PlaySound(activatedSoundEffect);
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
