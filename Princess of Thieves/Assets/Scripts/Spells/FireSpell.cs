using UnityEngine;
using System.Collections;

public class FireSpell : Spell {
	
	public override SpellProjectile Cast()
	{
		GameObject go = new GameObject();
		return go.AddComponent<FireballProjectile>();

	}
}
