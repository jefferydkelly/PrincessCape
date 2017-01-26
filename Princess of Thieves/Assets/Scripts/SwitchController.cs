using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : ActivatorObject {
    private bool activated;
    public Sprite activatedSprite;
    public Sprite deactivatedSprite;
    private SpriteRenderer myRenderer;

    public void Start()
    {
		Initialize ();
        myRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.CompareTag("Seed") || collision.CompareTag("Projectile"))
        {
            Destroy(collision.gameObject);
            Activated = !Activated;
            if (activated)
            {
                Activate();
            }
            else
            {
                Deactivate();
            }

            
        }
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
            myRenderer.sprite = activated ? activatedSprite : deactivatedSprite;
        }
    }
}
