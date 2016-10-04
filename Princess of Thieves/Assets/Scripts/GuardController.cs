using UnityEngine;
using System.Collections;

public class GuardController : EnemyController, AlertableObject {
	private Vector3 startPosition;
	public float patrolDistance = 3.0f;
	private Vector3 fwd = new Vector3(1, 0, 0);
	private Rigidbody2D myRigidBody;
	private VisionCone myCone;
	void Start()
	{
		state = EnemyState.Patrol;
		startPosition = transform.position;
		myRigidBody = GetComponent<Rigidbody2D>();
		myCone = GetComponentInChildren<VisionCone>();
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
			myCone.transform.position = transform.position + new Vector3(gameObject.HalfWidth() + myCone.gameObject.HalfWidth(), 0) * fwd.x;
			myCone.Rotation = 90 - (90 * fwd.x);
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (!col.collider.CompareTag("Player") && !col.collider.CompareTag("Platform"))
		{
			Debug.Log(col.collider.name);
			FWD *= -1;
		}
	}

	public void Alert()
	{
		Debug.Log("I've been alerted to the player");
	}
}
