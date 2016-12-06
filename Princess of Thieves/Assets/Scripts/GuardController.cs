using UnityEngine;
using System.Collections;

public class GuardController : EnemyController, AlertableObject {
	private Vector3 startPosition;
	public float patrolDistance = 3.0f;
	private Vector3 fwd = new Vector3(1, 0, 0);
	private Rigidbody2D myRigidBody;
	void Start()
	{
		state = EnemyState.Patrol;
		startPosition = transform.position;
		myRigidBody = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		if (state == EnemyState.Patrol)
		{
			myRigidBody.AddForce(fwd * 5);
			myRigidBody.ClampVelocity(2);

			Vector3 dist = transform.position - startPosition;
			if (dist.x / fwd.x > patrolDistance)
			{
				FWD *= -1;
			}
		}
	}

	public Vector3 FWD
	{
		get
		{
			return fwd;
		}

		protected set
		{
			fwd = value;
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (!col.collider.CompareTag("Player") && !col.collider.CompareTag("Platform"))
		{
			FWD *= -1;
		}
	}

	public void Alert()
	{
		
	}
}
