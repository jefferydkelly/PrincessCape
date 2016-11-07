using UnityEngine;
using System.Collections;

public class DarkProjectile : SpellProjectile {

	Rigidbody2D myRigidbody;
	float lifeTime = 3.0f;
	float speed = 15.0f;
	// Use this for initialization
	void Awake () {
		name = "Dark Bomb";
        myRenderer = gameObject.AddComponent<SpriteRenderer>();
        myRenderer.sprite = Resources.Load<Sprite>("Sprites/DarkBomb");
		gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
		myRigidbody = gameObject.AddComponent<Rigidbody2D>();
		myRigidbody.gravityScale = 0.0f;
		myRigidbody.freezeRotation = true;
		myRigidbody.mass = 0.5f;
		//Invoke("Destroy", lifeTime);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		SpellProjectile sp = col.GetComponent<SpellProjectile>();

		if (sp)
		{
			sp.Diminish();
			Destroy(gameObject);
		}
	}

	public void Init()
	{
		myRigidbody.velocity = FWD * speed;
	}

	public override void Enhance()
	{
		Destroy(gameObject);
	}

	public override void Diminish()
	{
		
	}
}
