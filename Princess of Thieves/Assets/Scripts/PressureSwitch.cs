using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PressureSwitch : ActivatorObject {

    private int numberOfThingsWeighingThisDown = 0;
    public bool triggered = false;

	[SerializeField]
	AudioClip activatedSoundEffect;
	// Use this for initialization
	Animator anim;
	void Start () {
		Initialize ();
		anim = GetComponent<Animator> ();
	}


	void OnTriggerEnter2D(Collider2D col)
	{
		Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();

        if (rb && rb.mass >= 1 && !col.isTrigger)
        {
            NumberOfThingsWeighingThisDown++;
        }
	}

	void OnTriggerExit2D(Collider2D col)
	{
		Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
        if (rb && rb.mass >= 1 && !col.isTrigger)
        {
            NumberOfThingsWeighingThisDown--;
        }
	}

	private int NumberOfThingsWeighingThisDown
	{
		set
		{
			

			if (value <= 0 && numberOfThingsWeighingThisDown > 0) {
                GetComponent<AudioSource>().clip = activatedSoundEffect;
                GetComponent<AudioSource>().Play();

                Deactivate ();
			} else if (numberOfThingsWeighingThisDown == 0 && value > 0) {
                GetComponent<AudioSource>().clip = activatedSoundEffect;
                GetComponent<AudioSource>().Play();
                Activate ();
			}
            numberOfThingsWeighingThisDown = Mathf.Max(value, 0);
            anim.SetInteger("ItemsOnTop", numberOfThingsWeighingThisDown);
        }

		get
		{
			return numberOfThingsWeighingThisDown;
		}
	}

    public override void Reset()
    {
        base.Deactivate();
		NumberOfThingsWeighingThisDown = 0;
    }
}
