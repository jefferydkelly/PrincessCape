using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushGlove : UsableItem {

	int range = 10;
	public float maxTargetSpeed = 10;
	public float force = 100;
	// Use this for initialization
	GameObject target;
	Rigidbody2D targetBody;
	Player player;
	Rigidbody2D playerBody;
	PushPullDirection direction;
	bool pushingOnTarget = true;
	LineRenderer lineRenderer;

	private void Start()
	{
		player = GameManager.Instance.Player;
		playerBody = player.GetComponent<Rigidbody2D>();
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.enabled = false;
	}


	public override void Activate()
	{
		if (player.MP >= activationManaCost && !onCooldown) {
			player.MP -= activationManaCost;
			//Shoot a ray fowards
			RaycastHit2D hit = Physics2D.BoxCast (player.transform.position, new Vector2 (1, 1), player.Aiming.GetAngle (), player.Aiming, range, 1 << LayerMask.NameToLayer ("Metal"));
			if (hit) {//first hit object has an ObjectWeight

				target = hit.collider.gameObject;
				Color col = Color.red;
				target.GetComponent<SpriteRenderer> ().color = col;
				lineRenderer.enabled = true;
				lineRenderer.SetColors (col, col);
				lineRenderer.SetPositions (new Vector3[]{ player.transform.position, target.transform.position });
				targetBody = target.GetComponent<Rigidbody2D> ();
				if (targetBody) {
					pushingOnTarget = targetBody.mass < playerBody.mass;
					targetBody.constraints = RigidbodyConstraints2D.FreezeRotation;
				}
				player.IsUsingMagnetGloves = true;


				if (player.Aiming.y == 1) {
					direction = PushPullDirection.Up;
				} else if (player.Aiming.y == -1) {
					direction = PushPullDirection.Down;
				} else if (player.Aiming.x == 1) {
					direction = PushPullDirection.Right;
				} else {
					direction = PushPullDirection.Left;
				}
				itemActive = true;
			}

		}
	}

	public override void Use() {

		if (target != null) {
			Vector2 distance = target.transform.position - player.transform.position;

			Vector2 moveDir;
			if (distance.sqrMagnitude <= range * range) {
				
				if (direction == PushPullDirection.Up) {
					moveDir = Vector2.up;
				} else if (direction == PushPullDirection.Down) {
					moveDir = Vector2.down;
				} else {
					moveDir = new Vector2 (direction == PushPullDirection.Right ? 1 : -1, 0);
				}
				if (pushingOnTarget && (direction == PushPullDirection.Up || direction == PushPullDirection.Down)) {
					moveDir += player.Aiming.XVector ();
				}

				 //moveDir = (distance.normalized + player.TrueAim).normalized;
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
		if (player.IsUsingMagnetGloves)
		{
			player.IsUsingMagnetGloves = false;
			target.GetComponent<SpriteRenderer>().color = Color.white;
		
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
