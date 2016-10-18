using UnityEngine;
using System.Collections;

public class WindProjectile : SpellProjectile {
	public float pushForce = 10;
	// Use this for initialization
	bool swapped = false;
	void Awake () {
		name = "Cyclone";
		SpriteRenderer sr = gameObject.AddComponent<SpriteRenderer>();
		sr.sprite = Resources.Load<Sprite>("Sprites/Wind");
		BoxCollider2D col = gameObject.AddComponent<BoxCollider2D>();
		col.size = new Vector2(2.2f, 1.0f);
		col.isTrigger = true;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += moveSpeed * fwd * Time.deltaTime;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();

		if (rb)
		{
			if (!swapped)
			{
				rb.AddForce(fwd * pushForce, ForceMode2D.Impulse);
			}
			else {
				rb.AddForce(-fwd * pushForce, ForceMode2D.Impulse);
			}
			Destroy(gameObject);
		}
	}

	public override void Enhance()
	{
		pushForce = 20;
	}

	public override void Diminish()
	{
		swapped = true;
	}
}
