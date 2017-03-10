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
	Animator anim;
	void Start () {
		Initialize ();
		anim = GetComponent<Animator> ();
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
			anim.SetInteger ("ItemsOnTop", value);

			if (value == 0 && numberOfThingsWeighingThisDown > 0) {
				Deactivate ();
			} else if (numberOfThingsWeighingThisDown == 0 && value > 0) {
				Activate ();
			}
			numberOfThingsWeighingThisDown = Mathf.Max(value, 0);
		}

		get
		{
			return numberOfThingsWeighingThisDown;
		}
	}
}
