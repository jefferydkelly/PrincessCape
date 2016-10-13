﻿using UnityEngine;
using System.Collections;

public class FireballProjectile : SpellProjectile {

	bool enhanced = false;
	bool flipped = false;
	void Start () {
		name = "Fireball";
		SpriteRenderer sr = gameObject.AddComponent<SpriteRenderer>();
		sr.sprite = Resources.Load<Sprite>("Sprites/fireprojectile");
		Collider2D col = gameObject.AddComponent<CircleCollider2D>();
		col.isTrigger = true;
		Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
		rb.gravityScale = 0;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += fwd * moveSpeed * Time.deltaTime;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		BurnableObject bob = col.GetComponent<BurnableObject>();

		if (bob != null)
		{
			if (!flipped)
			{
				bob.Burn();
			}
			else {
			}
		}

		DamageableObject dObj = col.GetComponent<DamageableObject>();

		if (dObj != null && dObj.Allegiance != allegiance)
		{
			
			dObj.TakeDamage(new DamageSource((flipped ? DamageType.Ice : DamageType.Fire), damage, allegiance));
			if (!enhanced)
			{
				Destroy(gameObject);
			}
			else {
				enhanced = false;
			}
		}

	}

	public override void Enhance()
	{
		enhanced = true;
	}

	public override void Diminish()
	{
		flipped = true;
	}

	public override Vector3 FWD
	{
		get
		{
			return fwd;
		}
		set
		{
			fwd = value;
			transform.localRotation = Quaternion.AngleAxis(90 + (90 * fwd.x), Vector3.up);
		}
	}
}
