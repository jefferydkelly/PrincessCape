using UnityEngine;
using System.Collections;
using System;

public class BlockController : ResettableObject, InteractiveObject {
    public void Dehighlight()
    {
        UIManager.Instance.ShowInteraction("");
    }

    public void Highlight()
    {
        UIManager.Instance.ShowInteraction("Push");
    }

    public void Interact()
    {
        GameManager.Instance.Player.IsPushing = !GameManager.Instance.Player.IsPushing;

        if (GameManager.Instance.Player.IsPushing)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        } else
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }
    }

    // Use this for initialization
    void Start () {
        startPosition = transform.position;
	}
}
