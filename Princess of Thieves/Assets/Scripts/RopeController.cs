using UnityEngine;
using System.Collections;

public class RopeController : MonoBehaviour {

    private bool completeRope = false;
    private Vector3 originalLocation;
    private bool moved = false;
    private float dTimeMove;
    [SerializeField]
    GameObject ropePrefab;
    //private float hit;
    GameObject nextRope;
    // Use this for initialization
    void Start () {
        originalLocation = transform.position;
       // ropePrefab = this.gameObject;
        //GenerateRope();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        int dist = 10;
        Vector3 dir = new Vector3(0, -1, 0);
        Debug.DrawRay(transform.position, dir * 2f, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 2f);
        if (hit.collider.tag == "Finish")
        {
            Debug.Log("Collided");
            completeRope = true;
        }
        else
        {
            //nothing was below your gameObject within 10m.
        }
        if (!completeRope)
        {
            GenerateRope();
        }
        if(Time.fixedTime - dTimeMove > 3f && moved)
        {
            Debug.Log("Reset to OL " + originalLocation);
            this.gameObject.transform.position = originalLocation;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            moved = false;
        }
	}
    //generateRope
    void GenerateRope()
    {
        //Creates a rope segment below this one.
        GameObject newRope = new GameObject();
        newRope.transform.parent = transform;
        newRope = (GameObject)Instantiate(ropePrefab,
            new Vector3(transform.position.x, transform.position.y-0.5f, transform.position.z),
            this.transform.rotation);
        
        completeRope = true;
        nextRope = newRope;
    }
    public void MoveRope(Vector2 velocity)
    {
        dTimeMove = Time.fixedTime;
        gameObject.GetComponent<Rigidbody2D>().AddForceAtPosition(velocity, new Vector2(0, -gameObject.GetComponent<Renderer>().bounds.extents.y));
        //gameObject.GetComponent<Rigidbody2D>().AddForce(velocity);
        if (!completeRope) 
            nextRope.GetComponent<RopeController>().MoveRope(velocity*1.5f);
       


        moved = true;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Rigidbody2D>() && col.gameObject.tag == "Player")
        {
            if(!moved)
            MoveRope(col.gameObject.GetComponent<Rigidbody2D>().velocity);
           
        }
        //Problematic?
        if(col.gameObject.tag != gameObject.tag)
            completeRope = true;
    }
}
