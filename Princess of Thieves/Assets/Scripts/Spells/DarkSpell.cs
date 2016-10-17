using UnityEngine;
using System.Collections;

public class DarkSpell : Spell {
	public DarkSpell()
	{
	}

	public override SpellProjectile Cast(CasterObject c)
	{
		GameObject go = new GameObject();
		DarkProjectile d = go.AddComponent<DarkProjectile>();
		go.transform.position = c.Position + (c.Forward * (c.GameObject.HalfWidth() + go.HalfWidth() + 0.1f));
		d.FWD = c.Forward;
		d.Init();
		return d;
	}
}
