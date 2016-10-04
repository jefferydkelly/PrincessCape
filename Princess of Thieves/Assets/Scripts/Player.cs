using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, DamageableObject, CasterObject {

	private Controller controller;
	private Rigidbody2D myRigidBody;
	private SpriteRenderer myRenderer;

	private int fwdX = 1;
	public float maxSpeed = 1;
	public float jumpImpulse = 10;
	private float lastYVel = 0;

	private int curHP = 0;
	public int maxHP = 100;

	private int curMP = 0;
	public int maxMP = 100;

	private Spell curSpell = new EarthSpell();
	// Use this for initialization
	void Start () {
		controller = new Controller();
		myRigidBody = GetComponent<Rigidbody2D>();
		myRenderer = GetComponent<SpriteRenderer>();
		curHP = maxHP;
		curMP = maxMP;
	}
	
	// Update is called once per frame
	void Update () {
		if (!GameManager.Instance.IsPaused)
		{
			Vector2 xForce = new Vector2(controller.Horizontal, 0) * 15;
			myRigidBody.AddForce(xForce, ForceMode2D.Force);

			myRigidBody.ClampVelocity(maxSpeed, VelocityType.X);

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

			if (Mathf.Abs(myRigidBody.velocity.x) > float.Epsilon)
			{
				fwdX = (int)Mathf.Sign(myRigidBody.velocity.x);
			}
			if (controller.UseSpell && curMP >= curSpell.Cost)
			{
				SpellProjectile sp = curSpell.Cast(this);
				sp.allegiance = Allegiance.Player;
				curMP -= curSpell.Cost;
			}
		}
	}

	void FixedUpdate()
	{
		lastYVel = myRigidBody.velocity.y;
	}

	bool IsOnGround
	{
		get
		{
			return Physics2D.Raycast(transform.position, Vector2.down, 1.0f, ~(1 << LayerMask.NameToLayer("Player")));
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.collider.CompareTag("Platform"))
		{
			
			if (lastYVel < -10)
			{
				TakeDamage(new DamageSource(DamageType.Physical, 10));
			}
		}
	}

	float JumpHeight
	{
		get
		{
			return Mathf.Pow(jumpImpulse, 2) / (Physics.gravity.y * myRigidBody.gravityScale * -2);
		}
	}

	public bool TakeDamage(DamageSource ds)
	{
		if (ds.allegiance != Allegiance.Player)
		{
			curHP -= ds.damage;
		}
		return curHP <= 0;
	}

	public Allegiance Allegiance
	{
		get
		{
			return Allegiance.Player;
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

	public float HPPercent
	{
		get
		{
			return (float)curHP / (float)maxHP;
		}
	}

	public int MP
	{
		get
		{
			return curMP;
		}
	}
	public float MPPercent
	{
		get
		{
			return (float)curMP / (float)maxMP;
		}
	}

	public Vector3 Forward
	{
		get
		{
			return new Vector3(fwdX, 0, 0);
		}
	}

	public Rigidbody2D RigidBody
	{
		get
		{
			return myRigidBody;
		}
	}

	public GameObject GameObject
	{
		get
		{
			return gameObject;
		}
	}

	public Vector3 Position
	{
		get
		{
			return transform.position;
		}
	}
}
