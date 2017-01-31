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
    private Animator myAnimator;
    public Light myLight;
    private float lightBase = 4;
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

	public float curMP = 0;
	public float maxMP = 100;

    private int numRopesTouching = 0;
	PlayerState state = PlayerState.Normal;
    bool tryingToJump = false;
    public float lightOnPlayer;

    
    //Rose Makes Dust-------------------------------***
    [SerializeField]
    GameObject dustParticle;
    private float lastDustPart;
    //***-------------------------------------------***
    [SerializeField]
    GameObject startItemObject;
    UsableItem leftItem;
    public UsableItem rightItem;
    [SerializeField]
    List<GameObject> startInventory;
    List<UsableItem> inventory;
    private bool usedItem;

    InteractiveObject highlighted;
    Rigidbody2D highlightedBody;
   
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
        myAnimator = GetComponent<Animator>();
		curHP = maxHP;
		curMP = maxMP;
		DontDestroyOnLoad(gameObject);
        
        UIManager.Instance.UpdateUI(controller);
        inventory = new List<UsableItem>();
      
	}

    public void ResetBecauseINeed()
    {
        transform.position = startPos.position;
    }
	void FixedUpdate()
	{
        if (!GameManager.Instance.IsPaused)
        {
           
            curMP = Mathf.Min(curMP + Time.deltaTime * 5, maxHP);
            lastYVel = myRigidBody.velocity.y;

            if (!IsFrozen)
            {
                myRigidBody.AddForce(new Vector2(controller.Horizontal * 35, 0));
				myRigidBody.ClampVelocity((IsPushedHorizontallyByTheWind ? maxSpeed * 5: maxSpeed), VelocityType.X);
				myRigidBody.ClampVelocity(IsPushedVerticallyByTheWind ? 20 : 10, VelocityType.Y);

                if (IsOnGround)
                {
                    if (tryingToJump)
                    {
                        
                        Jump();
                    }
                    else
                    {
                        
						RaycastHit2D hit = Physics2D.BoxCast (transform.position, new Vector2 (1.0f, HalfHeight * 2), 0, Forward, 0.25f, 1 << LayerMask.NameToLayer("Interactive"));//Physics2D.Raycast(transform.position, Forward, 2.0f, (1 << LayerMask.NameToLayer("Interactive")));

                        if (hit.collider != null)
                        {
                            InteractiveObject io = hit.collider.GetComponent<InteractiveObject>();

							if (io != null) {
								if (controller.Interact) {
									io.Interact ();
								} else if (io != highlighted) {
									if (highlighted != null) {
										highlighted.Dehighlight ();
									}

									highlighted = io;
                               
									highlighted.Highlight ();
								}
							}
                            
                        } else if (highlighted != null)
                        {
                            highlighted.Dehighlight();
                            highlighted = null;
                        }
                        
                    }

					if (Mathf.Abs(controller.Horizontal/*myRigidBody.velocity.x*/) > float.Epsilon)
                    {
						fwdX = (int)Mathf.Sign(controller.Horizontal/*myRigidBody.velocity.x*/);
                        myRenderer.flipX = (fwdX == -1);
                    }

                }
            }
            else if (IsDashing && IsOnGround && tryingToJump)
            {
                Jump();
            } else if (IsPushing && highlightedBody)
            {
                if (Controller.Interact)
                {
                    highlighted.Interact();
                }
                else
                {
                    Vector2 blockMove = controller.InputDirection.XVector() * maxSpeed * Time.deltaTime / 2;
                    highlightedBody.Translate(blockMove);
                    myRigidBody.Translate(blockMove);
                }
            }
            
        }

        tryingToJump = false;
        
	}
	// Update is called once per frame
	void Update()
	{
		if (Controller.Pause)
		{
            GameManager.Instance.IsInMenu = !GameManager.Instance.IsInMenu;
		} else if (Controller.Jump)
        {
            tryingToJump = true;
        }

		if (!GameManager.Instance.IsPaused)
		{   
			if (!IsFrozen) {
				if (controller.Horizontal != 0) {
					myAnimator.SetBool ("FWD", true);
				} else {
					myAnimator.SetBool ("FWD", false);
				}
			}

            if (controller.PeerDown)
            {
                Camera.main.transform.position = (new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y-0.5f, Camera.main.transform.position.z));
            }
            else if (controller.PeerUp)
            {
                Camera.main.transform.position = (new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + 0.5f, Camera.main.transform.position.z));
            }
            else if(!controller.PeerUp && !controller.PeerDown)
            {
                //of course this wouldn't work
                Camera.main.transform.Rotate(new Vector3(0, 0, 0));
            }
           
            if (leftItem != null)
            {
				if (controller.ActivateLeftItem) {

					leftItem.Activate ();
				} else if (leftItem.Continuous && leftItem.IsActive) {
					if (controller.LeftItemDown) {
						leftItem.Use ();
					} else if (controller.DeactivateLeftItem) {
						leftItem.Deactivate ();
					}
				}
            }

            if (rightItem != null)
            {
               
				if (controller.ActivateRightItem) {
					rightItem.Activate ();
				} else if (rightItem.Continuous && rightItem.IsActive) {
					if (controller.RightItemDown) {
						rightItem.Use ();
					} else if (controller.DeactivateRightItem) {
						rightItem.Deactivate ();
					}
				}
            }
            
		}
        
	}

	void Jump()
	{
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

	#region CollisionHandling
	void OnCollisionEnter2D(Collision2D col)
	{
        if (!col.collider.CompareTag("Platform") && IsDashing)
        {
            IsDashing = false;
        }
		if (col.collider.CompareTag ("Platform")) {
			if (lastYVel < 0) {
				CanFloat = true;
				if (IsUsingMagnetGloves) {
					myRigidBody.velocity = Vector2.zero;
				}
			}
		} else if (col.collider.CompareTag ("Enemy")) {
            GameManager.Instance.Reset();
		} else if (col.collider.OnLayer("Metal") && IsUsingMagnetGloves)
        {
            if (lastYVel < 0)
            {
				(leftItem is PullGlove ? leftItem : rightItem).Deactivate();
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
		} else if (col.CompareTag("Spike"))
        {
            GameManager.Instance.Reset();
        }
        if (col.CompareTag("Projectile"))
        {
            if (IsUsingReflectCape)
            {
                Reflect(col.gameObject);
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
   
    void Reflect(GameObject proj)
    {
		Rigidbody2D rb = proj.GetComponent<Rigidbody2D> ();
		//rb.AddForce (new Vector2 (fwdX * 5, 0), ForceMode2D.Impulse);
		Vector2 vel = rb.velocity;
		vel = vel.Rotated (-vel.GetAngle ());

		rb.velocity = vel.Rotated (TrueAim.GetAngle ());

		proj.transform.position = transform.position + (Vector3)(TrueAim.normalized * (HalfWidth + 1));

		//rb.velocity = new Vector2 (fwdX * 5, 0);
    }
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
			LayerMask mask = 1 << LayerMask.NameToLayer("Platforms");
			LayerMask mask2 = 1 << LayerMask.NameToLayer("Metal");
			int finalMask = mask | mask2;
            Vector2 down = new Vector2(0, -Mathf.Sign(myRigidBody.gravityScale));
			RaycastHit2D hit = Physics2D.BoxCast (transform.position, new Vector2 (HalfWidth, HalfHeight + 0.1f), 0, down, 1.1f, finalMask); 
            
			return hit.collider != null;
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

		set {
			curMP = value;
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
	/// Gets the inventory.
	/// </summary>
	/// <value>The inventory.</value>
    public List<UsableItem> Inventory
    {
        get
        {
            return inventory;
        }
    }

	/// <summary>
	/// Gets the sprite for the left item.
	/// </summary>
	/// <value>The left item's sprite.  Null otherwise.</value>
    public Sprite LeftItem
    {
        get
        {
            return leftItem ? leftItem.uiSprite : null;
        }
    }

	/// <summary>
	/// Gets the sprite for the right item.
	/// </summary>
	/// <value>The right item's sprite if there is one.  Null otherwise.</value>
    public Sprite RightItem
    {
        get
        {
            return rightItem ? rightItem.uiSprite : null;
        }
    }

	/// <summary>
	/// Adds the item to the player's inventory.
	/// </summary>
	/// <param name="go">The GameObjet containing the item.</param>
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
            UIManager.Instance.StartCoroutine(UIManager.Instance.ShowFoundItemMessage(ui.description));
            
        }
    }

    /// <summary>
    /// Returns the direction the Player is aiming.  If there's no vertical component, it assumes the player is just aiming forward.
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
	/// Gets the true aim.  Unlike aiming, it doesn't make an assumption about the player aiming forward.
	/// </summary>
	/// <value>The true aim.</value>
	public Vector2 TrueAim {
		get
		{
			Vector2 aim = new Vector2 (controller.Horizontal, controller.Vertical);
			if (Vector2.Equals (aim, Vector2.zero)) {
				return new Vector2(fwdX, 0);
			}

			return aim;
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
				myRigidBody.velocity = new Vector2 (0f, myRigidBody.velocity.y);
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

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="T:Player"/> is pushing.
	/// </summary>
	/// <value><c>true</c> if is pushing; otherwise, <c>false</c>.</value>
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
                UIManager.Instance.ShowInteraction("Stop");
                state |= PlayerState.Pushing;
                state |= PlayerState.Frozen;
                highlightedBody = (highlighted as BlockController).GetComponent<Rigidbody2D>();
            }
            else
            {
                state &= ~PlayerState.Pushing;
                state &= ~PlayerState.Frozen;
            }
        }
    }

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="T:Player"/> is using reflect cape.
	/// </summary>
	/// <value><c>true</c> if is using reflect cape; otherwise, <c>false</c>.</value>
    public bool IsUsingReflectCape
    {
        get
        {
			return (state & PlayerState.UsingReflectCape) > 0;;
        }
        set
        {
			if (value && !IsUsingReflectCape && !IsFrozen)
			{
				myRenderer.material.color = Color.yellow;
				state |= PlayerState.UsingReflectCape;

				if (!IsOnGround && CanFloat) {
					CanFloat = false;
					myRigidBody.velocity = myRigidBody.velocity.XVector();
					myRigidBody.gravityScale = 0;//0.75f;
					TimerManager.Instance.AddTimer(new Timer(()=>{StopFloat();}, 1.0f));
				} else {
					state |= PlayerState.Frozen;
				}
			}
			else
			{
				myRenderer.material.color = Color.white;
				state &= ~PlayerState.UsingReflectCape;
				state &= ~PlayerState.Frozen;
				myRigidBody.gravityScale = 1.5f;
			}
        }
    }

	void StopFloat() {
		myRigidBody.gravityScale = 1.5f;
	}
	bool CanFloat {
		get {
			return (state & PlayerState.CanFloat) > 0;
		}

		set {
			if (value) {
				state |= PlayerState.CanFloat;
			} else {
				state &= ~PlayerState.CanFloat;
			}
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="T:Player"/> is using magnet gloves.
	/// </summary>
	/// <value><c>true</c> if is using magnet gloves; otherwise, <c>false</c>.</value>
    public bool IsUsingMagnetGloves
    {
        get
        {
            return (state & PlayerState.UsingMagnetGloves) > 0;
        }

        set
        {
            if (value && !IsUsingMagnetGloves && !IsFrozen)
            {
                state |= PlayerState.UsingMagnetGloves;
                state |= PlayerState.Frozen;
            }
            else
            {
                state &= ~PlayerState.UsingMagnetGloves;
                state &= ~PlayerState.Frozen;
            }
        }
    }

	public bool IsPushedVerticallyByTheWind {
		get {
			return (state & PlayerState.PushedByTheWindVert) > 0;
		}

		set {
			if (value) {
				state |= PlayerState.PushedByTheWindVert;
			} else {
				state &= ~PlayerState.PushedByTheWindVert;
			}
		}
	}

	public bool IsPushedHorizontallyByTheWind {
		get {
			return (state & PlayerState.PushedByTheWindHorz) > 0;
		}

		set {
			if (value) {
				state |= PlayerState.PushedByTheWindHorz;
			} else {
				state &= ~PlayerState.PushedByTheWindHorz;
			}
		}
	}

    #endregion gets

    void Unfreeze() {
		IsFrozen = false;
	}
   
	/// <summary>
	/// Equips the item.
	/// </summary>
	/// <param name="itemNum">Item number.</param>
	/// <param name="left">If set to <c>true</c> left.</param>
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

	/// <summary>
	/// Reset this instance.
	/// </summary>
    public override void Reset()
    {
		if (leftItem && leftItem.IsActive) { 
			leftItem.Deactivate ();
		}

		if (rightItem && rightItem.IsActive) {
			rightItem.Deactivate ();
		}
		myRigidBody.gravityScale = 1.5f;
		myRenderer.material.color = Color.white;
		state = PlayerState.Normal;
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
    Pushing = 8,
    UsingMagnetGloves = 16,
	UsingReflectCape = 32,
	CanFloat = 64,
	PushedByTheWindHorz = 128,
	PushedByTheWindVert = 256
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