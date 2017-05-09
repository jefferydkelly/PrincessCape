using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour {
    [SerializeField]
    AimDirection direction;
    Vector2 reflectionForward = new Vector2(1, 0);
    static Sprite[] sprites;
    private Rigidbody2D myRigidbody;
    private SpriteRenderer myRenderer;

    public Vector2 SurfaceForward
    {
        get
        {
            return reflectionForward;
        }
    }

    public GameObject GameObject
    {
        get
        {
            return gameObject;
        }
    }

    public bool IsReflecting
    {
        get
        {
            return true;
        }
    }

    private void Awake()
    {
        if (sprites == null)
        {
            sprites = Resources.LoadAll<Sprite>("Sprites/Mirror");
        }
        int spriteNum = -1;
        switch (direction)
        {
            case AimDirection.Right:
                spriteNum = 0;
                reflectionForward = new Vector2(1, 0);
                break;
            case AimDirection.Up:
                spriteNum = 1;
                reflectionForward = new Vector2(0, 1);
                break;
            case AimDirection.Left:
                spriteNum = 2;
                reflectionForward = new Vector2(-1, 0);
                break;
            case AimDirection.Down:
                spriteNum = 3;
                reflectionForward = new Vector2(0, -1);
                break;
        }

        myRigidbody = GetComponent<Rigidbody2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        myRenderer.sprite = sprites[spriteNum];
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Projectile"))
        {
            col.GetComponent<Projectile>().Reflect(reflectionForward);
        }
    }
}
