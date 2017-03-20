using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectStand : BlockController {
	[SerializeField]
	Vector2 reflectDirection = new Vector2 (1, 0);

	void Awake() {
		Sprite[] sprites = Resources.LoadAll<Sprite> ("Sprites/ReflectStand");
		Sprite mySprite = sprites [0];
		if (reflectDirection.y == 1) {
			mySprite = sprites [1];
		} else if (reflectDirection.x == -1) {
			mySprite = sprites [2];
		} else if (reflectDirection.y == -1) {
			mySprite = sprites [3];
		}
		GetComponent<SpriteRenderer> ().sprite = mySprite;
		myRigidbody = GetComponent<Rigidbody2D> ();
	}
	void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag ("Projectile")) {
			Rigidbody2D projectileBody = col.attachedRigidbody;
			if (projectileBody.velocity.Dot (reflectDirection) <= 0) {
				float rot = (reflectDirection.GetAngle () - projectileBody.velocity.GetAngle ());
				projectileBody.velocity = projectileBody.velocity.Rotated (rot);
				projectileBody.transform.RotateAround (projectileBody.transform.position, Vector3.fwd, rot * Mathf.Rad2Deg);
			}
		}
	}
}
