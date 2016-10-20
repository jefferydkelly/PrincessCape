using UnityEngine;
using System.Collections;

public class HyperMagicArea : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D col) {
		SpellProjectile sp = col.GetComponent<SpellProjectile>();

		if (sp != null) {
			sp.Enhance();
		}
	}
}
