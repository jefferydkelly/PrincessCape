using UnityEngine;
using System.Collections;

public class WaterProjectile : SpellProjectile {

	float startForce = 10;
	float lifeTime = 5;
	float oldXVel = 0;
	int numberOfThingsToPassThrough = 1;
	int passedThrough = 0;
	Rigidbody2D myRigidbody;
	// Use this for initialization
	void Awake () {
		name = "Tsunami";
        myRenderer = gameObject.AddComponent<SpriteRenderer>();
        myRenderer.sprite = Resources.Load<Sprite>("Sprites/Water");
        BoxCollider2D col = gameObject.AddComponent<BoxCollider2D>();
		col.size = new Vector2(2, 1);
		col.offset = new Vector2(0.07f, 0);
		myRigidbody = gameObject.AddComponent<Rigidbody2D>();
		myRigidbody.freezeRotation = true;
		myRigidbody.mass = 0.5f;
		Invoke("Destroy", lifeTime);
	}

	public void Init()
	{
		myRigidbody.AddForce(fwd * startForce, ForceMode2D.Impulse);
	}
	void FixedUpdate()
	{
		oldXVel = myRigidbody.velocity.x;

		if (Mathf.Abs(oldXVel) < Mathf.Epsilon)
		{
			Destroy();
		}
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

			BurnableObject bob = col.gameObject.GetComponent<BurnableObject>();

			if (bob != null)
			{
				bob.Douse();
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

	void OnTriggerEnter2D(Collider2D col)
	{
		BurnableObject bob = col.gameObject.GetComponent<BurnableObject>();

		if (bob != null)
		{
			bob.Douse();
		}
	}

	void Destroy()
	{
		CancelInvoke();
		Destroy(gameObject);
	}

	public override void Enhance()
	{
		transform.localScale *= 2;
	}

	public override void Diminish()
	{
		transform.localScale /= 2;
	}
}
