using UnityEngine;
using System.Collections;

public class WindProjectile : SpellProjectile {
	public float pushForce = 10;
	// Use this for initialization
	void Start () {
		SpriteRenderer sr = gameObject.AddComponent<SpriteRenderer>();
		sr.sprite = Resources.Load<Sprite>("Sprites/WindProjectile");
		Collider2D col = gameObject.AddComponent<BoxCollider2D>();
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
			rb.AddForce(fwd * pushForce, ForceMode2D.Impulse);
			Destroy(gameObject);
		}
	}
}
