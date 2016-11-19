using UnityEngine;
using System.Collections;

public class WindSpell : Spell {

	public WindSpell()
	{
		spellName = "Gust";
		cost = 20;
	}

	public override SpellProjectile Cast(CasterObject c)
	{
		GameObject go = new GameObject();
		WindProjectile w = go.AddComponent<WindProjectile>();

        if (c is Player)
        {
            Vector2 id = (c as Player).Controller.InputDirection;

            if (id.SqrMagnitude() > 0)
            {
                w.FWD = id;
            }
            else
            {
                w.FWD = c.Forward;
            }
        }
        else
        {
            w.FWD = c.Forward;
        }

		go.transform.position = c.Position;
		c.RigidBody.AddForce(-w.FWD * w.pushForce, ForceMode2D.Impulse);
		return w;
	}
}
