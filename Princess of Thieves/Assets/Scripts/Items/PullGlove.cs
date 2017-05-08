using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullGlove : GloveItem {

	// Use this for initialization
	PushPullDirection direction;
    Timer ProjectWaveTimer;
	private void Start()
	{
        myAudio = GetComponent<AudioSource>();
		player = GameManager.Instance.Player;
		playerBody = player.GetComponent<Rigidbody2D>();
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.enabled = false;
		lineColor = Color.blue;
        ProjectWaveTimer = new Timer(() => {
            if (target)
            {
                ProjectWave(target);
            }
        }, 0.25f, true);

}


public override void Activate()
	{
        myAudio.Play();
        player.IsUsingMagnetGloves = true;

		if (activeGlove && activeGlove.IsActive) {
			activeGlove.Deactivate ();
		}
        
		activeGlove = this;

		itemActive = true;
		FindTarget ();
        
       // InvokeRepeating("ProjectWave", 0, 0.5f);
        if (target) {
            ProjectWave(target);
            ProjectWaveTimer.Reset();
            ProjectWaveTimer.Start();
            ResetTargetTimer.Stop ();

			if (!targetIsHeavier)
			{
				if (!Physics2D.BoxCast (target.transform.position, Vector2.one, 0, Vector2.up, 1.0f, 1 << LayerMask.NameToLayer ("Player"))) {
					playerBody.constraints |= RigidbodyConstraints2D.FreezePositionX;
					if (player.IsOnGround) {
						playerBody.constraints |= RigidbodyConstraints2D.FreezePositionY;
					}
				}
			}
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

				if (targetIsHeavier) {
					//Heavier object, so the player gets moved
					//moveDir *= force;


					if (Mathf.Abs(hitNormal.y) <= 0.1f) {
						moveDir += player.KeyAim.YVector () * 1.15f;
					}  
					if (Mathf.Abs (hitNormal.x) <= 0.1f) {
						Vector2 xv = player.KeyAim.XVector ();

						if (Vector2.Dot (moveDir, xv) >= 0) {
							moveDir += xv * 5;
						}
					}

					moveDir.Normalize ();
					playerBody.AddForce (
						moveDir * force * 0.75f,
						ForceMode2D.Force);
				} else {
					if (Mathf.Abs (hitNormal.y) <= 0.1f) {
						moveDir -= player.KeyAim.YVector ();
					}
						
					moveDir.Normalize ();
					targetBody.AddForce (
						moveDir * -force,
						ForceMode2D.Force);
					targetBody.ClampVelocity (maxTargetSpeed);
				}
				//lineRenderer.SetPositions (new Vector3[] { player.transform.position, target.transform.position });
			}
		}
	}
		
	public override void Deactivate()
	{
        myAudio.Stop();
        player.IsUsingMagnetGloves = false;
		if (target) {
			target.GetComponent<SpriteRenderer> ().color = Color.white;
		}
        playerBody.constraints &= ~RigidbodyConstraints2D.FreezePosition;
		targetIsHeavier = true;
		//lineRenderer.enabled = false;
		itemActive = false;
		player.HideMagnetRange ();
		ResetTargetTimer.Reset ();
		ResetTargetTimer.Start ();
        ProjectWaveTimer.Stop();
    }

}

public enum PushPullDirection {
	Up,
	Down,
	Left,
	Right
}
