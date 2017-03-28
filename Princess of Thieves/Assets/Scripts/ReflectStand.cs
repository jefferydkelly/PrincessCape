using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectStand : BlockController {
    [SerializeField]
    AimDirection direction;
    Vector2 reflectionForward = new Vector2(1, 0);
    static Sprite[] sprites;

	void Awake() {
        if (sprites == null)
        {
            sprites = Resources.LoadAll<Sprite>("Sprites/ReflectStand");
        }
        
        float sqrtHalf = 1.0f / Mathf.Sqrt(2);
		switch (direction)
        {
            case AimDirection.Right:
                reflectionForward = new Vector2(1, 0);
                break;
            case AimDirection.UpRight:
                reflectionForward = new Vector2(sqrtHalf, sqrtHalf);
                break;
            case AimDirection.Up:
                reflectionForward = new Vector2(0, 1);
                break;
            case AimDirection.UpLeft:
                reflectionForward = new Vector2(-sqrtHalf, sqrtHalf);
                break;
            case AimDirection.Left:
                reflectionForward = new Vector2(-1, 0);
                break;
            case AimDirection.DownLeft:
                reflectionForward = new Vector2(-sqrtHalf, -sqrtHalf);
                break;
            case AimDirection.Down:
                reflectionForward = new Vector2(0, -1);
                break;
            case AimDirection.DownRight:
                reflectionForward = new Vector2(sqrtHalf, -sqrtHalf);
                break;
        }

		myRigidbody = GetComponent<Rigidbody2D> ();
        myRenderer = GetComponent<SpriteRenderer>();
        myRenderer.sprite = sprites[(int)direction];
	}
	void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag ("Projectile")) {
            col.GetComponent<Projectile>().Reflect(reflectionForward);
		}
	}
}

public enum AimDirection
{
    Right,
    UpRight,
    Up,
    UpLeft,
    Left,
    DownLeft,
    Down,
    DownRight
}