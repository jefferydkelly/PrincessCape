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
}
