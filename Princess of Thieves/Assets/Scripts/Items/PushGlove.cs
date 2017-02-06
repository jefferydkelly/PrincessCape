using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushGlove : GloveItem{

	PushPullDirection direction;

	private void Start()
	{
		player = GameManager.Instance.Player;
		playerBody = player.GetComponent<Rigidbody2D>();
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.enabled = false;
		lineColor = Color.red;
	}


	public override void Activate()
	{
		

		player.IsUsingMagnetGloves = true;

		if (activeGlove && activeGlove.IsActive) {
			Debug.Log ("Deactivating");
			activeGlove.Deactivate ();
		}
		activeGlove = this;

		itemActive = true;
		FindTarget ();

		if (target) {
			ResetTargetTimer.Stop ();
			/*
			if (player.Aiming.y == 1) {
				direction = PushPullDirection.Up;
			} else if (player.Aiming.y == -1) {
				direction = PushPullDirection.Down;
			} else if (player.Aiming.x == 1) {
				direction = PushPullDirection.Right;
			} else {
				direction = PushPullDirection.Left;
			}*/

		}
			
	}

	public override void Use() {

		if (target == null) {
			FindTarget ();

			if (target) {
				ResetTargetTimer.Stop ();
			}
		}
		if (target) {
			Vector2 distance = target.transform.position - player.transform.position;

			Vector2 moveDir;
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

				moveDir = (distance.normalized + player.TrueAim).normalized;
				moveDir.Normalize();
				if (pushingOnTarget) {
					//Heavier object, so the player gets moved
					moveDir.y *= -1;
					playerBody.AddForce (
						moveDir * force,
						ForceMode2D.Force);
				} else {
					targetBody.AddForce (
						moveDir * force,
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
	
		targetBody = null;
		pushingOnTarget = true;
		lineRenderer.enabled = false;
		itemActive = false;
		ResetTargetTimer.Reset ();
		ResetTargetTimer.Start ();
		player.HideMagnetRange ();

	}
}
