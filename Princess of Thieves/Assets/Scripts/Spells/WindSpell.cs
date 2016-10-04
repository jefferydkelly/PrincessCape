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
		c.RigidBody.AddForce(-c.Forward * w.pushForce, ForceMode2D.Impulse);
		return w;
	}
}
