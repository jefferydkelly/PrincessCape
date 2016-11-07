using UnityEngine;
using System.Collections;

public abstract class SpellProjectile : MonoBehaviour {
	public Vector3 fwd = new Vector3(1, 0, 0);
	public float moveSpeed = 10;
	public Allegiance allegiance = Allegiance.None;
	public int damage = 10;
    protected SpriteRenderer myRenderer;

    public virtual Vector3 FWD
    {
        get
        {
            return fwd;
        }

        set
        {
            fwd = value;
            if (fwd.y == 0)
            {
                myRenderer.flipX = (fwd.x == -1);
            }
            else
            {
                transform.Rotate(Vector3.forward, 90 * fwd.y);
            }
        }
    }

    public abstract void Enhance();

	public abstract void Diminish();
}
