using UnityEngine;
using System.Collections;

public class NoMagicArea : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.GetComponent<SpellProjectile>())
		{
			Destroy(col.gameObject);
		}
	}
}
