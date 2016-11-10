using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    Rigidbody2D myRigidBody;
    SpriteRenderer myRenderer;
    int fwdX = 1;
    public float maxSpeed = 1;



    //AI Things
    public enum EnemyState { Stationary, Patrol, Chase, Charge, ActualShoot };
    EnemyState curState = EnemyState.Stationary;
    float sightRange = 10.0f;
    float sightAngle = 45.0f;
    bool playerInSight = false;
    GameObject playerObj;
    /// <summary>
    /// Float time value that represent the last time the enemy saw the player. Used to Chasing
    /// </summary>
    float lastTimeSeenPlayer = 0f;
   	Vector3 playerChaseDest = Vector3.zero;
    bool atChaseDest = true;
    Vector3 patrolDest = Vector3.zero;
    Vector3 patDestBuffer = new Vector3(0.5f, 0.5f, 0.5f);
    bool atPatrolDest = true;
    //Enemy Ability Things
    /// <summary>
    /// This float keeps track of the time the enemy started it's ability.
    /// </summary>
    private float abilityTimeStart = 0f;
    #region cooldownTimes
    [SerializeField]
    float timeToChargeAttack = 1f;
    [SerializeField]
    public GameObject chargeProjectile;
   	IEnumerator chargingAnimation;

    #endregion cooldownTimes
    // Use this for initialization
    void Start () {
        myRigidBody = GetComponent<Rigidbody2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        
        playerObj = GameManager.Instance.Player.GameObject;
    }

    /// <summary>
    /// Returns true while there is still ground. False it sees no ground.
    /// </summary>
    /// <returns></returns>
    bool CheckGround()
    {
		RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(fwdX, -1),
            1.5f, (1 << LayerMask.NameToLayer("Platforms")));
        if (hit.collider != null)
        {
			Debug.DrawRay(transform.position, new Vector2(fwdX, -1) * 1.5f, Color.red, 1f);
            return false;
        }
        
        return true;
    }
    
    void Fire()
    {
		Debug.Log("Ready.  Aim. Fire.");
		Vector2 target = (playerObj.transform.position - transform.position).normalized;
		GameObject go = Instantiate(chargeProjectile);
		go.transform.position = transform.position;
		go.GetComponent<Rigidbody2D>().AddForce(1000 * target);
		curState = EnemyState.Chase;
    }

    /// <summary>
    /// currently just goes wherever it wants
    /// </summary>
    /// <returns></returns>
    Vector3 GetPatrolLocation()
    {
        //Get a random X value for the player to move to
        int tempLoR = Random.Range(-1, 1); //should force it to -1 or 1
        int tempX = Random.Range(1, 5);
        if(tempLoR == 0)
        {
            tempLoR = 1;
        }
        Flip();
		Vector3 returnVal = new Vector3(transform.position.x + fwdX * 5, transform.position.y, transform.position.z);

        return returnVal;
    }
    // Update is called once per frame
    void Update () {
		if (!GameManager.Instance.IsPaused)
		{
			//CheckGround();
			curState = CheckState();
			// Debug.Log("patrol Dest is: " + patrolDest);
			//Debug.Log("State is: " + curState);
			switch (curState)
			{
				case EnemyState.Charge:
					break;
				case EnemyState.Patrol:
					if (atPatrolDest)
					{
						//  Debug.Log("Am I here? " + atPatrolDest);
						patrolDest = GetPatrolLocation();
						atPatrolDest = false;
					}
					else
					{
						if (!CheckGround())
						{
							transform.position = Vector2.MoveTowards(gameObject.transform.position, patrolDest, 0.5f * Time.deltaTime);
							if (Vector3.Distance(this.transform.position, patrolDest) < 0.1f)
							{
								atPatrolDest = true;
							}
						}
						else
						{
							Debug.Log("No floor");
							atPatrolDest = true;
						}
					}
					break;
				case EnemyState.Chase:
					if (atChaseDest)
					{
						// Debug.Log("Am I at the player? " + atChaseDest);
						playerChaseDest = playerObj.transform.position;//GetPatrolLocation();
						atChaseDest = false;
					}
					else
					{
						transform.position = Vector2.MoveTowards(gameObject.transform.position, playerChaseDest, 0.25f * Time.deltaTime);
						if (transform.position == playerChaseDest)
						{
							atChaseDest = true;
						}
					}
					break;
			}
		}
	}
    EnemyState CheckState()
    {
		// Debug.Log("Player? : " + playerInSight);
		float pDist = Vector3.Distance(transform.position, playerObj.transform.position);
		if (curState == EnemyState.Charge) //override all else
		{
			//except one 
			if (!playerInSight)
			{
				CancelInvoke("Fire");
				return EnemyState.Chase;
			}
			return EnemyState.Charge;
		}
		else if (curState == EnemyState.Patrol)
		{
			if (playerInSight)
			{
				if (pDist < 15)
				{
					Invoke("Fire", timeToChargeAttack);
					return EnemyState.Charge;
				}

				return EnemyState.Chase;

			}
		}
		else if (curState == EnemyState.Chase)
		{
			if (playerInSight)
			{
				if (pDist < 15)
				{
					if (pDist < 15)
					{
						Debug.Log("Fire");
						Invoke("Fire", timeToChargeAttack);
						return EnemyState.Charge;
					}

					return Enemy.EnemyState.Chase;
				}

			}
		}
       
        return EnemyState.Patrol;
    }
    void FixedUpdate()
    {
        LookForward();
    }

    void LookForward()
    {
        Color color = Color.red;
        //is it more efficient to 'get player' once in Start? 
        Player p = GameManager.Instance.Player;
        if (!p.Hidden)
        {
            Vector3 dif = p.transform.position - transform.position;
            if (dif.sqrMagnitude <= sightRange * sightRange)
            {
				if (Vector2.Dot(dif.normalized, Forward) >= Mathf.Cos(sightAngle * Mathf.Deg2Rad)) //yeah this is much better
                {
                    if (!Physics2D.Raycast(transform.position, dif.normalized, dif.magnitude, 1 << LayerMask.NameToLayer("Platforms")))
                    {
                        playerInSight = true;
                        lastTimeSeenPlayer = Time.time;
                        return;
                    }
                }
            }
        }
        playerInSight = false;
    
        /*
        for (double x = 0;  x < 1; x += 0.1 * fwd)
        {
            RaycastHit2D hitRecast = Physics2D.Raycast(transform.position, new Vector2(Forward.x, (float)x), 15.0f, (1 << LayerMask.NameToLayer("Player")));
            color = Color.red;
            if ((hitRecast.collider != null && hitRecast.collider.gameObject.name == "Player") )
            {
                playerObj = hitRecast.collider.gameObject;
                //throw  line to make sure it's not hitting a platform first
                color = Color.red;
                RaycastHit2D hitRecast2 = Physics2D.Raycast(transform.position, new Vector2(Forward.x, (float)x),
                    Vector2.Distance(this.transform.position,playerObj.transform.position), (1 << LayerMask.NameToLayer("Platforms")));
                if(hitRecast2.collider == null && hitRecast2.collider.gameObject.tag != "Platforms")
                {
                    color = Color.green;
                    Debug.Log("Draw ray because I can see the player because there are no platforms in the way");
                    Debug.DrawRay(transform.position, playerObj.transform.position-transform.position, color);
                    playerInSight = true;
                    return;
                }
                playerInSight = false;
            }
        }*/ 

    }

    private void Flip()
    {
		fwdX *= -1;
		myRenderer.flipX = !myRenderer.flipX;
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
