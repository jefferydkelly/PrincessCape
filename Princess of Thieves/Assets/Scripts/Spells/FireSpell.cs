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
		return go.AddComponent<FireballProjectile>();

	}
}
