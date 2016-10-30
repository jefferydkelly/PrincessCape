using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    private Rigidbody2D myRigidBody;
    private SpriteRenderer myRenderer;
    private int fwdX = 1;
    public float maxSpeed = 1;



    //AI Things
    public enum EnemyState { Stationary, Patrol, Chase, Charge, ActualShoot };
    EnemyState curState = EnemyState.Stationary;
    private bool playerInSight = false;
    private GameObject playerObj;
    GameObject tempGameObj; //bad move but unavoidable
    /// <summary>
    /// Float time value that represent the last time the enemy saw the player. Used to Chasing
    /// </summary>
    private float lastTimeSeenPlayer = 0f;
    private Vector3 playerChaseDest = Vector3.zero;
    private bool atChaseDest = true;
    private Vector3 patrolDest = Vector3.zero;
    private bool atPatrolDest = true;
    //Enemy Ability Things
    /// <summary>
    /// This float keeps track of the time the enemy started it's ability.
    /// </summary>
    private float abilityTimeStart = 0f;
    #region cooldownTimes
    [SerializeField]
    float timeToChargeAttack = 2.5f;
    [SerializeField]
    GameObject chargeProjectile;
    private IEnumerator chargingAnimation;

    #endregion cooldownTimes
    // Use this for initialization
    void Start () {
        myRigidBody = GetComponent<Rigidbody2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        Flip();
    }
	
    /// <summary>
    /// Will set the time to charge, and defer anything until the charge is complete. Then it will fire a shot
    /// </summary>
    void ChargeAttack()
    {
        
        if (abilityTimeStart == 0f)
        {
            
            abilityTimeStart = Time.time;
            //I think this will just call it in 2.5 seconds
            //IEnumerator coroutine = ChargeAnim(2.5f);
            //StartCoroutine(coroutine);
            tempGameObj = Chargeshot();
        }
       // Debug.Log("Time: " + (Time.time - abilityTimeStart));
        //Should do nothing when between these two states
        if(Time.time - abilityTimeStart >= timeToChargeAttack)
        {
            //has to shoot!  
          
            ShootChargedShot();
            abilityTimeStart = 0f;
        }
    }

    private IEnumerator ChargeAnim(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            
        }
    }
    void ShootChargedShot()
    {
        if (tempGameObj)
        {
            //Vector2 temp = Vector2.D(this.gameObject.transform.position, playerObj.transform.position);
            float x = playerObj.transform.position.x - this.gameObject.transform.position.x;
            float y = playerObj.transform.position.y - this.gameObject.transform.position.y;
            Vector2 target = new Vector2(x, y);
            tempGameObj.GetComponent<Rigidbody2D>().AddForce(target * 100);
        }

    }
    GameObject Chargeshot()
    {
        GameObject shot = (GameObject)Instantiate(chargeProjectile, transform);
        shot.transform.position = this.transform.position;
        Destroy(shot, 5.5f);    
        //Add Force at a vector that points to the player object at the time of shooting.
        //uses lastKnownLocation if the player disappeared
        return shot;
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
        Vector3 returnVal = new Vector3(transform.position.x-tempX * tempLoR, transform.position.y, transform.position.z);

        return returnVal;
    }
    // Update is called once per frame
    void Update () {
        curState = CheckState();
       // Debug.Log("patrol Dest is: " + patrolDest);
        //Debug.Log("Chase is: " + curState);
        switch (curState)
        {
            case EnemyState.Charge:
               
                ChargeAttack();
                curState = EnemyState.ActualShoot;
                break;
            case EnemyState.ActualShoot:
                ChargeAttack();
                break;
            case EnemyState.Patrol:
                
                break;
            case EnemyState.Chase:

                break;
        }
        if (curState == EnemyState.Chase)
        {
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
        }
        if (curState == EnemyState.Patrol)
        {
            if (atPatrolDest)
            {
              //  Debug.Log("Am I here? " + atPatrolDest);
                patrolDest = GetPatrolLocation();      
                atPatrolDest = false;
            }
            else
            { 
                transform.position = Vector2.MoveTowards(gameObject.transform.position, patrolDest, 0.25f * Time.deltaTime);
                if (transform.position == patrolDest)
                {
                    atPatrolDest = true;
                }
            }
        }   
	}
    EnemyState CheckState()
    {
       // Debug.Log("Player? : " + playerInSight);
        if(curState == EnemyState.ActualShoot) //override all else
        {
            //except one 
            if (playerInSight)
                return EnemyState.ActualShoot;
            else
                return EnemyState.Chase;
        }
        if (playerInSight && Vector3.Distance(this.transform.position,playerObj.transform.position) < 15)
        {
            return EnemyState.Charge;
        }
        else if (playerInSight && Vector3.Distance(this.transform.position, playerObj.transform.position) >= 15)
        {
            return EnemyState.Chase;
        }
        else
        {
            return EnemyState.Patrol;
        }


        //if nothing else is happening
       // return EnemyState.Stationary;
    }
    void FixedUpdate()
    {
        LookForward(fwdX);
    }

    void LookForward(int fwd)
    {
        Color color = Color.red;

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
                if(hitRecast2.collider == null /*&& hitRecast2.collider.gameObject.tag != "Platforms"*/)
                {
                    color = Color.green;
                    Debug.Log("Draw ray because I can see the player because there are no platforms in the way");
                    Debug.DrawRay(transform.position, playerObj.transform.position-transform.position, color);
                    playerInSight = true;
                    return;
                }
                playerInSight = false;
            }
        }
        if (playerInSight)
        {
            lastTimeSeenPlayer = Time.time;
        }
        playerInSight = false;  

    }

    private void Flip()
    {
        if(fwdX > 1)
        {
            fwdX = ~fwdX;
        }
        else
        {
            fwdX = ~fwdX;
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
