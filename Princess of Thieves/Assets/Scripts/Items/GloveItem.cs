using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloveItem : UsableItem {
	protected static GameObject target;
	protected static Timer ResetTargetTimer = new Timer(()=>{
		if (GloveItem.target) {
			GloveItem.target.GetComponent<SpriteRenderer> ().color = Color.white;
			GloveItem.target = null;
		}}, 0.5f);

    protected static int range = 10;
    protected static MetalBlock highlighted;
	public float maxTargetSpeed = 60f;
	public float force = 100;
	protected Rigidbody2D targetBody;
	protected Player player;
	protected Rigidbody2D playerBody;
	protected bool targetIsHeavier = true;
	protected LineRenderer lineRenderer;

	protected Color lineColor;
	protected static GloveItem activeGlove;
	protected Vector2 hitNormal;
	protected Vector2 hitPos;
    [SerializeField]
    GameObject waveSprite;
    [SerializeField]
	protected float difWeight = 0.5f;
	[SerializeField]
	protected float aimWeight = 0.5f;


    public override void Activate ()
	{
		
	}

	public override void Deactivate ()
	{
		
	}

	public override void Use ()
	{
		
	}

    /// <summary>
    /// Creates a wave that will move to target
    /// </summary>
    /// <param name="tar">The target that the waves will move to</param>
    public void ProjectWave(GameObject tar)
    {
        tempWaveScript wave = Instantiate(waveSprite, GameManager.Instance.Player.transform.position, transform.rotation).GetComponent<tempWaveScript>();
        
        wave.target = tar;
    }
	protected void FindTarget() {
        /*
        if (highlighted == null)
        {
            Vector2 aim = player.TrueAim;
            RaycastHit2D hit = Physics2D.BoxCast(player.transform.position, new Vector2(1, 1), aim.GetAngle(), aim, range, 1 << LayerMask.NameToLayer("Metal"));

            if (hit && hit.collider.gameObject != target)
            {
                target = hit.collider.gameObject;
                hitNormal = hit.normal;
                hitPos = hit.point;

            }
        } else
        {
            target = highlighted.gameObject;
            Vector3 dif = highlighted.transform.position - player.transform.position;
            if (Mathf.Abs(dif.x) >= Mathf.Abs(dif.y)) {
                hitNormal = new Vector2(Mathf.Sign(dif.x), 0);
            } else
            {
                hitNormal = new Vector2(0, Mathf.Sign(dif.y));
            }
        }*/

        if (highlighted != null)
        {
            target = highlighted.gameObject;
            highlighted = null;
        }
		if (target != null) {
			targetBody = target.GetComponent<Rigidbody2D> ();
			targetIsHeavier = true;
			if (targetBody) {
				targetIsHeavier = targetBody.mass > playerBody.mass;
				targetBody.constraints = RigidbodyConstraints2D.FreezeRotation;
			}
			target.GetComponent<SpriteRenderer> ().color = lineColor;
			//lineRenderer.enabled = true;
   //         lineRenderer.startColor = lineColor;
   //         lineRenderer.endColor = lineColor;
			//lineRenderer.SetPositions (new Vector3[]{ player.transform.position, target.transform.position });
			player.HideMagnetRange ();
		} else {
			targetIsHeavier = false;
			Color circleColor = lineColor;
			circleColor.a = 0.25f;
			player.ShowMagnetRange (circleColor);
		}
	}

    public static bool InRange(GameObject go)
    {
        return GameManager.Instance.DistanceToPlayer(go) <= range;
    }

    private void FixedUpdate()
    {
        if (IsActive)
        {
            if (targetIsHeavier)
            {
                playerBody.ClampVelocity(maxTargetSpeed);
            } else if (targetBody)
            {
                targetBody.ClampVelocity(maxTargetSpeed);
            }
        }
    }
    public static MetalBlock Highlighted
    {
        get
        {
            return highlighted;
        }

        set
        {
            highlighted = value;
        }
    }
}
