using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
public class Player : ResettableObject, CasterObject, ReflectiveObject
{
	private Controller controller;
	private Rigidbody2D myRigidBody;
	private SpriteRenderer myRenderer;
    private Animator myAnimator;

	private int fwdX = 1;
	public float maxSpeed = 1;
	public float sneakSpeed = 0.5f;
	public float jumpImpulse = 10;
	private float lastYVel = 0;

    [SerializeField]
	PlayerState state = PlayerState.Normal;
    bool tryingToJump = false;
	bool tryingToInteract = false;
    public float lightOnPlayer;

    [SerializeField]
    GameObject startItemObject;
    UsableItem leftItem;
    UsableItem rightItem;
    [SerializeField]
    List<GameObject> startInventory;
    List<UsableItem> inventory;
    private bool usedItem;


    //InteractiveObject highlighted;
    Rigidbody2D highlightedBody;
	//bool collidingWithHighlighted = false;

	float interactDistance = 2.0f;

	Timer resetTimer;
	//Sound Effects
	[SerializeField]
	AudioClip jumpClip;
	[SerializeField]
	AudioClip spikeDeathClip;

    Sprite basicSprite;
    [SerializeField]
    Sprite[] capeSprites;
    [SerializeField]
    Sprite[] magPushGloves;
    [SerializeField]
    Sprite[] magPullGloves;
    SpriteRenderer arrowRenderer;
	SpriteRenderer rangeRenderer;
	GameManager manager;
    void Awake()
    {
		arrowRenderer = GetComponentsInChildren<SpriteRenderer> ()[1];
		arrowRenderer.enabled = false;
		rangeRenderer = GetComponentsInChildren<SpriteRenderer> ()[2];
		rangeRenderer.enabled = false;
		manager = GameManager.Instance;


    }
	// Use this for initialization
	void Start()
	{
		controller = new Controller();
		myRigidBody = GetComponent<Rigidbody2D>();
		myRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
		DontDestroyOnLoad(gameObject);
        basicSprite = myRenderer.sprite;
        UIManager.Instance.UpdateUI(controller);
        inventory = new List<UsableItem>();

		if (!startInventory.IsEmpty ()) {
			foreach (GameObject go in startInventory) {
				AddItem (go);
			}
		}

		resetTimer = new Timer (() => {
			manager.Reset();
		}, 0.33f);
	}

	void FixedUpdate()
	{
        if (!manager.IsPaused)
        {
            lastYVel = myRigidBody.velocity.y;

            if (!IsFrozen)
			{
				
				if (IsClimbing)
                {
					Vector2 vel = myRigidBody.velocity;
					vel.x = 0;
					myRigidBody.velocity = vel;
					float vert = controller.Vertical;
					if (vert != 0)
                    {
						myRigidBody.AddForce (new Vector2 (0, controller.Vertical * 10));
						myRigidBody.ClampVelocity (5, VelocityType.Y);
					}
                    else
                    {
						myRigidBody.velocity = Vector2.zero;
					}


					if (controller.AltJump) {
						IsClimbing = false;
						Jump ();
						return;
					}
					if (tryingToInteract) {
						IsClimbing = false;
						Input.ResetInputAxes ();
					}
				}//end climbing
                else
                {
					myRigidBody.AddForce (new Vector2 (controller.Horizontal * 35, 0));
					myRigidBody.ClampVelocity ((IsPushedHorizontallyByTheWind ? maxSpeed * 5 : maxSpeed), VelocityType.X);
					myRigidBody.ClampVelocity (IsPushedVerticallyByTheWind ? 20 : 10, VelocityType.Y);

					if (IsOnGround) {
						if (tryingToJump) {
                        
							Jump ();
						} 

						if (Mathf.Abs (controller.Horizontal) > float.Epsilon) {
							fwdX = (int)Mathf.Sign (controller.Horizontal);
							myRenderer.flipX = (fwdX == 1);
						}
					}

				} 
			
            }
            else if (IsDashing && IsOnGround && tryingToJump)
            {
                Jump();
            } else if (IsPushing && highlightedBody)
            {
                Vector2 input = controller.InputDirection.XVector();
                if (input.x / fwdX < 0)
                {
                    Vector3 move = new Vector3(-fwdX * maxSpeed * Time.deltaTime / 2, 0);
                    RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(1, 1), 0, new Vector2(-fwdX, 0), move.x);

                    if (hit)
                    {
                        move.x = hit.distance * -fwdX;
                    }
                    transform.position += move;
                    highlightedBody.GetComponent<BlockController>().Move(move);
                }
                else
                {
                    Vector2 blockMove = input * maxSpeed * Time.deltaTime / 2;

                    Vector3 move = highlightedBody.GetComponent<BlockController>().Move(blockMove);
                    move.x -= fwdX * (HalfWidth + highlightedBody.gameObject.HalfWidth());
                    move.y = transform.position.y;
                    transform.position = move;
                }
          
                Vector2 vel = highlightedBody.velocity;
				vel.x = 0;
				highlightedBody.velocity = vel;
            }
            
        }

        tryingToJump = false;
		tryingToInteract = false;
        
	}
		
	// Update is called once per frame
	void Update()
	{
		if (Controller.Pause)
		{
            manager.IsInMenu = !manager.IsInMenu;
		}
		else if (Controller.Jump)
        {
            tryingToJump = true;
        }
        else if (Controller.Reset)
        {
            // IsDead = true;
			SceneManager.LoadScene("TitleScreen");
        }
        else if (Controller.Restart)
        {
            // IsDead = true;
            GameManager.Instance.Reset();
        }
		if (!manager.IsPaused) {
			tryingToInteract = controller.Interact;
           
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

			if (ShowAimArrow) {
				arrowRenderer.transform.rotation = Quaternion.AngleAxis (MouseAim.GetAngle ().ToDegrees(), Vector3.forward);
			}
            
		}
        
	}

    void LateUpdate()
    {
        //Awful code, but it works
        if (IsUsingMagnetGloves)
        {
          //  myAnimator.SetBool("CapeUse", false);
            myAnimator.SetBool("PullUse",true);
            switch ((int)MouseAim.x)
            {
                case 0:
                    switch ((int)MouseAim.y)
                    {
                        case 1: //up
                            myAnimator.SetInteger("Direction", 1);
                            // myRenderer.sprite = magPullGloves[2];
                            break;
                        case 0: //forward
                            if (fwdX == 1)
                            {
                                myAnimator.SetInteger("Direction", 3);
                                // myRenderer.sprite = magPullGloves[4];
                            }
                            else
                            {
                                myAnimator.SetInteger("Direction", 7);
                                // myRenderer.sprite = magPullGloves[0];
                            }
                            break;
                        case -1: //down
                            myAnimator.SetInteger("Direction", 5);
                            // myRenderer.sprite = magPullGloves[6];
                            break;
                    }//end of internal Switch
                    break; // end of x = 0
                case 1:
                    switch ((int)MouseAim.y)
                    {
                        case 1: //up
                            myAnimator.SetInteger("Direction", 2);
                            //myRenderer.sprite = magPullGloves[3];
                            break;
                        case 0: //forward
                            if (fwdX == 1)
                            {
                                myAnimator.SetInteger("Direction", 3);
                                // myRenderer.sprite = magPullGloves[4];
                            }
                            else
                            {
                                myAnimator.SetInteger("Direction", 7);
                                // myRenderer.sprite = magPullGloves[0];
                            }
                            break;
                        case -1: //down
                            myAnimator.SetInteger("Direction", 4);
                            // myRenderer.sprite = magPullGloves[5];
                            break;
                    }//end of internal Switch
                    break;
                case -1:
                    switch ((int)MouseAim.y)
                    {
                        case 1: //up
                            myAnimator.SetInteger("Direction", 8);
                            // myRenderer.sprite = magPullGloves[1];
                            break;
                        case 0: //forward
                            if (fwdX == 1)
                            {
                                myAnimator.SetInteger("Direction", 3);
                                // myRenderer.sprite = magPullGloves[4];
                            }
                            else
                            {
                                myAnimator.SetInteger("Direction", 7);
                                // myRenderer.sprite = magPullGloves[0];
                            }
                            break;
                        case -1: //down
                            myAnimator.SetInteger("Direction", 6);
                            //myRenderer.sprite = magPullGloves[7];
                            break;
                    }//end of internal Switch
                    break;
            }// end of External Switch
        }
        else if (IsUsingPushGloves)
        {
            myAnimator.SetBool("PushUse", true);
            switch ((int)MouseAim.x)
            {
                case 0:
                    switch ((int)MouseAim.y)
                    {
                        case 1: //up
                            myAnimator.SetInteger("Direction", 1);
                            break;
                        case 0: //forward
                            if (fwdX == 1)
                            {
                                myAnimator.SetInteger("Direction", 3);
                            }
                            else
                            {
                                myAnimator.SetInteger("Direction", 7);
                            }
                            break;
                        case -1: //down
                            myAnimator.SetInteger("Direction", 5);
                            break;
                    }//end of internal Switch
                    break; // end of x = 0
                case 1:
                    switch ((int)MouseAim.y)
                    {
                        case 1: //up
                            myAnimator.SetInteger("Direction",2);
                            break;
                        case 0: //forward
                            if (fwdX == 1)
                            {
                                myAnimator.SetInteger("Direction", 3);
                            }
                            else
                            {
                                myAnimator.SetInteger("Direction", 7);
                            }
                            break;
                        case -1: //down
                            myAnimator.SetInteger("Direction", 4);
                            break;
                    }//end of internal Switch
                    break;
                case -1:
                    switch ((int)MouseAim.y)
                    {
                        case 1: //up
                            myAnimator.SetInteger("Direction", 8);
                            break;
                        case 0: //forward
                            if (fwdX == 1)
                            {
                                myAnimator.SetInteger("Direction", 3);
                            }
                            else
                            {
                                myAnimator.SetInteger("Direction", 7);
                            }
                            break;
                        case -1: //down
                            myAnimator.SetInteger("Direction", 6);
                            break;
                    }//end of internal Switch
                    break;
            }
        }
        else if (IsUsingReflectCape)
        {
           // myAnimator.SetBool("PullUse", false);
            myAnimator.SetBool("CapeUse", true);
            switch ((int)MouseAim.x)
            {
                case 0:
                    switch ((int)MouseAim.y)
                    {
                        case 1: //up
                            myAnimator.SetInteger("Direction",1);
                            //myRenderer.sprite = capeSprites[2];
                            break;
                        case 0: //forward
                            if (fwdX <= 0)
                            {
                              //  myAnimator.SetTrigger("Left");
                                myAnimator.SetInteger("Direction", 4);
                                // myRenderer.sprite = capeSprites[0];
                            }
                            else
                            {
                            //    myAnimator.SetTrigger("Right");
                                myAnimator.SetInteger("Direction", 3);
                                // myRenderer.sprite = capeSprites[4];
                            }
                            break;
                        case -1: //down
                            //myAnimator.SetTrigger("Up");
                            myAnimator.SetInteger("Direction", 1);
                            // myRenderer.sprite = capeSprites[2];
                            break;
                    }//end of internal Switch
                    break; // end of x = 0
                case 1:
                    switch ((int)MouseAim.y)
                    {
                        case 1: //up
                           // myAnimator.SetTrigger("LeftUp");
                            myAnimator.SetInteger("Direction", 5);
                            // myRenderer.sprite = capeSprites[3];
                            break;
                        case 0: //forward
                           // myAnimator.SetTrigger("Left");
                            myAnimator.SetInteger("Direction", 4);
                            //myRenderer.sprite = capeSprites[4];

                            break;
                        case -1: //down
                          //  myAnimator.SetTrigger("Left");
                            myAnimator.SetInteger("Direction", 4);
                            //myRenderer.sprite = capeSprites[0];
                            break;
                    }//end of internal Switch
                    break;
                case -1:
                    switch ((int)MouseAim.y)
                    {
                        case 1: //up
                          //  myAnimator.SetTrigger("RightUp");
                            myAnimator.SetInteger("Direction", 2);
                            //myRenderer.sprite = capeSprites[1];
                            break;
                        case 0: //forward
                           // myAnimator.SetTrigger("Right");
                            myAnimator.SetInteger("Direction", 3);
                            // myRenderer.sprite = capeSprites[0];

                            break;
                        case -1: //down
                           // myAnimator.SetTrigger("Right");
                            myAnimator.SetInteger("Direction", 3);
                            //myRenderer.sprite = capeSprites[0];
                            break;
                    }//end of internal Switch
                    break;
            }// end of External Switch
        }
        else
        {
            myAnimator.SetBool("PushUse", false);
            myAnimator.SetBool("PullUse", false);
            myAnimator.SetBool("CapeUse", false);
            myRenderer.sprite = basicSprite;
        }



    }
	void Jump()
	{
        
        float ji = IsDashing ? jumpImpulse * 1.5f : jumpImpulse;
		myRigidBody.AddForce(new Vector2(0, ji * Mathf.Sign(myRigidBody.gravityScale)), ForceMode2D.Impulse);
		AudioManager.Instance.PlaySound (jumpClip);
	}

    void HandleLadder(LadderController lc, Collider2D col)
    {
        if (!IsClimbing)
        {
            if (BottomCenter.y >= lc.transform.position.y + lc.gameObject.HalfHeight() * 0.8f)
            {
                if (controller.Vertical < 0)
                {
                    col.isTrigger = true;
                    IsClimbing = true;
                    Vector3 pos = transform.position;
                    pos.x = lc.transform.position.x;
                    transform.position = pos;
                }
                else
                {
                    col.isTrigger = lc.LadderAbove;
                }
            }
            else
            {
                col.isTrigger = true;

                if (controller.Vertical > 0)
                {
                    IsClimbing = true;
                    Vector3 pos = transform.position;
                    pos.x = lc.transform.position.x;
                    transform.position = pos;
                }
            }
        }
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
				} else if (IsClimbing) {
					IsClimbing = false;
				}
			} else if (IsDashing) {
				foreach (ContactPoint2D cp in col.contacts) {
					if (cp.normal.y == 0) {
						IsDashing = false;
						break;
					}
				}
			}
		} else if (col.collider.CompareTag ("Enemy")) {
			manager.Reset ();
		} else if (col.collider.OnLayer ("Metal") && IsUsingMagnetGloves) {
			if (lastYVel < 0) {
				if (leftItem && leftItem.IsActive && leftItem is PullGlove) {
					leftItem.Deactivate ();
				} else if (rightItem && rightItem.IsActive && rightItem is PullGlove) {
					rightItem.Deactivate ();
				}
				myRigidBody.velocity = Vector2.zero;
			}
		} else if (col.collider.CompareTag ("Spike")) {
			IsDead = true;
		} else if (col.collider.CompareTag ("Ladder")) {
            HandleLadder(col.collider.GetComponent<LadderController>(), col.collider);
		}
       
	}

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.collider.CompareTag("Ladder"))
        {
            HandleLadder(col.collider.GetComponent<LadderController>(), col.collider);
        }
    }

    void OnCollisionExit2D(Collision2D col) {
		if (col.collider.CompareTag ("Ladder")) {
			col.collider.isTrigger = true;
		}

	}
		
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag ("Fire")) {
			IsDead = true;
		} else if (col.CompareTag ("Projectile")) {
			Projectile p = col.GetComponent<Projectile> ();
			if (!IsUsingReflectCape || !p.Reflected && !p.Reflect(MouseAim))
            {
                Die();
            } else if (!p.Reflected)
            {
                p.Reflected = true;
            }
		} else if (col.CompareTag ("Ladder")) {
            HandleLadder(col.GetComponent<LadderController>(), col);
		} 
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Ladder"))
        {
            HandleLadder(col.GetComponent<LadderController>(), col);
        }
    }

    void OnTriggerExit2D(Collider2D col)
	{
		if (col.CompareTag ("Ladder")) {
			if (IsClimbing) {
				if (BottomCenter.y >= col.transform.position.y + col.gameObject.HalfHeight () * 0.8f) {
					if (!col.GetComponent<LadderController> ().LadderAbove) {
						
						IsClimbing = false;

						Vector3 pos = transform.position;
						pos.y = col.transform.position.y + col.gameObject.HalfHeight () + HalfHeight;
						transform.position = pos;
						myRigidBody.velocity = Vector2.zero;
						col.isTrigger = false;
					}
				} else if (BottomCenter.y <= col.transform.position.y - col.gameObject.HalfHeight () * 0.8f) {
					if (!col.GetComponent<LadderController> ().LadderBelow) {
						IsClimbing = false;
						Vector3 pos = transform.position;
						pos.y = col.transform.position.y - col.gameObject.HalfHeight () + HalfHeight;
						transform.position = pos;
						myRigidBody.velocity = Vector2.zero;
						col.isTrigger = false;
					}
				}
			}
		} 
    }
	#endregion
 
	#region Gets
	/// <summary>
	/// Gets a value indicating whether this <see cref="T:Player"/> is on ground.
	/// </summary>
	/// <value><c>true</c> if is on ground; otherwise, <c>false</c>.</value>
	public bool IsOnGround
	{
		get
		{
			LayerMask mask = 1 << LayerMask.NameToLayer("Platforms");
			LayerMask mask2 = 1 << LayerMask.NameToLayer("Metal");

			int finalMask = mask | mask2;
			if (!IsClimbing) {
				finalMask |= (1 << LayerMask.NameToLayer ("Ladder"));
			}
            Vector2 down = new Vector2(0, -Mathf.Sign(myRigidBody.gravityScale));
			RaycastHit2D hit = Physics2D.BoxCast (transform.position, new Vector2 (HalfWidth, HalfHeight + 0.1f), 0, down, 0.5f, finalMask); 

			if (hit.collider != null) {
				if (!hit.collider.OnLayer ("Interactive") || (hit.collider.CompareTag("Ladder") || hit.collider.CompareTag("Block"))) {
					return hit.normal.y != 0;
				}

			}
			return false;
		} // end Get
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

	public Vector3 BottomCenter {
		get {
			return transform.position - new Vector3 (0, HalfHeight);
		}
	}

	public Vector3 TopCenter {
		get {
			return transform.position + new Vector3 (0, HalfHeight);
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
	/// Gets the animator.
	/// </summary>
	/// <value>The animator.</value>
	//public Animator Animator {
	//	get {
	//		return myAnimator;
	//	}
	//}

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
	/// Gets the interact distance.
	/// </summary>
	/// <value>The interact distance.</value>
	public float InteractDistance {
		get {
			return interactDistance;
		}
	}

	public bool CanInteract {
		get {
			return (state == PlayerState.Normal || state == PlayerState.CanFloat) && !GameManager.Instance.IsPaused;
		}
	}

    public UsableItem LeftItem
    {
        get
        {
            return leftItem;
        }
    }

    public UsableItem RightItem
    {
        get
        {
            return rightItem;
        }
    }

	/// <summary>
	/// Gets the sprite for the left item.
	/// </summary>
	/// <value>The left item's sprite.  Null otherwise.</value>
    public Sprite LeftItemSprite
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
    public Sprite RightItemSprite
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
				leftItem.itemBox = UIManager.Instance.LeftItemBox;
                UIManager.Instance.UpdateUI();
            } else if (rightItem == null)
            {
                rightItem = ui;
				rightItem.itemBox = UIManager.Instance.RightItemBox;
                UIManager.Instance.UpdateUI();
            } else
            {
                inventory.Add(ui);
            }
			//UIManager.Instance.StartCoroutine(UIManager.Instance.ShowFoundItemMessage(ui.description));
            
        }
    }

	void Die() {
		transform.Rotate (Vector3.forward, 90);
		AudioManager.Instance.PlaySound (spikeDeathClip);
		resetTimer.Reset ();
		resetTimer.Start ();
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
	public Vector2 MouseAim {
		get
		{
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			return (mousePos - (Vector2)transform.position).normalized;
		}
	}

    public Vector2 KeyAim
    {
        get
        {
            return new Vector2(controller.Horizontal, controller.Vertical).normalized;
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
				//myAnimator.SetBool ("FWD", false);
                state |= PlayerState.Frozen;
				myRigidBody.velocity = new Vector2 (0f, myRigidBody.velocity.y);
            }else
            {
                state &= ~PlayerState.Frozen;
            }
        }
    }

	public bool IsClimbing {
		get
		{
			return (state & PlayerState.IsClimbing) > 0;
		}
		set
		{
			if (value)
			{
				if (leftItem && leftItem.IsActive) {
					leftItem.Deactivate ();
				}

				if (rightItem && rightItem.IsActive) {
					rightItem.Deactivate ();
				}
				state = PlayerState.IsClimbing;
                myRigidBody.gravityScale = 0;

			}else
			{
				state &= ~PlayerState.IsClimbing;
				UIManager.Instance.HideInteraction ();
				if (myRigidBody) {
					myRigidBody.gravityScale = 1.5f;
				}
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
            if (value && !IsDashing && !IsFrozen) //if true, we aren't already dashing, and we aren't frozen
            {
                state |= PlayerState.Dashing;
                state |= PlayerState.Frozen;
				//myAnimator.SetBool ("FWD", false);
                myRigidBody.AddForce(new Vector2(fwdX * maxSpeed * 10,0), ForceMode2D.Impulse); //yes we are dashing now

               
            }
            else
            {
                state &= ~PlayerState.Dashing;
                state &= ~PlayerState.Frozen;
				UsableItem curItem = leftItem is DashBoots ? leftItem : rightItem;
				curItem.Deactivate();
            }
        }
    }

	public void Push(BlockController bc) {
		highlightedBody = bc.GetComponent<Rigidbody2D>();
		float dx = Mathf.Sign ((highlightedBody.transform.position - transform.position).x);
		fwdX = (int)dx;
		myRenderer.flipX = (fwdX == 1);
		transform.position = highlightedBody.transform.position - dx * new Vector3(HalfWidth + bc.gameObject.HalfWidth (), 0);
		IsPushing = true;
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
			if (value /*&& highlighted != null*/ && !IsPushing && !IsFrozen)
            {
                UIManager.Instance.ShowInteraction("Let Go");
                state = PlayerState.Pushing;
                state |= PlayerState.Frozen;
                state |= PlayerState.CanFloat;
				//myAnimator.SetBool ("FWD", false);
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
                state |= PlayerState.UsingReflectCape;
                
				if (!IsOnGround && CanFloat) {
					CanFloat = false;
					myRigidBody.velocity = myRigidBody.velocity.XVector();
					myRigidBody.gravityScale = 0;//0.75f;
					TimerManager.Instance.AddTimer(new Timer(()=>{StopFloat();}, 1.0f));
				} else {
					state |= PlayerState.Frozen;
				//	myAnimator.SetBool ("FWD", false);
				}
			}
			else if (!value && IsUsingReflectCape)
			{
				state &= ~PlayerState.UsingReflectCape;
				state &= ~PlayerState.Frozen;
				myRigidBody.gravityScale = 1.5f;
			}
			ShowAimArrow = IsUsingReflectCape;
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
			ShowAimArrow = IsUsingMagnetGloves;
        }
    }
    public bool IsUsingPushGloves
    {
        get
        {
            return (state & PlayerState.UsingPushGloves) > 0;
        }

        set
        {
            if (value && !IsUsingPushGloves && !IsFrozen)
            {
                state |= PlayerState.UsingPushGloves;
                state |= PlayerState.Frozen;
            }
            else
            {
                state &= ~PlayerState.UsingPushGloves;
                state &= ~PlayerState.Frozen;
            }
            ShowAimArrow = IsUsingPushGloves;
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

	public bool IsOnMovingPlatform {
		get {
			return (state & PlayerState.OnMovingPlatform) > 0;
		}

		set {
			if (value) {
				state |= PlayerState.OnMovingPlatform;
			} else {
				state &= ~PlayerState.OnMovingPlatform;
			}
		}
	}

    public void ResetStatus()
    {
        state = PlayerState.CanFloat;
    }
	public bool IsDead {
		get {
			return (state & PlayerState.Dead) > 0;
		}

		private set {
			if (value && !IsDead) {
				state = PlayerState.Dead | PlayerState.Frozen;
				Die ();
			} else if (!value){
				state &= ~PlayerState.Dead;
				state &= ~PlayerState.Frozen;
			}
		}
	}

	public bool ShowAimArrow {
		get {
			return arrowRenderer.enabled;
		}

		set {
			arrowRenderer.enabled = value;
		}
	}

	public void ShowMagnetRange(Color c) {
		rangeRenderer.enabled = true;
		rangeRenderer.color = c;
	}

	public void HideMagnetRange() {
		rangeRenderer.enabled = false;
	}

    #endregion gets

	public void Freeze(float time) {
		IsFrozen = true;
		Timer t = new Timer (() => {
			IsFrozen = false;
		}, time);
		t.Start ();
	}
    void Unfreeze() {
		IsFrozen = false;
	}

    public void Remove()
    {
        if (gameObject)
        {
            Destroy(gameObject);
        }
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
				leftItem.itemBox = UIManager.Instance.LeftItemBox;
            } else
            {
                oldItem = rightItem;
                rightItem = inventory[itemNum];
				rightItem.itemBox = UIManager.Instance.RightItemBox;
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
        
        UIManager.Instance.UpdateUI();
        UIManager.Instance.HideInteraction();
        myRigidBody.gravityScale = 1.5f;
		myRenderer.material.color = Color.white;
		state = PlayerState.CanFloat;
		transform.rotation = Quaternion.Euler(0, 0, 0);// (Vector3.forward, -90);
        myRigidBody.velocity = Vector2.zero;
        transform.position = Checkpoint.ActiveCheckpointPosition;

    }

	public bool IsReflecting {
		get {
			return IsUsingReflectCape;
		}
	}

	public Vector2 SurfaceForward {
		get {
			return MouseAim;
		}
	}

    public bool HasPushGloveEquipped
    {
        get
        {
            return (leftItem && leftItem is PushGlove) || (rightItem && rightItem is PushGlove);
        }
    }

    public bool HasPullGloveEquipped
    {
        get
        {
            return (leftItem && leftItem is PullGlove) || (rightItem && rightItem is PullGlove);
        }
    }
}


[System.Flags]
public enum PlayerState
{
	Normal = 0,
	Dashing = 1,
	Frozen = 2,
    Pushing = 4,
    UsingMagnetGloves = 8,
	UsingReflectCape = 16,
	CanFloat = 32,
	PushedByTheWindHorz = 64,
	PushedByTheWindVert = 128,
	IsClimbing = 256,
	OnMovingPlatform = 512,
	Dead = 1024,
    UsingPushGloves = 2048
}