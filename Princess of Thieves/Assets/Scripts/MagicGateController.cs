using UnityEngine;
using System.Collections;

public class MagicGateController : GateController, DamageableObject {

	[EnumFlag]
	public DamageType openType;

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
		endPosition = startPosition - new Vector3(0, GetComponent<SpriteRenderer>().bounds.extents.y * 2);
	}

	public bool TakeDamage(DamageSource ds)
	{
		if ((openType & ds.type) > 0)
		{
			StartCoroutine(Open());
		}
		return false;
	}
}
