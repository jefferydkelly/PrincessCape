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
        //Vector2 temp = Vector2.D(this.gameObject.transform.position, playerObj.transform.position);
        float x = playerObj.transform.position.x - this.gameObject.transform.position.x;
        float y = playerObj.transform.position.y - this.gameObject.transform.position.y;
        Vector2 target = new Vector2(x,y);
        tempGameObj.GetComponent<Rigidbody2D>().AddForce(target * 100);

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
    // Update is called once per frame
    void Update () {
        curState = CheckState();
       // Debug.Log(playerInSight + " So I am now " + curState);

        switch (curState)
        {
            case EnemyState.Charge:
               
                ChargeAttack();
                curState = EnemyState.ActualShoot;
                break;
            case EnemyState.ActualShoot:
                ChargeAttack();
                break;
        }

	}
    EnemyState CheckState()
    {
        Debug.Log("Player? : " + playerInSight);
        if(curState == EnemyState.ActualShoot) //override all else
        {
            //except one 
            if (!playerInSight)
                return EnemyState.ActualShoot;
            else
                return EnemyState.Chase;
        }
        if (playerInSight && Vector3.Distance(this.transform.position,playerObj.transform.position) < 10)
        {
            return EnemyState.Charge;
        }
        else if (playerInSight && Vector3.Distance(this.transform.position, playerObj.transform.position) >= 10)
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

        for (double x = 0;  x < 1; x += 0.1)
        {
            RaycastHit2D hitRecast = Physics2D.Raycast(transform.position, new Vector2(Forward.x, (float)x), 10.0f, (1 << LayerMask.NameToLayer("Player")));
            color = Color.red;
            if ((hitRecast.collider != null && hitRecast.collider.gameObject.name == "Player") )
            {
                playerObj = hitRecast.collider.gameObject;
                color = Color.green;
                Debug.DrawRay(transform.position, new Vector2(Forward.x, (float)x) * 10, color);
                playerInSight = true;
                return;
            }
        }
        playerInSight = false;  
       // Debug.DrawRay(transform.position, Forward * 10, color);
       // Debug.DrawRay(transform.position, new Vector2(Forward.x, 0.7f) * 10, color);
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
