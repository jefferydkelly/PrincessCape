using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    private Rigidbody2D myRigidBody;
    private SpriteRenderer myRenderer;
    private int fwdX = 1;
    public float maxSpeed = 1;

    public enum EnemyState { Stationary, Patrol, Chase, Charge };
    EnemyState curState = EnemyState.Stationary;

    private bool playerInSight = false;
    // Use this for initialization
    void Start () {
        myRigidBody = GetComponent<Rigidbody2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        Flip();
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(playerInSight);
	}

    void FixedUpdate()
    {
        LookForward(fwdX);
    }

    void LookForward(int fwd)
    {
        Color color = Color.red;
        //Debug.DrawRay(transform.position, Forward*10,Color.red);
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Forward, 10.0f, (1 << LayerMask.NameToLayer("Player")));
        //RaycastHit2D hitUp = Physics2D.Raycast(transform.position, new Vector2(Forward.x,0.7f), 10.0f, (1 << LayerMask.NameToLayer("Player")));
        //Debug.Log(hit.collider);
        //if ((hit.collider != null && hit.collider.gameObject.name == "Player") ||(hitUp.collider != null && hitUp.collider.gameObject.name == "Player"))
        //{
        //    color = Color.green;
           
        //}
        for (double x = 0;  x < 1; x += 0.1)
        {
            RaycastHit2D hitRecast = Physics2D.Raycast(transform.position, new Vector2(Forward.x, (float)x), 10.0f, (1 << LayerMask.NameToLayer("Player")));
            color = Color.red;
            if ((hitRecast.collider != null && hitRecast.collider.gameObject.name == "Player") )
            {
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
