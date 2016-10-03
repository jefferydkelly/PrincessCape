using UnityEngine;
using System.Collections;

public interface DamageableObject {
	bool TakeDamage(DamageSource ds);
}

public struct DamageSource
{
	public DamageType type;
	public int damage;

	public DamageSource(DamageType dt, int dmg = 0)
	{
		type = dt;
		damage = dmg;
	}
}

[System.Flags]
public enum DamageType
{
	None = 0,
	Earth = 1,
	Fire = 2,
	Wind = 4,
	Water = 8,
	Light = 16,
	Dark = 32
}
