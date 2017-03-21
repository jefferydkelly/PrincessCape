using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectStand : BlockController {
    [SerializeField]
    ReflectDirection direction;
    Vector2 reflectionForward = new Vector2(1, 0);

	void Awake() {
        float sqrtHalf = 1.0f / Mathf.Sqrt(2);
		switch (direction)
        {
            case ReflectDirection.Right:
                reflectionForward = new Vector2(1, 0);
                break;
            case ReflectDirection.UpRight:
                reflectionForward = new Vector2(sqrtHalf, sqrtHalf);
                break;
            case ReflectDirection.Up:
                reflectionForward = new Vector2(0, 1);
                break;
            case ReflectDirection.UpLeft:
                reflectionForward = new Vector2(-sqrtHalf, sqrtHalf);
                break;
            case ReflectDirection.Left:
                reflectionForward = new Vector2(-1, 0);
                break;
            case ReflectDirection.DownLeft:
                reflectionForward = new Vector2(-sqrtHalf, -sqrtHalf);
                break;
            case ReflectDirection.Down:
                reflectionForward = new Vector2(0, -1);
                break;
            case ReflectDirection.DownRight:
                reflectionForward = new Vector2(sqrtHalf, -sqrtHalf);
                break;
        }

		myRigidbody = GetComponent<Rigidbody2D> ();
	}
	void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag ("Projectile")) {
            col.GetComponent<Projectile>().Reflect(reflectionForward);
		}
	}
}

public enum ReflectDirection
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