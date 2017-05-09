using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectStand : BlockController, ReflectiveObject {
    [SerializeField]
    AimDirection direction;
    Vector2 reflectionForward = new Vector2(1, 0);
    Vector2 rfTwo = new Vector2(1, 0);
    static Sprite[] sprites;

	void Awake() {
        if (sprites == null)
        {
            sprites = Resources.LoadAll<Sprite>("Sprites/ReflectStand");
        }
        
        float sqrtHalf = 1.0f / Mathf.Sqrt(2);
		switch (direction)
        {
            case AimDirection.UpRight:
                reflectionForward = new Vector2(0, 1);
                rfTwo = new Vector2(1, 0);
                break;
            case AimDirection.UpLeft:
                reflectionForward = new Vector2(0, 1);
                rfTwo = new Vector2(-1, 0);
                break;
            case AimDirection.DownLeft:
                reflectionForward = new Vector2(0, -1);
                rfTwo = new Vector2(-1, 0);
                break;
            case AimDirection.DownRight:
                reflectionForward = new Vector2(0, -1);
                rfTwo = new Vector2(1, 0);
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

	public Vector2 GetSurfaceForward(Vector2 fwd) {
		if (reflectionForward.Dot(fwd) == -1)
        {
            return rfTwo;
        }

        return reflectionForward;
	}

	public GameObject GameObject {
		get {
			return gameObject;
		}
	}

	public bool IsReflecting {
		get {
			return true;
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