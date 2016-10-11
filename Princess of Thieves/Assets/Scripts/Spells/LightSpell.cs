using UnityEngine;
using System.Collections;

public class LightSpell : Spell {

	public override SpellProjectile Cast(CasterObject c)
	{
		GameObject go = new GameObject();
		LightProjectile l = go.AddComponent<LightProjectile>();
		go.transform.position = c.Position + (c.Forward * (c.GameObject.HalfWidth() + go.HalfWidth() + 0.1f));
		l.FWD = c.Forward;
		l.Init();
		return l;
	}
}
