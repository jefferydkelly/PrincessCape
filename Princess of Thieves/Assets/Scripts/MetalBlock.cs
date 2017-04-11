using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalBlock : ResettableObject {

    SpriteRenderer myRenderer = null;
    Rigidbody2D myRigidbody = null;
	// Use this for initialization
	void Start () {
        startPosition = transform.position;
        myRenderer = GetComponent<SpriteRenderer>();
        myRigidbody = GetComponent<Rigidbody2D>();
	}

    private void OnMouseEnter()
    {
        if (GameManager.Instance.Player.CanInteract && GloveItem.InRange(gameObject))
        {
            Highlight();
        }
    }

    private void OnMouseOver()
    {
        if (GameManager.Instance.Player.CanInteract)
        {
            if (!GloveItem.InRange(gameObject))
            {
                Dehighlight();
            } else if (myRenderer.color == Color.white)
            {
                Highlight();
            }
        }
    }

    void Highlight()
    {
        CursorManager.Instance.State = CursorState.Metal;
        myRenderer.color = Color.blue;
        GloveItem.Highlighted = this;
    }

    void Dehighlight()
    {
        if (myRenderer.color == Color.blue)
        {
            CursorManager.Instance.State = CursorState.Normal;
            myRenderer.color = Color.white;
            GloveItem.Highlighted = null;
        }
    }
    private void OnMouseExit()
    {
        if (GameManager.Instance.Player.CanInteract)
        {
            Dehighlight();
        }
    }

    public Rigidbody2D Rigidbody
    {
        get
        {
            return myRigidbody;
        }
    }
}
