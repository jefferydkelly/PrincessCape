using UnityEngine;
using System.Collections;

public abstract class SpellProjectile : MonoBehaviour {
	public Vector3 fwd = new Vector3(1, 0, 0);
	public float moveSpeed = 10;
	public Allegiance allegiance = Allegiance.None;
	public int damage = 10;

	public Vector3 FWD
	{
		get
		{
			return fwd;
		}

		set
		{
			fwd = value;
			transform.localRotation = Quaternion.AngleAxis(90 - (90 * fwd.x), Vector3.up);
		}
	}

	public abstract void Enhance();

	public abstract void Diminish();
}
