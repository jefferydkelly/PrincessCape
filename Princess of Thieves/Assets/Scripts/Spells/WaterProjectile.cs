using UnityEngine;
using System.Collections;

public class WaterProjectile : SpellProjectile {

	float startForce = 10;
	float lifeTime = 5;
	float oldXVel = 0;
	int numberOfThingsToPassThrough = 5;
	int passedThrough = 0;
	Rigidbody2D myRigidbody;
	// Use this for initialization
	void Start () {
		SpriteRenderer sr = gameObject.AddComponent<SpriteRenderer>();
		sr.sprite = Resources.Load<Sprite>("Sprites/Wave");
		gameObject.AddComponent<BoxCollider2D>();
		myRigidbody = gameObject.AddComponent<Rigidbody2D>();
		myRigidbody.AddForce(fwd * startForce, ForceMode2D.Impulse);
		myRigidbody.freezeRotation = true;
		myRigidbody.mass = 0.5f;
		Invoke("Destroy", lifeTime);
	}

	void FixedUpdate()
	{
		oldXVel = myRigidbody.velocity.x;
	}
	void OnCollisionEnter2D(Collision2D col)
	{
		if (!col.gameObject.CompareTag("Platform"))
		{
			DamageableObject dObj = col.gameObject.GetComponent<DamageableObject>();

			if (dObj != null && dObj.Allegiance != allegiance)
			{
				dObj.TakeDamage(new DamageSource(DamageType.Water, damage, allegiance));
			}

			Physics2D.IgnoreCollision(GetComponent<Collider2D>(), col.collider);
			myRigidbody.velocity = new Vector2(oldXVel, myRigidbody.velocity.y);

			passedThrough++;

			if (passedThrough >= numberOfThingsToPassThrough)
			{
				Destroy();
			}
		}
	}

	void Destroy()
	{
		CancelInvoke();
		Destroy(gameObject);
	}
}
