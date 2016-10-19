using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, DamageableObject, CasterObject {

	private Controller controller;
	private Rigidbody2D myRigidBody;
	private SpriteRenderer myRenderer;

	private int fwdX = 1;
	public float maxSpeed = 1;
    public float sneakSpeed = 0.8f;
    public float jumpImpulse = 10;
	private float lastYVel = 0;
    private bool onRope = false;

	private int curHP = 0;
	public int maxHP = 100;

	private int curMP = 0;
	public int maxMP = 100;

	private Spell curSpell = new WaterSpell();

	private int numRopesTouching = 0;

    bool sneaking = false;
    bool hidden = false;
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
		if (!GameManager.Instance.IsPaused)
		{
            if (Input.GetKeyDown(KeyCode.Q))
            {
                GoStealth();
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                Hide();
            }
            if (hidden) //stops in place hiding
            {
                if(new Vector2(controller.Horizontal,controller.Vertical) != Vector2.zero)
                {
                    Material mat = GetComponent<SpriteRenderer>().material;

                    Color newColor = mat.color;
                    if (newColor.a != 1)
                    {

                        newColor.a = 0.7f;
                        mat.color = newColor;
                    }
                    hidden = false;
                }
            }
			Vector2 xForce = new Vector2(controller.Horizontal, 0) * 15;
			myRigidBody.AddForce(xForce, ForceMode2D.Force);
            if(!sneaking)
                myRigidBody.ClampVelocity(maxSpeed, VelocityType.X);
            else
                myRigidBody.ClampVelocity(sneakSpeed, VelocityType.X);

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
			}else if (IsOnGround)
			{
				if (controller.Jump)
				{
					Jump();
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
                myRenderer.flipX = (fwdX == -1);
			}
			if (controller.UseSpell && curMP >= curSpell.Cost)
			{
				SpellProjectile sp = curSpell.Cast(this);
				sp.allegiance = Allegiance.Player;
				curMP -= curSpell.Cost;
			}
			else if (controller.Attack)
			{
				SpellProjectile sp = new EarthSpell().Cast(this);
				sp.allegiance = Allegiance.Player;
			}
		}
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
		myRigidBody.AddForce(new Vector2(0, jumpImpulse), ForceMode2D.Impulse);
	}

	void FixedUpdate()
	{
		lastYVel = myRigidBody.velocity.y;
        if (sneaking)
            Debug.Log("Sneaking");
        if (hidden)
            Debug.Log("Hidden");
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
        }
    }
    #endregion
    #region Gets
    bool IsOnGround
	{
		get
		{
			return Physics2D.Raycast(transform.position, Vector2.down, 1.0f, ~(1 << LayerMask.NameToLayer("Player")));
		}
	}

	

	bool IsOnRope
	{
		get
		{
			return numRopesTouching > 0;
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

	public string SpellName
	{
		get
		{
			return curSpell.SpellName;
		}
	}
    #endregion gets


    #region abilites 
    /// <summary>
    /// Activates the player being stealthed. Probably will use ability scripts that are preferred.
    /// Certainly could be created within the player itself
    /// </summary>
    void GoStealth()
    {
        sneaking = true;
        Material mat = GetComponent<SpriteRenderer>().material;        
        Color newColor = mat.color;
        if (newColor.a == 1) {
            newColor.a = 0.7f;
            mat.color = newColor;
        }    
    }
    void Hide()
    {
        if (sneaking)
        {
            hidden = true;
            Material mat = GetComponent<SpriteRenderer>().material;
            Color newColor = mat.color;
            if (newColor.a != 1)
            {
                newColor.a = 0.4f;
                mat.color = newColor;
            }
        }
    }
    /// <summary>
    /// Throws a dagger, so far only one dagger, coated in a slowing poison.
    /// </summary>
    void ThrowDagger()
    {

    }
    #endregion
}
