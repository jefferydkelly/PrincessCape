using UnityEngine;
using System.Collections;

public class FireSpell : Spell {

	public FireSpell()
	{
		spellName = "Fireball";
		cost = 10;
	}
	public override SpellProjectile Cast(CasterObject c)
	{
		GameObject go = new GameObject();
		FireballProjectile w = go.AddComponent<FireballProjectile>();
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
        return w;

	}
}
