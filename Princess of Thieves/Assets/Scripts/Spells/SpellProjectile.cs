using UnityEngine;
using System.Collections;

public class SpellProjectile : MonoBehaviour {
	public Vector3 fwd = new Vector3(1, 0, 0);
	public float moveSpeed = 10;
	public Allegiance allegiance = Allegiance.None;
	public int damage = 10;
}
