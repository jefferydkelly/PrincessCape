using UnityEngine;
using System.Collections;

public class EarthSpell : Spell {

	public EarthSpell()
	{
		spellName = "Raise Earth";
		cost = 30;
	}
	public override SpellProjectile Cast(CasterObject c)
	{
		GameObject go = new GameObject();
		EarthProjectile ep = go.AddComponent<EarthProjectile>();
		ep.caster = c;
		go.transform.position = c.Position + (c.Forward * (c.GameObject.HalfWidth() + go.HalfWidth() * 1.5f)) - (Vector3.up * c.GameObject.HalfHeight());
		return ep;
	}
}
