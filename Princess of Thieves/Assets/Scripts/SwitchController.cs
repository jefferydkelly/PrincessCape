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
        myRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Seed"))
        {
            if (activated)
            {
                Deactivate();
            } else
            {
                Activate();
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
