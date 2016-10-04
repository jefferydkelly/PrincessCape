using UnityEngine;
using System.Collections;

public class WaterProjectile : SpellProjectile {

	float startForce = 10;
	float lifeTime = 5;
	// Use this for initialization
	void Start () {
		SpriteRenderer sr = gameObject.AddComponent<SpriteRenderer>();
		sr.sprite = Resources.Load<Sprite>("Sprites/Wave");
		gameObject.AddComponent<BoxCollider2D>();
		Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
		rb.AddForce(fwd * startForce, ForceMode2D.Impulse);
		rb.freezeRotation = true;
		rb.mass = 0.5f;
		Invoke("Destroy", lifeTime);
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (!col.gameObject.CompareTag("Platform"))
		{
			Debug.Log(col.gameObject.tag);
			DamageableObject dObj = col.gameObject.GetComponent<DamageableObject>();

			if (dObj != null && dObj.Allegiance != allegiance)
			{
				dObj.TakeDamage(new DamageSource(DamageType.Water, damage, allegiance));

			}

			Destroy();
		}
	}

	void Destroy()
	{
		CancelInvoke();
		Destroy(gameObject);
	}
}
