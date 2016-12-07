using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
public class Player : ResettableObject, DamageableObject, CasterObject
{
    private Transform startPos;
	private Controller controller;
	private Rigidbody2D myRigidBody;
	private SpriteRenderer myRenderer;

    private MagicState mState = MagicState.Stun;
    private ArmorState aState = ArmorState.Base;
	private int fwdX = 1;
	public float maxSpeed = 1;
	public float sneakSpeed = 0.5f;
	public float jumpImpulse = 10;
	private float lastYVel = 0;
	private bool onRope = false;
    private int jumpsInAir;

	private int curHP = 0;
	public int maxHP = 100;

	private float curMP = 0;
	public float maxMP = 100;
   
	int curSpell = 0;

    private int numRopesTouching = 0;
	PlayerState state = PlayerState.Normal;

    public float lightOnPlayer;

    //Rose Makes Dust-------------------------------***
    [SerializeField]
    GameObject dustParticle;
    private float lastDustPart;
    //***-------------------------------------------***
    [SerializeField]
    GameObject startItemObject;
    UsableItem leftItem;
    UsableItem rightItem;
    [SerializeField]
    List<GameObject> startInventory;
    List<UsableItem> inventory;

    InteractiveObject highlighted;
    void Awake()
    {
        startPos = transform;
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
        //UIManager.Instance.LightLevel = 0;
		DontDestroyOnLoad(gameObject);
        GameObject item = Instantiate(startItemObject);
        item.transform.SetParent(transform);
        leftItem = item.GetComponent<UsableItem>();
        UIManager.Instance.UpdateUI(controller);
        inventory = new List<UsableItem>();
        foreach (GameObject go in startInventory)
        {
            AddItem(go);
        }
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
            //lightOnPlayer = GetLocalLightLevel();
            curMP = Mathf.Min(curMP + Time.deltaTime * 5, maxHP);
            lastYVel = myRigidBody.velocity.y;

            if (!(Hidden || IsFrozen))
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
                        
                        RaycastHit2D hit = Physics2D.Raycast(transform.position, Forward, 2.0f, (1 << LayerMask.NameToLayer("Interactive")));

                        if (hit.collider != null)
                        {
                            // Debug.Log("Found" + hit.collider.gameObject.name);
                            InteractiveObject io = hit.collider.GetComponent<InteractiveObject>();

                            if (controller.Interact)
                            {
                                io.Interact();
                            } else if (io != highlighted)
                            {
                                if (highlighted != null)
                                {
                                    highlighted.Dehighlight();
                                }

                                highlighted = io;
                                highlighted.Highlight();
                            }
                            
                        } else if (highlighted != null)
                        {
                            highlighted.Dehighlight();
                            highlighted = null;
                        }
                        
                    }

                    if (Mathf.Abs(myRigidBody.velocity.x) > float.Epsilon)
                    {
                        fwdX = (int)Mathf.Sign(myRigidBody.velocity.x);
                        myRenderer.flipX = (fwdX == -1);
                    }


                   //UIManager.Instance.LightLevel = GetLocalLightLevel();

                }
                //CameraManager.Instance.Velocity = myRigidBody.velocity;
            }
            else if (Hidden && !IsFrozen)
            {
                myRigidBody.velocity = Vector2.zero;
                if (controller.Interact)
                {
                    Hidden = false;
                }

                //UIManager.Instance.LightLevel = 0;
            }
            else if (IsDashing && IsOnGround && controller.Jump)
            {
                Jump();
            } else if (IsPushing)
            {
                UsableItem magGloves = leftItem is MagnetGloves ? leftItem : rightItem;
                if (leftItem == magGloves ? controller.LeftItemDown : controller.RightItemDown)
                {
                    (magGloves as MagnetGloves).Use();
                }
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
			if (!Hidden)
			{
                //UIManager.Instance.LightLevel = GetLocalLightLevel();
                if (leftItem != null)
                {
                    if (controller.ActivateLeftItem)
                    {
                        leftItem.Activate();
                    }
                    else if (controller.DeactivateLeftItem)
                    {
                        leftItem.Deactivate();
                    }
                }

                if (rightItem != null)
                {
                    if (controller.ActivateRightItem)
                    {
                        rightItem.Activate();
                    }
                    else if (controller.DeactivateRightItem)
                    {
                        rightItem.Deactivate();
                    }
                }
            }
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
        float ji = IsDashing ? jumpImpulse * 1.5f : jumpImpulse;
		myRigidBody.AddForce(new Vector2(0, ji * Mathf.Sign(myRigidBody.gravityScale)), ForceMode2D.Impulse);
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
        if (!col.collider.CompareTag("Platform") && IsDashing)
        {
            IsDashing = false;
        }
		if (col.collider.CompareTag ("Platform")) {

			if (lastYVel < -10) {
                GameManager.Instance.Reset();
			} else if (lastYVel < 0 && IsPushing)
            {
                myRigidBody.velocity = Vector2.zero;
            }
		} else if (col.collider.CompareTag ("Enemy")) {
            GameManager.Instance.Reset();
		} else if (col.collider.OnLayer("Metal") && IsPushing)
        {
            if (lastYVel < 0)
            {
                (leftItem is MagnetGloves ? leftItem : rightItem).Deactivate();
                myRigidBody.velocity = Vector2.zero;
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
   

    void CreateDustParticle(Vector2 posOfParticle)
    {
        lastDustPart = Time.time;
        //Transform tempT = new GameObject().transform; //this doesn't feel right;
        //tempT.position = posOfParticle;
        GameObject newPart = (GameObject)Instantiate(dustParticle, transform);
        newPart.transform.position = posOfParticle;
        newPart.transform.parent = null;
    }
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
            if (Physics2D.Raycast(transform.position, down, HalfHeight + 0.1f, (1 << LayerMask.NameToLayer("Platforms"))))
            { //Straight down
                return true;
            }
            if (Physics2D.Raycast(transform.position - new Vector3(HalfWidth, 0), down, HalfHeight + 0.1f, (1 << LayerMask.NameToLayer("Platforms"))))
            { //backwards
                return true;
            }
            if (Physics2D.Raycast(transform.position + new Vector3(HalfWidth, 0), down, HalfHeight + 0.1f, (1 << LayerMask.NameToLayer("Platforms"))))
            {//forwards
                return true;
            }
            bool exists = Enum.IsDefined(typeof(MagicState), 2);      // exists = true
            if (HasFlag(mState,MagicState.WallJump))
            {
                Debug.Log("I've done it!");

                if (Physics2D.Raycast(transform.position, Vector2.right, HalfWidth + 0.4f, (1 << LayerMask.NameToLayer("Wall")))) //Right
                {
                    if (Time.time - lastDustPart >= 0.2f)
                        CreateDustParticle(new Vector2(transform.position.x + 0.4f, transform.position.y));
                    return true;
                }
                if (Physics2D.Raycast(transform.position, -Vector2.right, HalfWidth - 0.4f, (1 << LayerMask.NameToLayer("Wall")))) //Right
                {
                    if (Time.time - lastDustPart >= 0.2f)
                        CreateDustParticle(new Vector2(transform.position.x - 0.4f, transform.position.y));
                    return true;

                }

            }		
        return false;
		} // end Get
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

    public List<UsableItem> Inventory
    {
        get
        {
            return inventory;
        }
    }

    public Sprite LeftItem
    {
        get
        {
            return leftItem ? leftItem.uiSprite : null;
        }
    }

    public Sprite RightItem
    {
        get
        {
            return rightItem ? rightItem.uiSprite : null;
        }
    }

    public void AddItem(GameObject go)
    {
        if (go.GetComponent<UsableItem>() != null)
        {
            GameObject g = Instantiate(go);
            g.transform.SetParent(transform);
            UsableItem ui = g.GetComponent<UsableItem>();
            if (leftItem == null)
            {
                leftItem = ui;
                UIManager.Instance.UpdateUI();
            } else if (rightItem == null)
            {
                rightItem = ui;
                UIManager.Instance.UpdateUI();
            } else
            {
                inventory.Add(ui);
            }
            
        }
    }

    /// <summary>
    /// Returns the direction the Player is aiming
    /// </summary>
    public Vector2 Aiming
    {
        get
        {
            int vert = controller.Vertical;

            if (vert == 0)
            {
                return new Vector2(fwdX, 0);
            }
         
            return new Vector2(controller.Horizontal, vert);
        }
    }
    /// <summary>
    /// Getter and setter for whether or not the Player is able to move
    /// </summary>
    public bool IsFrozen
    {
        get
        {
            return (state & PlayerState.Frozen) > 0;
        }
        set
        {
            if (value)
            {
                state |= PlayerState.Frozen;
            }else
            {
                state &= ~PlayerState.Frozen;
            }
        }
    }

    /// <summary>
    /// Getter and setter for if the player is using the Dash Boots
    /// </summary>
    public bool IsDashing
    {
        get
        {
            return (state & PlayerState.Dashing) > 0;
        }
        set
        {
            if (value && !IsDashing && !IsFrozen)
            {
                state |= PlayerState.Dashing;
                state |= PlayerState.Frozen;
                myRigidBody.AddForce(new Vector2(fwdX * maxSpeed * 10, 0), ForceMode2D.Impulse);
            }
            else
            {
                UsableItem curItem = leftItem is DashBoots ? leftItem : rightItem;
                curItem.Deactivate();
                state &= ~PlayerState.Dashing;
                state &= ~PlayerState.Frozen;
            }
        }
    }

    public bool IsPushing
    {
        get
        {
            return (state & PlayerState.Pushing) > 0;
        }

        set
        {
            if (value && !IsPushing && !IsFrozen)
            {
                state |= PlayerState.Pushing;
                state |= PlayerState.Frozen;
            }
            else
            {
                state &= ~PlayerState.Pushing;
                state &= ~PlayerState.Frozen;
            }
        }
    }

	#endregion gets

	void Unfreeze() {
		IsFrozen = false;
	}
    #region UpgradeRegion
    public void UnlockMagicWand()
    {
        mState = UnsetFlag(mState, MagicState.NoMagic);
        mState = SetFlag(mState, MagicState.Range);
        
    }
    public void ArmorUp()
    {
        Debug.Log("I upgraded my armor");
        
    }

    public void UnlockDoubleJump()
    {
        //Unlocks Double Jump
        mState = SetFlag(mState, MagicState.DJump);
    }
    public void UnlockWallJump()
    {
        mState = SetFlag(mState, MagicState.WallJump);
        Debug.Log("Play WJump? " + HasFlag(mState, MagicState.WallJump));
    }
    #endregion
    #region flags
    public static MagicState SetFlag(MagicState a, MagicState b)
    {
        //a |= b;
        return a |= b;
    }

    public static MagicState UnsetFlag(MagicState a, MagicState b)
    {
        return a & (~b);
    }

    // Works with "None" as well
    public static bool HasFlag(MagicState a, MagicState b)
    {
        return (a & b) == b;
    }

    public static MagicState ToggleFlag(MagicState a, MagicState b)
    {
        return a ^ b;
    }

    public static ArmorState ASetFlag(ArmorState a, ArmorState b)
    {
        //a |= b;
        return a | b;
    }

    public static ArmorState AUnsetFlag(ArmorState a, ArmorState b)
    {
        return a & (~b);
    }

    // Works with "None" as well
    public static bool AHasFlag(ArmorState a, ArmorState b)
    {
        return (a & b) == b;
    }

    public static ArmorState AToggleFlag(ArmorState a, ArmorState b)
    {
        return a ^ b;
    }
    #endregion flags

    public void EquipItem(int itemNum, bool left)
    {
        if (itemNum < inventory.Count)
        {
            UsableItem oldItem;
            if (left)
            {
                oldItem = leftItem;
                leftItem = inventory[itemNum];
            } else
            {
                oldItem = rightItem;
                rightItem = inventory[itemNum];
            }

            inventory[itemNum] = oldItem;
            
            UIManager.Instance.UpdateUI();
        }
    }

    public override void Reset()
    {
        myRigidBody.velocity = Vector2.zero;
        transform.position = Checkpoint.ActiveCheckpointPosition;
        curHP = maxHP;
        curMP = maxMP;
    }
}


[System.Flags]
public enum PlayerState
{
	Normal = 0,
	InCover = 1,
	Dashing = 2,
	Frozen = 4,
    Pushing = 8
}

[System.Flags]
public enum ArmorState
{
    //How many hits it can take I suppose
    Base = 0,
    One = 1,
    Two = 2   
}
[System.Flags]
public enum MagicState
{
    NoMagic = 0,
    Range = 1,
    WallJump = 2,
    DJump = 4,
    Stun = 8,
    Magnet = 16
}