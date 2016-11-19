using UnityEngine;
using System.Collections;

public class WaterSpell : Spell {

	public WaterSpell()
	{
		spellName = "Wave";
		cost = 20;
	}

	public override SpellProjectile Cast(CasterObject c)
	{
		GameObject go = new GameObject();
		WaterProjectile w = go.AddComponent<WaterProjectile>();
		w.FWD = c.Forward;
		go.transform.position = c.Position;
		w.Init();
		return w;
	}
}
