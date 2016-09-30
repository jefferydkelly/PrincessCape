using UnityEngine;
using System.Collections;

public class RopeController : MonoBehaviour {

    private bool completeRope = false;
    [SerializeField]
    GameObject ropePrefab;
    //private float hit;
    GameObject nextRope;
    // Use this for initialization
    void Start () {
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
	}
    //generateRope
    void GenerateRope()
    {
        //Creates a rope segment below this one.
        GameObject newRope = (GameObject)Instantiate(ropePrefab,
            new Vector3(transform.position.x, transform.position.y-0.5f, transform.position.z),
            this.transform.rotation);
        completeRope = true;
        nextRope = newRope;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Rigidbody2D>())
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(col.gameObject.GetComponent<Rigidbody2D>().velocity);
            nextRope.GetComponent<Rigidbody2D>().AddForce(col.gameObject.GetComponent<Rigidbody2D>().velocity);
        }
        //Problematic?
        if(col.gameObject.tag != gameObject.tag)
            completeRope = true;
    }
}
