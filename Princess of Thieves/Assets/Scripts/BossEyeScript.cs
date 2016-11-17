using UnityEngine;
using System.Collections.Generic;

public class BossEyeScript : MonoBehaviour {


    Player player;

    /// <summary>
    /// Bools for whether or not the object is 'near' up, right, down, left
    /// </summary>
    List<bool> urdl;// { false,false,false,false };
    private bool atPatrolDest;
    private Vector3 patrolDest;
    private float newLocLockout;
    public bool playerInSight = false;
    private float lastTimeSeenPlayer;
    private int sightRange = 10;
    private Vector3 lastKnownPlayerPosition;
    // Use this for initialization
    void Start()
    {
        atPatrolDest = true;
        //at least it's not an array (semantics)
        urdl = new List<bool>();
        urdl.Add(false);
        urdl.Add(false);
        urdl.Add(false);
        urdl.Add(false);
        player = GameManager.Instance.Player;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void FixedUpdate()
    {
        LookForward();
        if (Time.time - newLocLockout >=1f)
           // CheckPlatforms();
        if (atPatrolDest)
        {
            //  Debug.Log("Am I here? " + atPatrolDest);
            patrolDest = GetNewPatrolLocation();
            atPatrolDest = false;
        }
        else
        {
            if (!playerInSight)
            {
                    playerInSight = false;
                //MordilManager.Instance.PlayerInSight = false;
                transform.position = Vector2.MoveTowards(gameObject.transform.position, patrolDest, 0.5f * Time.deltaTime);
            }
            else
            {
                playerInSight = true;
                //Debug.Log("I see you...");
               // MordilManager.Instance.PlayerInSight = true;
                transform.position = Vector2.MoveTowards(gameObject.transform.position, lastKnownPlayerPosition, 0.5f * Time.deltaTime);
            }
            if (Vector3.Distance(this.transform.position, patrolDest) < 1)
            {
                atPatrolDest = true;
            }
           

            
        }
    }
    /// <summary>
    /// in the event that something comes to close to the eye, it will find a new location to wander to.
    /// </summary>
    Vector3 GetNewPatrolLocation()
    {
        Vector3 newPatrolDest = Vector3.zero;
        if (!playerInSight)
        {
            int tempLoR = Random.Range(-1, 1); //should force it to -1 or 1
            if (tempLoR == 0)
            {
                tempLoR = 1;
            }
            int tempUoD = Random.Range(-1, 1);
            if (tempUoD == 0)
            {
                tempUoD = 1;
            }
            int tempY = Random.Range(1, 6);

            int tempX = Random.Range(1, 6);
           
            //what AI should it use? Move up down left right until it can't?
            newPatrolDest = new Vector3(transform.position.x - tempX * tempLoR,
                transform.position.y - tempY * tempUoD, transform.position.z);
            //Debug.Log("Going to: " + newPatrolDest);
           
        }
        else
        {
            //At this point needs to move towards lastKnownPlayerPosition

        }
        return newPatrolDest;
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

                if (!Physics2D.Raycast(transform.position, dif.normalized, dif.magnitude, 1 << LayerMask.NameToLayer("Platforms")))
                {
                    playerInSight = true;
                    lastKnownPlayerPosition = player.Position;
                    return;
                }
                
            }
        }
        playerInSight = false;


    }
    Vector3 GetPatrolLocation()
    {
        //Get a random X value for the player to move to
        int tempLoR = Random.Range(-1, 1); //should force it to -1 or 1
        int tempX = Random.Range(1, 5);
        if (tempLoR == 0)
        {
            tempLoR = 1;
        }

        Vector3 returnVal = new Vector3(transform.position.x - tempX * tempLoR, transform.position.y, transform.position.z);
        //if (returnVal.x > transform.position.x)
        //{
        //   //. fwdX = 1;
        //    gameObject.GetComponent<SpriteRenderer>().flipX = true;
        //}
        //else
        //{
        //    fwdX = -1;
        //    gameObject.GetComponent<SpriteRenderer>().flipX = false;
        //}
        return returnVal;
    }

    /// <summary>
    /// Toggles the bool for the corresponding direction if the eye is near an object
    /// </summary>
    void CheckPlatforms()
    {
        //this makes me feel bad... but It works
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 2.0f, (1 << LayerMask.NameToLayer("Platforms")));
        if(hit.collider != null)
        {
            urdl[0] = true;
        }
        else
        {
            urdl[0] = false;
        }
        hit = Physics2D.Raycast(transform.position, Vector2.right, 2.0f, (1 << LayerMask.NameToLayer("Platforms")));
        if (hit.collider != null)
        {
            urdl[1] = true;
        }
        else
        {
            urdl[1] = false;
        }
        hit = Physics2D.Raycast(transform.position, Vector2.down, 2.0f, (1 << LayerMask.NameToLayer("Platforms")));
        if (hit.collider != null)
        {
            urdl[2] = true;
        }
        else
        {
            urdl[2] = false;
        }
        hit = Physics2D.Raycast(transform.position, Vector2.left, 2.0f, (1 << LayerMask.NameToLayer("Platforms")));
        if (hit.collider != null)
        {
            urdl[3] = true;
        }
        else
        {
            urdl[3] = false;
        }
        foreach(bool val in urdl)
        {
            if (val)
            {
                newLocLockout = Time.time;
                Debug.Log("Too close");
                patrolDest = GetNewPatrolLocation();
                return;
            }
        }
    }
}
