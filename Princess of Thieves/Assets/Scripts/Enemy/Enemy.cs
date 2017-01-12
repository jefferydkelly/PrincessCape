using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//--------------------
//Bug Fixes:
//When reseting, it not longer looks for Vector zero, and will take it's original patrol destination.
//Only sees 'behind' it, needed negative forward

//------------
//Bugs
//
public class Enemy : ResettableObject {
    Rigidbody2D myRigidBody;
    SpriteRenderer myRenderer;

    int fwdX = 1;
    public float maxSpeed = 1;

    [SerializeField]
    Sprite[] spriteStates;


    //AI Things
    public enum EnemyState { Stationary, Patrol, Chase, Charge };
    EnemyState curState = EnemyState.Patrol;
    float sightRange = 5.0f;
    float sightAngle = 60.0f;
    bool playerInSight = false;
    GameObject playerObj;
    /// <summary>
    /// Float time value that represent the last time the enemy saw the player. Used to Chasing
    /// </summary>
    float lastTimeSeenPlayer = 0f;
   	Vector3 playerChaseDest = Vector3.zero;
    bool atChaseDest = true;
    public float patrolDist = 1;
    Vector3 originalDest;
    Vector3 patrolDest = Vector3.zero;
    Vector3 patDestBuffer = new Vector3(0.5f, 0.5f, 0.5f);
    bool atPatrolDest = true, goingToPatrolDest = true;
    //Enemy Ability Things
    /// <summary>
    /// This float keeps track of the time the enemy started it's ability.
    /// </summary>
    private float abilityTimeStart = 0f;

    private bool isFrozen = false;

    private float dieValue;
    #region cooldownTimes
    [SerializeField]
    float timeToChargeAttack = 1f;
    [SerializeField]
    public GameObject chargeProjectile;
   	IEnumerator chargingAnimation;
 
    #endregion cooldownTimes
    // Use this for initialization
    void Start () {
        Dies();
        //MaterialPropertyBlock mater = new MaterialPropertyBlock();// = GetComponent<MaterialPropertyBlock>();
        //mater.SetFloat("_SliceAmount", 0);
        //GetComponent<SpriteRenderer>().SetPropertyBlock(mater);
        myRigidBody = GetComponent<Rigidbody2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position;
        playerObj = GameManager.Instance.Player.GameObject;
        originalDest = transform.position;
        patrolDest = new Vector3(transform.position.x + patrolDist, transform.position.y, 0);
       // Debug.Log(patrolDest);
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
    

    /// <summary>
    /// currently just goes wherever it wants
    /// </summary>
    /// <returns></returns>
    Vector3 GetPatrolLocation()
	{
        Flip();
        return new Vector3(transform.position.x + fwdX * 5, transform.position.y, transform.position.z);
    }
    void DieALittleMore()
    {
        dieValue += 1f;
        MaterialPropertyBlock mater = new MaterialPropertyBlock();// = GetComponent<MaterialPropertyBlock>();
        float sliceamt = 0.01f * dieValue;
        mater.SetFloat("_SliceAmount", sliceamt);
        GetComponent<SpriteRenderer>().SetPropertyBlock(mater);
        if (dieValue >= 35)
        {
            Destroy(GameObject);
        }
    }
    void Dies()
    {
        InvokeRepeating("DieALittleMore", 0f, 0.1f);
    }
    void Patrol()
    {
        if (goingToPatrolDest)
        {
            if (Vector3.Distance(patrolDest, transform.position) >= 0.5f)
            {
                // Debug.Log("here");                        
                transform.position = Vector3.MoveTowards(transform.position, patrolDest, 0.03f);
            }
            else
            {
                atPatrolDest = true;
                goingToPatrolDest = false;
                Flip();
                //transform.position = Vector3.MoveTowards(transform.position, originalDest, 0.05f);
            }
        }
        else
        {
            if (Vector3.Distance(originalDest, transform.position) >= 0.5f)
            {
                // Debug.Log("here2");
                transform.position = Vector3.MoveTowards(transform.position, originalDest, 0.03f);
            }
            else
            {
                Flip();
                atPatrolDest = true;
                goingToPatrolDest = true;
            }
        }
    }
    // Update is called once per frame
    void Update () {
        if (!GameManager.Instance.IsPaused)
        {
           
            //   GetComponent<MaterialPropertyBlock>().SetFloat("slice amount", 1);
            // Debug.Log(curState);
            switch (curState)
            {
               
                case EnemyState.Patrol://it's patrolling
                    Patrol(); //moved in order to save space.
                    break; //AT PATROL STATE---------------------   
                case EnemyState.Chase:
                    
                    transform.position = Vector3.MoveTowards(transform.position, GameManager.Instance.Player.transform.position, 0.03f);
                    if(Time.time - lastTimeSeenPlayer > 3f)
                    {
                        Debug.Log("This machine is experiencing uncertainty");
                        curState = EnemyState.Patrol;
                    }
                    break;
            }
	    }
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
        
        Vector3 dif = p.transform.position - transform.position;
        if (dif.sqrMagnitude <= sightRange * sightRange)
        {
            if (InSightCone(p.gameObject, sightAngle))
            {
                if (!Physics2D.Raycast(transform.position, dif.normalized, dif.magnitude, 1 << LayerMask.NameToLayer("Platforms"))) //doesn't hit a platform
                {                      
                   // 
                    playerInSight = true;
                    curState = EnemyState.Chase;
                    lastTimeSeenPlayer = Time.time;
                    
                    return;                       
                }
            }
            
        }
	
        playerInSight = false;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("ShockBall"))
        {

            if (!isFrozen)
            {
                isFrozen = true;
                WaitDelegate w = () => { isFrozen = false; }; //Unsure what this does
                StartCoroutine(gameObject.RunAfter(w, 2f)); //freezes for 2 seconds
            }

            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Metal"){//dies when hit with metal of a high velocity
            //Code that works goes here

        }
    }
    protected bool InSightCone(GameObject go, float ang)
	{
        //angle is 60deg
        //Vector2 dif = go.transform.position - transform.position;
        //float dot = Vector2.Dot(dif.normalized, Forward);
        //return dot >= Mathf.Cos(ang * Mathf.Deg2Rad);
        float dot = Vector2.Dot(-Forward, (go.transform.position - transform.position).normalized);

        if (dot >= Mathf.Cos(ang * Mathf.Deg2Rad))
        {
            Debug.Log("forward sensors detect motion");
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Fires in an arc.
    /// </summary>
    private void Fire()
    {
        GameObject temp = new GameObject(); //change to instantiate once I put a prefab in
        temp.GetComponent<Rigidbody2D>().AddForce(new Vector2(fwdX, 1), ForceMode2D.Force);
    }
    private void Flip()
    {
		fwdX *= -1;
		myRenderer.flipX = (fwdX == -1);
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

    public override void Reset()
    {
        transform.position = startPosition;
        isFrozen = false;
        curState = EnemyState.Patrol;
        lastTimeSeenPlayer = 0f;
        playerChaseDest = Vector3.zero;
        atChaseDest = true;
        patrolDest = new Vector3(transform.position.x + patrolDist, transform.position.y, 0);
        patDestBuffer = new Vector3(0.5f, 0.5f, 0.5f);
        atPatrolDest = true;
    }
}
