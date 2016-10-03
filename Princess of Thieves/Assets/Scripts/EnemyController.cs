using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour, DamageableObject {

	protected int curHP = 0;
	public int maxHP = 0;
	protected EnemyState state = EnemyState.Patrol;

	[EnumFlag]
	public DamageType resistances = DamageType.Physical;
	[EnumFlag]
	public DamageType weaknesses = DamageType.Physical;

	public bool TakeDamage(DamageSource ds)
	{
		if (ds.allegiance != Allegiance.Enemy)
		{
			int totalDmg = ds.damage;

			if (Resists(ds.type))
			{
				totalDmg /= 2;
			}
			else if (IsWeakAgainst(ds.type))
			{
				totalDmg *= 2;
			}
			curHP -= totalDmg;
		}
		return curHP <= 0;
	}

	public Allegiance Allegiance
	{
		get
		{
			return Allegiance.Enemy;
		}
	}

	protected bool Resists(DamageType dt)
	{
		return (dt & resistances) > 0;
	}

	protected bool IsWeakAgainst(DamageType dt)
	{
		return (dt & weaknesses) > 0;
	}


}

public enum EnemyState
{
	None = 0,
	Patrol = 1,
	Search = 2,
	Pursue = 4
}
