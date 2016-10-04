using UnityEngine;
using System.Collections;

public class WaterProjectile : SpellProjectile {

	// Use this for initialization
	void Start () {
		SpriteRenderer sr = gameObject.AddComponent<SpriteRenderer>();
		sr.sprite = Resources.Load<Sprite>("Sprites/Wave");
		Collider2D col = gameObject.AddComponent<EdgeCollider2D>();
		col.isTrigger = true;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += moveSpeed * fwd * Time.deltaTime;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		DamageableObject dObj = col.gameObject.GetComponent<DamageableObject>();

		if (dObj != null && dObj.Allegiance != allegiance)
		{
			dObj.TakeDamage(new DamageSource(DamageType.Water, damage, allegiance));	
		}
	}
}
