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

		go.transform.position = c.Position + new Vector3(w.FWD.x * (c.GameObject.HalfWidth() + go.HalfWidth() + 0.1f), w.FWD.y * (c.GameObject.HalfHeight() + go.HalfHeight() + 0.1f));
		c.RigidBody.AddForce(-w.FWD * w.pushForce, ForceMode2D.Impulse);
		return w;
	}
}
