using UnityEngine;
using System.Collections;

public class RopeCreateScript : MonoBehaviour {

    Rope2DEdScript ropEd;
    [SerializeField]
    GameObject ropeprefab;
	// Use this for initialization
	void Start () {
        ropEd = GetComponent<Rope2DEdScript>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D col)//only collides with platforms I suppose
    {
        if(col.gameObject.layer == (LayerMask.NameToLayer("Platforms")))
        {
            RopeGen();
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    void RopeGen()
    {

        //GameObject newobj = new GameObject();
        ropEd.endPoint = (GameObject)Instantiate(ropeprefab, new Vector2(this.transform.position.x, this.transform.position.y-2f), this.gameObject.transform.rotation);
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10.0f, (1 << LayerMask.NameToLayer("Platforms")));
        //if (hit.collider)
        //{
        //    ropEd.startPoint = this.gameObject;
        //    GameObject newobj = new GameObject();
        //    ropEd.endPoint = (GameObject)Instantiate(newobj, hit.point, this.gameObject.transform.rotation);
        //    ropEd.GenerateRope();
        //    Destroy(newobj);
        //}
    }
}
