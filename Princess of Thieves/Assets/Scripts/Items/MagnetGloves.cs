using UnityEngine;
using System.Collections;
using System;

public class MagnetGloves : UsableItem {

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Use()
    {
        //Shoot a ray fowards
        RaycastHit2D hit;
        hit = (Physics2D.Raycast(transform.position, GameManager.Instance.Player.Forward, 10f));
        if (hit.collider.gameObject.GetComponent<ObjectWeight>())
        {//first hit object has an ObjectWeight
            Debug.Log("Maaaagnet");
            ObjectWeight thatWeight = hit.collider.gameObject.GetComponent<ObjectWeight>(); 
            if(thatWeight.objectWeight > GameManager.Instance.Player.gameObject.GetComponent<ObjectWeight>().objectWeight)
            {
                //Heavier object, so the player gets moved
                //myRigidBody.AddForce(new Vector2(0, jumpImpulse * Mathf.Sign(myRigidBody.gravityScale)),
                //ForceMode2D.Impulse);
                GameManager.Instance.Player.gameObject.GetComponent<Rigidbody2D>().AddForce(
                    new Vector2(Vector3.Distance(transform.position, GameManager.Instance.Player.gameObject.transform.position), 0) * 100f,
                    ForceMode2D.Impulse);

            }
            else
            {

            }
        }

    }

}
