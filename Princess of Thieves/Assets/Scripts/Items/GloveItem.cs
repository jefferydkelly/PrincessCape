using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloveItem : UsableItem {
	protected static GameObject target;
	protected static Timer ResetTargetTimer = new Timer(()=>{
		if (GloveItem.target) {
			GloveItem.target.GetComponent<SpriteRenderer> ().color = Color.white;
			GloveItem.target = null;
		}}, 1.0f);
	protected int range = 10;
	public float maxTargetSpeed = 10;
	public float force = 100;
	protected Rigidbody2D targetBody;
	protected Player player;
	protected Rigidbody2D playerBody;
	protected bool pushingOnTarget = true;
	protected LineRenderer lineRenderer;

	protected Color lineColor;
	protected static GloveItem activeGlove;
	protected Vector2 hitNormal;

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

	protected void FindTarget() {
		Vector2 aim = player.TrueAim;
		RaycastHit2D hit = Physics2D.BoxCast (player.transform.position, new Vector2 (1, 1), aim.GetAngle (), aim, range, 1 << LayerMask.NameToLayer ("Metal"));

		if (hit && hit.collider.gameObject != target) {
			target = hit.collider.gameObject;
			hitNormal = hit.normal;

		}

		if (target != null) {
			targetBody = target.GetComponent<Rigidbody2D> ();
			pushingOnTarget = true;
			if (targetBody) {
				pushingOnTarget = targetBody.mass < playerBody.mass;
				targetBody.constraints = RigidbodyConstraints2D.FreezeRotation;
			}
			target.GetComponent<SpriteRenderer> ().color = lineColor;
			lineRenderer.enabled = true;
			lineRenderer.SetColors (lineColor, lineColor);
			lineRenderer.SetPositions (new Vector3[]{ player.transform.position, target.transform.position });
			player.HideMagnetRange ();
		} else {
			pushingOnTarget = false;
			Color circleColor = lineColor;
			circleColor.a = 0.25f;
			player.ShowMagnetRange (circleColor);
		}
	}
}
