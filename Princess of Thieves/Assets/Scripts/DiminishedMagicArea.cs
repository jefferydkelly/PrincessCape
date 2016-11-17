using UnityEngine;
using System.Collections;

public class DiminishedMagicArea : JDMappableObject {

	void OnTriggerEnter2D(Collider2D col)
	{
		SpellProjectile sp = col.GetComponent<SpellProjectile>();

		if (sp)
		{
			sp.Diminish();
		}
	}
}
