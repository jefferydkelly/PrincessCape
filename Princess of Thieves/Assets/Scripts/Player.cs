﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Controller controller;
	private Rigidbody2D myRigidBody;
	private SpriteRenderer myRenderer;

	public float maxSpeed = 1;
	public float jumpImpulse = 10;

	// Use this for initialization
	void Start () {
		controller = new Controller();
		myRigidBody = GetComponent<Rigidbody2D>();
		myRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!GameManager.Instance.IsPaused)
		{
			Vector2 xForce = new Vector2(controller.Horizontal, 0) * 5;
			myRigidBody.AddForce(xForce, ForceMode2D.Force);

			if (Mathf.Abs(myRigidBody.velocity.x) > maxSpeed)
			{
				Vector2 vel = myRigidBody.velocity;
				vel.x = Mathf.Sign(vel.x) * maxSpeed;
				myRigidBody.velocity = vel;
			}

			if (IsOnGround)
			{
				if (controller.Jump)
				{
					foreach (RaycastHit2D hit in Physics2D.RaycastAll(transform.position, Vector3.up, JumpHeight, 1 << LayerMask.NameToLayer("Platforms")))
					{
						PlatformObject po = hit.collider.GetComponent<PlatformObject>();
						if (po.passThrough)
						{
							po.AllowPassThrough();
						}
					}
					myRigidBody.AddForce(new Vector2(0, jumpImpulse), ForceMode2D.Impulse);
				
				}
				else if (controller.Vertical == -1 && controller.Interact)
				{
					RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.0f, ~(1 << LayerMask.NameToLayer("Player")));

					PlatformObject po = hit.collider.GetComponent<PlatformObject>();

					if (po)
					{
						if (po.passThrough)
						{
							po.AllowPassThrough();
						}
					}
				}
			}
		}
	}

	bool IsOnGround
	{
		get
		{
			return Physics2D.Raycast(transform.position, Vector2.down, 1.0f, ~(1 << LayerMask.NameToLayer("Player")));
		}
	}

	float JumpHeight
	{
		get
		{
			return Mathf.Pow(jumpImpulse, 2) / (Physics.gravity.y * myRigidBody.gravityScale * -2);
		}
	}
	bool CanPassThrough
	{
		get
		{
			return false;
		}
	}

	public float HalfWidth
	{
		get
		{
			return myRenderer.bounds.extents.x;
		}
	}

	public float HalfHeight
	{
		get
		{
			return myRenderer.bounds.extents.y;
		}
	}
}
