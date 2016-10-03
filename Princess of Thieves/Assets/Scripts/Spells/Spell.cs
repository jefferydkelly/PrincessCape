using UnityEngine;
using System.Collections;

public abstract class Spell {
	private string spellName = "Generic Spell";
	private int cost = 50;

	public abstract SpellProjectile Cast();

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
