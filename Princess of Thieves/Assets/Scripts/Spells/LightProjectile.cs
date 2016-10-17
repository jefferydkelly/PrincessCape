using UnityEngine;
using System.Collections;

public class LightProjectile : SpellProjectile {

	private float lifeTime;
	private float speed = 5.0f;
	private Rigidbody2D myRigidbody;
	void Awake()
	{
		name = "Ball of Light";
		SpriteRenderer sr = gameObject.AddComponent<SpriteRenderer>();
		sr.sprite = Resources.Load<Sprite>("Sprites/LightBall");
		gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
		myRigidbody = gameObject.AddComponent<Rigidbody2D>();
		myRigidbody.gravityScale = 0;
		//Invoke("Destroy", lifeTime);
	}

	public void Init()
	{
		myRigidbody.velocity = FWD * speed;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		SpellProjectile sp = col.GetComponent<SpellProjectile>();

		if (sp)
		{
			sp.Enhance();
			Destroy(gameObject);
		}

	}

	public override void Enhance()
	{
	}

	public override void Diminish()
	{
		Destroy(gameObject);
	}

}
