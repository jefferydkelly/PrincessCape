using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : ActivatorObject {
    private bool activated;
    Animator myAnimator;
	public AudioClip switchSound;

    public void Start()
    {
		Initialize ();
        myAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.CompareTag("Seed") || collision.CompareTag("Projectile"))
        {
            Destroy(collision.gameObject);
			Vector3 pos = Camera.main.WorldToScreenPoint (transform.position);
			if (pos.x.Between (-Screen.width / 2, Screen.width * 3 / 2)) {
				AudioManager.Instance.PlaySound (switchSound);
			}
            if (!activated)
            {
                Activate();
            }
            else
            {
                Deactivate();
            }

            
        }
    }

	protected override void Activate ()
	{
		base.Activate ();
        myAnimator.SetTrigger("Activated");
		activated = true;
	}

	protected override void Deactivate ()
	{
		base.Deactivate ();
        myAnimator.SetTrigger("Deactivated");
        activated = false;
	}
    private bool Activated
    {
        get
        {
            return activated;
        }

        set
        {
            activated = value;
        }
    }
}
