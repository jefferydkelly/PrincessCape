using UnityEngine;
using System.Collections;

public interface DamageableObject {
	bool TakeDamage(DamageSource ds);
	Allegiance Allegiance { get; }
}

public struct DamageSource
{
	public DamageType type;
	public int damage;
	public Allegiance allegiance;

	public DamageSource(DamageType dt, int dmg = 0, Allegiance a = Allegiance.None)
	{
		type = dt;
		damage = dmg;
		allegiance = a;
	}
}

[System.Flags]
public enum DamageType
{
	Earth = 1,
	Fire = 2,
	Wind = 4,
	Water = 8,
	Ice = 16,
	Light = 32,
	Dark = 64,
	Physical = 128
}

public enum Allegiance
{
	None,
	Player,
	Enemy,
	Neutral
}
