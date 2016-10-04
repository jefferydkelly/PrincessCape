using UnityEngine;
using System.Collections;

public abstract class Spell {
	protected string spellName = "Generic Spell";
	protected int cost = 50;

	public abstract SpellProjectile Cast(CasterObject c);

	public string SpellName
	{
		get
		{
			return spellName;
		}
	}

	public int Cost
	{
		get
		{
			return cost;
		}
	}
}
