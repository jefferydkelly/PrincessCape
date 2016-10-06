using UnityEngine;
using System.Collections;

public class WindSpell : Spell {

	public WindSpell()
	{
		spellName = "Gust";
		cost = 20;
	}

	public override SpellProjectile Cast(CasterObject c)
	{
		GameObject go = new GameObject();
		WindProjectile w = go.AddComponent<WindProjectile>();
		w.FWD = c.Forward;
		go.transform.position = c.Position + (c.Forward * (c.GameObject.HalfWidth() + go.HalfWidth() + 0.1f));
		c.RigidBody.AddForce(-c.Forward * w.pushForce, ForceMode2D.Impulse);
		return w;
	}
}
