using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour, DamageableObject, CasterObject
{
    private Transform startPos;
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

	private float curMP = 0;
	public float maxMP = 100;
   
	int curSpell = 0;
    [SerializeField]
    public List<Spell> spells = new List<Spell>();

    private int numRopesTouching = 0;
	PlayerState state = PlayerState.Normal;

    public float lightOnPlayer;
    void Awake()
    {

        startPos = transform;
        Debug.Log("Start Pos is " + startPos.position);
    }
	// Use this for initialization
	void Start()
	{
        startPos = transform;
		controller = new Controller();
		myRigidBody = GetComponent<Rigidbody2D>();
		myRenderer = GetComponent<SpriteRenderer>();
		curHP = maxHP;
		curMP = maxMP;
        spells.Add(new FireSpell());
        spells.Add(new WaterSpell());
        spells.Add(new WindSpell());
        UIManager.Instance.ShowSpell = true;
        UIManager.Instance.LightLevel = 0;
		DontDestroyOnLoad(gameObject);
	}

    public void ResetBecauseINeed()
    {
        Debug.Log("StartPos is " + startPos.position);
        transform.position = startPos.position;
    }
	void FixedUpdate()
	{
        if (!GameManager.Instance.IsPaused)
        {
            lightOnPlayer = GetLocalLightLevel();
            curMP = Mathf.Min(curMP + Time.deltaTime * 5, maxHP);
            lastYVel = myRigidBody.velocity.y;

            {
                if (!Hidden)
                {
                    myRigidBody.AddForce(new Vector2(controller.Horizontal * 35, 0));


                    if (controller.Sneak)
                        myRigidBody.ClampVelocity(sneakSpeed, VelocityType.X);
                    else
                        myRigidBody.ClampVelocity(maxSpeed, VelocityType.X);

                    if (IsOnRope)
                    {
                        Vector2 vel = myRigidBody.velocity;
                        //vel.x = 0;
                        vel.y = controller.Vertical * maxSpeed;
                        myRigidBody.velocity = vel;
                        //myRigidBody.AddForce(new Vector2(0, controller.Vertical) * 35);
                        //myRigidBody.ClampVelocity(maxSpeed, VelocityType.Y);

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
                        else
                        {
                            if (controller.Interact)
                            {
                                RaycastHit2D hit = Physics2D.Raycast(transform.position, Forward, 2.0f, (1 << LayerMask.NameToLayer("SpellStatue")));
                               
                                if (hit.collider != null)
                                {
                                   // Debug.Log("Found" + hit.collider.gameObject.name);
                                    InteractiveObject io = hit.collider.GetComponent<InteractiveObject>();

                                    if (io != null)
                                    {
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


                    UIManager.Instance.LightLevel = GetLocalLightLevel();

                }
                else
                {
                    myRigidBody.velocity = Vector2.zero;
                    if (controller.Interact)
                    {
                        Hidden = false;
                    }

                    UIManager.Instance.LightLevel = 0;
                }
                //CameraManager.Instance.Velocity = myRigidBody.velocity;
            }
        }
	}
	// Update is called once per frame
	void Update()
	{
		if (Controller.Pause)
		{
			GameManager.Instance.IsPaused = !GameManager.Instance.IsPaused;
		}

		if (!GameManager.Instance.IsPaused)
		{
			if (spells.Count > 0)
			{
				CurrentSpell += controller.SpellChange;
			}
			if (!Hidden)
			{
				if (CanUseMagic && controller.UseSpell && curMP >= CurSpell.Cost)
				{
					SpellProjectile sp = CurSpell.Cast(this);
					OnCooldown = true;
					Invoke("SpellCooldown", 1.0f);
					sp.allegiance = Allegiance.Player;
					curMP -= CurSpell.Cost;
				}
				UIManager.Instance.LightLevel = GetLocalLightLevel();
			}
		}
        
	}

	public void HandleSpellAxisCooldownForController(float t)
	{
		Invoke("SpellAxisCooldown", t);
	}

	void SpellAxisCooldown()
	{
		controller.UnfreezeSpellAxis();
	}
	void SpellCooldown()
	{
		OnCooldown = false;
	}

	public void AddSpell(Spell s)
	{
		if (!spells.Contains(s))
		{
            Debug.Log("Spell: " + s);
			spells.Add(s);

			CurrentSpell = spells.Count - 1;
			UIManager.Instance.ShowSpell = true;
		}
	}

	public int SpellsKnown
	{
		get
		{
			return spells.Count;
		}
	}
	void Jump()
	{
		foreach (RaycastHit2D hit in Physics2D.RaycastAll(transform.position, Vector3.up, JumpHeight, 1 << LayerMask.NameToLayer("Platforms")))
		{
			PlatformObject po = hit.collider.GetComponent<PlatformObject>();
			if (po != null && po.passThrough)
			{
				po.AllowPassThrough();
			}
		}
		myRigidBody.AddForce(new Vector2(0, jumpImpulse * Mathf.Sign(myRigidBody.gravityScale)), ForceMode2D.Impulse);
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

	/// <summary>
	/// Gets the closest light value within maxDistance.
	/// </summary>
	float GetLocalLightLevel(float maxDistance = 20f)
	{
		float lowestDist = maxDistance; //assume that the furthest light is optDist awawy

		if (!Hidden)
		{
			//this means each light on an object has to be a seperate gameobject and be correctly layered. Cool.
			Collider2D[] cols = Physics2D.OverlapCircleAll(this.transform.position, maxDistance, 1 << LayerMask.NameToLayer("Light"));
			//each collider that was hit, we should mask
			//mask'd

			foreach (Collider2D col in cols)
			{
				//Debug.Log("Col is : " + col);
				//Better than two v2Distance calls
				Vector3 dif = col.transform.position - transform.position;
				float tempD = dif.magnitude;
				if (tempD < lowestDist)
				{
					if (!Physics2D.Raycast(transform.position, dif.normalized, tempD, 1 << LayerMask.NameToLayer("Platforms")))
					{
						lowestDist = tempD;
					}
				}

			}
			// Debug.Log("Light level is : " + lowestDist/10);
		}
		return 1 - lowestDist / maxDistance;
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
			InNoMagicArea = false;
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
		else if (col.CompareTag("NoMagicArea"))
		{
			InNoMagicArea = true;
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
			return Physics2D.Raycast(transform.position, down, 1.0f, (1 << LayerMask.NameToLayer("Platforms")));
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
	public float MP
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
			return curMP / maxMP;
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
			return CurSpell.SpellName;
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
			return (state & PlayerState.InCover) > 0;
		}

		set
		{
			if (value)
			{
				state |= PlayerState.InCover;
			}
			else {
				state &= ~PlayerState.InCover;
			}

			GetComponent<Collider2D>().isTrigger = value;
			myRigidBody.gravityScale = value ? 0 : 1;
		}
	}

	/// <summary>
	/// Gets the controller.
	/// </summary>
	/// <value>The controller.</value>
	public Controller Controller
	{
		get
		{
			return controller;
		}
	}

	/// <summary>
	/// Gets a value indicating whether this <see cref="T:Player"/> on cooldown.
	/// </summary>
	/// <value><c>true</c> if on cooldown; otherwise, <c>false</c>.</value>
	public bool OnCooldown
	{
		get
		{
			return (state & PlayerState.MagicCooldown) > 0;
		}

		private set
		{
			if (value)
			{
				state |= PlayerState.MagicCooldown;
			}
			else {
				state &= ~PlayerState.MagicCooldown;
			}
		}
	}

	/// <summary>
	/// Gets a value indicating whether this <see cref="T:Player"/> in no magic area.
	/// </summary>
	/// <value><c>true</c> if in no magic area; otherwise, <c>false</c>.</value>
	public bool InNoMagicArea
	{
		get
		{
			return (state & PlayerState.InNoMagicArea) > 0;
		}

		private set
		{
			if (value)
			{
				state |= PlayerState.InNoMagicArea;
			}
			else {
				state &= ~PlayerState.InNoMagicArea;
			}
		}
	}

	/// <summary>
	/// Gets a value indicating whether this <see cref="T:Player"/> can use magic.
	/// </summary>
	/// <value><c>true</c> if can use magic; otherwise, <c>false</c>.</value>
	public bool CanUseMagic
	{
		get
		{
			return !(OnCooldown || InNoMagicArea);
		}
	}

	/// <summary>
	/// Gets or sets the current spell.
	/// </summary>
	/// <value>The current spell.</value>
	public int CurrentSpell
	{
		get
		{
			return curSpell;
		}

		set
		{
			curSpell = value % spells.Count;

			while (curSpell < 0)
			{
				curSpell += spells.Count;
			}

			UIManager.Instance.UpdateSpellInfo();
		}
	}

    public bool IsFrozen
    {
        get
        {
            if (state ==  PlayerState.Frozen)
            {
                return true;
            }else
            {
                return false;
            }
        }
        set
        {
            if (value)
            {
                state = PlayerState.Frozen;
            }else
            {
                state &= ~PlayerState.Frozen;
            }
        }
    }

	/// <summary>
	/// Gets the current spell.
	/// </summary>
	/// <value>The current spell.</value>
	public Spell CurSpell
	{
		get
		{
			return spells[CurrentSpell];
		}
	}
	#endregion gets

}

[System.Flags]
public enum PlayerState
{
	Normal = 0,
	InCover = 1,
	MagicCooldown = 2,
	InNoMagicArea = 4,
	Frozen = 8
}