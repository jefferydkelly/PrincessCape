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

		if (activeGlove && activeGlove.IsActive) {
			activeGlove.Deactivate ();
		}
		activeGlove = this;

		itemActive = true;
		FindTarget ();

		if (target) {
			ResetTargetTimer.Stop ();
		}
			
	}

	public override void Use()
	{
		if (target == null) {
			FindTarget ();

			if (target) {
				ResetTargetTimer.Stop ();
			}
		}
		if (target != null) {
			Vector2 distance = target.transform.position - player.transform.position;
			Vector2 moveDir = Vector3.zero;

			if (distance.sqrMagnitude <= range * range) {

				moveDir = distance.normalized;
				/*
				if (Mathf.Abs(hitNormal.y) <= 0.1f) {
					moveDir -= player.TrueAim.YVector ();
				}
				moveDir.Normalize ();
				*/

				if (pushingOnTarget) {
					//Heavier object, so the player gets moved
					if (Mathf.Abs(hitNormal.y) <= 0.1f) {
						moveDir += player.TrueAim.YVector ();
					}
					moveDir.Normalize ();
					playerBody.AddForce (
						moveDir * force,
						ForceMode2D.Force);
				} else {
					if (Mathf.Abs(hitNormal.y) <= 0.1f) {
						moveDir -= player.TrueAim.YVector ();
					}
					moveDir.Normalize ();
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
		player.IsUsingMagnetGloves = false;
		if (target) {
			target.GetComponent<SpriteRenderer> ().color = Color.white;
		}

		pushingOnTarget = true;
		lineRenderer.enabled = false;
		itemActive = false;
		player.HideMagnetRange ();
		ResetTargetTimer.Reset ();
		ResetTargetTimer.Start ();
	}

}

public enum PushPullDirection {
	Up,
	Down,
	Left,
	Right
}
