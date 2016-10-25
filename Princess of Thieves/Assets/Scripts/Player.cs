using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, DamageableObject, CasterObject {

	private Controller controller;
	private Rigidbody2D myRigidBody;
	private SpriteRenderer myRenderer;

	private int fwdX = 1;
	public float maxSpeed = 1;
    public float sneakSpeed = 0.5f;
    public float jumpImpulse = 10;
	private float lastYVel = 0;
    private bool onRope = false;

	private int curHP = 0;
	public int maxHP = 100;

	private int curMP = 0;
	public int maxMP = 100;

	private Spell curSpell = new FireSpell();
	bool onCooldown = false;
	private int numRopesTouching = 0;

    bool hidden = false;
	bool canUseMagic = true;
	// Use this for initialization
	void Start () {
		controller = new Controller();
		myRigidBody = GetComponent<Rigidbody2D>();
		myRenderer = GetComponent<SpriteRenderer>();
		curHP = maxHP;
		curMP = maxMP;
		//UIManager.Instance.ShowSpell = true;
	}
	
    
	// Update is called once per frame
	void Update () {
		if (!GameManager.Instance.IsPaused && !Hidden)
		{
			Vector2 xForce = new Vector2(controller.Horizontal, 0) * 15;
			myRigidBody.AddForce(xForce, ForceMode2D.Force);

            if(controller.Sneak)
				myRigidBody.ClampVelocity(sneakSpeed, VelocityType.X);
            else
				myRigidBody.ClampVelocity(maxSpeed, VelocityType.X);

			if (IsOnRope)
			{
				Vector2 vel = myRigidBody.velocity;
				vel.x = 0;
				myRigidBody.velocity = vel;
				myRigidBody.AddForce(new Vector2(0, controller.Vertical) * 35);
				myRigidBody.ClampVelocity(maxSpeed, VelocityType.Y);

				if (controller.Jump)
				{
					Jump();
				}
			}
			else if (IsOnGround)
			{
				if (controller.Jump)
				{
					Jump();
				}
				else {
					if (controller.Interact)
					{
						RaycastHit2D hit = Physics2D.Raycast(transform.position, Forward, 2.0f, ~(1 << LayerMask.NameToLayer("Player")));

						if (hit.collider != null)
						{
							InteractiveObject io = hit.collider.GetComponent<InteractiveObject>();

							if (io != null)
							{
								Debug.Log("Interacting");
								io.Interact();
							}
						}
					}
				}
			}

			if (Mathf.Abs(myRigidBody.velocity.x) > float.Epsilon)
			{
				fwdX = (int)Mathf.Sign(myRigidBody.velocity.x);
                myRenderer.flipX = (fwdX == -1);
			}
			if (!onCooldown && controller.UseSpell && curMP >= curSpell.Cost)
			{
				SpellProjectile sp = curSpell.Cast(this);
				onCooldown = true;
				Invoke("SpellCooldown", 1.0f);
				sp.allegiance = Allegiance.Player;
				curMP -= curSpell.Cost;
			}
		} else if (!GameManager.Instance.IsPaused && Hidden && controller.Interact)
		{
			Hidden = false;
		}

	}

	void SpellCooldown()
	{
		onCooldown = false;
	}

	void Jump()
	{
		foreach (RaycastHit2D hit in Physics2D.RaycastAll(transform.position, Vector3.up, JumpHeight, 1 << LayerMask.NameToLayer("Platforms")))
		{
			PlatformObject po = hit.collider.GetComponent<PlatformObject>();
			if (po.passThrough)
			{
				po.AllowPassThrough();
			}
		}
		myRigidBody.AddForce(new Vector2(0, jumpImpulse * Mathf.Sign(myRigidBody.gravityScale)), ForceMode2D.Impulse);
	}

	void FixedUpdate()
	{
		lastYVel = myRigidBody.velocity.y;
    }

	/// <summary>
	/// Handles the Player taking damage.
	/// </summary>
	/// <returns><c>true</c>, if the player is killed, <c>false</c> otherwise.</returns>
	/// <param name="ds">Ds.</param>
	public bool TakeDamage(DamageSource ds)
	{
		if (ds.allegiance != Allegiance.Player)
		{
			curHP -= ds.damage;
		}
		return curHP <= 0;
	}

    #region CollisionHandling
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

    void OnTriggerEnter2D(Collider2D col)
    {
		if (col.CompareTag("Rope"))
		{
			numRopesTouching++;

			if (numRopesTouching > 0)
			{
				myRigidBody.gravityScale = 0;
			}
		}
		else if (col.CompareTag("NoMagicArea"))
		{
			canUseMagic = false;
		}
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Rope"))
        {
            numRopesTouching--;

            if (numRopesTouching == 0)
            {
                myRigidBody.gravityScale = 1.5f;
            }
        } else if (col.CompareTag("NoMagicArea"))
		{
			canUseMagic = true;
		}
    }
    #endregion
    #region Gets
	/// <summary>
	/// Gets a value indicating whether this <see cref="T:Player"/> is on ground.
	/// </summary>
	/// <value><c>true</c> if is on ground; otherwise, <c>false</c>.</value>
    bool IsOnGround
	{
		get
		{
			Vector2 down = new Vector2(0, -Mathf.Sign(myRigidBody.gravityScale));
			return Physics2D.Raycast(transform.position, down, 1.0f, ~(1 << LayerMask.NameToLayer("Player")));
		}
	}

	
	/// <summary>
	/// Gets a value indicating whether this <see cref="T:Player"/> is on rope.
	/// </summary>
	/// <value><c>true</c> if is on rope; otherwise, <c>false</c>.</value>
	bool IsOnRope
	{
		get
		{
			return numRopesTouching > 0;
		}
	}

	/// <summary>
	/// Gets the height of the jump.
	/// </summary>
	/// <value>The height of the jump.</value>
	float JumpHeight
	{
		get
		{
			return Mathf.Pow(jumpImpulse, 2) / (Physics.gravity.y * myRigidBody.gravityScale * -2);
		}
	}

	/// <summary>
	/// Gets the allegiance.
	/// </summary>
	/// <value>The allegiance.</value>
	public Allegiance Allegiance
	{
		get
		{
			return Allegiance.Player;
		}
	}

	/// <summary>
	/// Gets half of the width.
	/// </summary>
	/// <value>The half of the width.</value>
	public float HalfWidth
	{
		get
		{
			return myRenderer.bounds.extents.x;
		}
	}

	/// <summary>
	/// Gets half of the height
	/// </summary>
	/// <value>Half of the height.</value>
	public float HalfHeight
	{
		get
		{
			return myRenderer.bounds.extents.y;
		}
	}

	/// <summary>
	/// Gets the percent of HP the player has.
	/// </summary>
	/// <value>The percent of HP the player has.</value>
	public float HPPercent
	{
		get
		{
			return (float)curHP / (float)maxHP;
		}
	}

	/// <summary>
	/// Gets the mp the player has.
	/// </summary>
	/// <value>The mp the player has.</value>
	public int MP
	{
		get
		{
			return curMP;
		}
	}

	/// <summary>
	/// Gets the percent of MP the player has.
	/// </summary>
	/// <value>The percent of MP the player has.</value>
	public float MPPercent
	{
		get
		{
			return (float)curMP / (float)maxMP;
		}
	}

	/// <summary>
	/// Gets the Player's forward vector.
	/// </summary>
	/// <value>The Player's forward vector.</value>
	public Vector3 Forward
	{
		get
		{
			return new Vector3(fwdX, 0, 0);
		}
	}

	/// <summary>
	/// Gets the Player's rigid body.
	/// </summary>
	/// <value>The Player's rigid body.</value>
	public Rigidbody2D RigidBody
	{
		get
		{
			return myRigidBody;
		}
	}

	/// <summary>
	/// Gets the game object this object is attached to.
	/// </summary>
	/// <value>The game object this is attached to.</value>
	public GameObject GameObject
	{
		get
		{
			return gameObject;
		}
	}

	/// <summary>
	/// Gets the position.
	/// </summary>
	/// <value>The position.</value>
	public Vector3 Position
	{
		get
		{
			return transform.position;
		}
	}

	/// <summary>
	/// Gets the name of the currently equipped spell.
	/// </summary>
	/// <value>The name of the equipped spell.</value>
	public string SpellName
	{
		get
		{
			return curSpell.SpellName;
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="T:Player"/> is hidden.
	/// </summary>
	/// <value><c>true</c> if hidden; otherwise, <c>false</c>.</value>
	public bool Hidden
	{
		get
		{
			return hidden;
		}

		set
		{
			hidden = value;
		}
	}
    #endregion gets
}
