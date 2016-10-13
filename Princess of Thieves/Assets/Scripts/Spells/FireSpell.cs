using UnityEngine;
using System.Collections;

public class FireSpell : Spell {

	public FireSpell()
	{
		spellName = "Fireball";
		cost = 10;
	}
	public override SpellProjectile Cast(CasterObject c)
	{
		GameObject go = new GameObject();
		FireballProjectile w = go.AddComponent<FireballProjectile>();
		w.FWD = c.Forward;
		go.transform.position = c.Position + (c.Forward * (c.GameObject.HalfWidth() + go.HalfWidth() + 0.1f));
		return w;

	}
}
