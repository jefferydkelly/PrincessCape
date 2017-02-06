using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullGlove : GloveItem {

	// Use this for initialization
	PushPullDirection direction;

	private void Start()
	{
		player = GameManager.Instance.Player;
		playerBody = player.GetComponent<Rigidbody2D>();
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.enabled = false;
		lineColor = Color.blue;
	}


	public override void Activate()
	{
		player.IsUsingMagnetGloves = true;
		itemActive = true;
		FindTarget ();
			
	}

	public override void Use()
	{
		if (target == null) {
			FindTarget ();
		}
		if (target != null) {
			Vector2 distance = target.transform.position - player.transform.position;
			Vector2 moveDir = Vector3.zero;

			if (distance.sqrMagnitude <= range * range) {

				/*
				if (direction == PushPullDirection.Up) {
					moveDir = Vector2.up;
				} else if (direction == PushPullDirection.Down) {
					moveDir = Vector2.down;
				} else {
					moveDir = new Vector2 (direction == PushPullDirection.Right ? 1 : -1, 0);
				}
				if (pushingOnTarget && (direction == PushPullDirection.Up || direction == PushPullDirection.Down)) {
					moveDir += player.Aiming.XVector ();
				}*/
				moveDir.Normalize ();
				moveDir = (distance.normalized + player.TrueAim).normalized;


				if (pushingOnTarget) {
					//Heavier object, so the player gets moved
					moveDir.x *= -1;
					playerBody.AddForce (
						moveDir * force,
						ForceMode2D.Force);
				} else {
					targetBody.AddForce (
						moveDir * -force,
						ForceMode2D.Force);
					targetBody.ClampVelocity (maxTargetSpeed);
				}
				lineRenderer.SetPositions (new Vector3[] { player.transform.position, target.transform.position });
			}
		}
	}
		
	public override void Deactivate()
	{
		if (player.IsUsingMagnetGloves)
		{
			player.IsUsingMagnetGloves = false;
			if (target) {
				target.GetComponent<SpriteRenderer> ().color = Color.white;
			}

			if (targetBody != null) {
				targetBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
				targetBody = null;
			}
			pushingOnTarget = true;
			lineRenderer.enabled = false;
			itemActive = false;
		}
	}

}

public enum PushPullDirection {
	Up,
	Down,
	Left,
	Right
}
