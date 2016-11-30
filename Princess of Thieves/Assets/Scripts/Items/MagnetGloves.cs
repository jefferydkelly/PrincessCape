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

    public override void Activate()
    {
        //Shoot a ray fowards
        RaycastHit2D hit;
        hit = (Physics2D.Raycast(GameManager.Instance.Player.gameObject.transform.position, GameManager.Instance.Player.Forward,
            100f, ~1<<LayerMask.NameToLayer("Player") | LayerMask.NameToLayer("SpellStatue")));
       // Debug.Log("Hit is what: " + hit.collider.name);
        if (hit.collider.gameObject.GetComponent<ObjectWeight>())
        {//first hit object has an ObjectWeight
            ObjectWeight thatWeight = hit.collider.gameObject.GetComponent<ObjectWeight>(); 
            if(thatWeight.objectWeight > GameManager.Instance.Player.gameObject.GetComponent<ObjectWeight>().objectWeight)
            {
                //Heavier object, so the player gets moved
                float dist = Vector3.Distance(thatWeight.gameObject.transform.position, GameManager.Instance.Player.gameObject.transform.position);
                GameManager.Instance.Player.gameObject.GetComponent<Rigidbody2D>().AddForce(
                    new Vector2(dist * GameManager.Instance.Player.Forward.x, 0).normalized  * (2500),
                    ForceMode2D.Force);
            }
            else
            {
                float dist = Vector3.Distance(thatWeight.gameObject.transform.position, GameManager.Instance.Player.gameObject.transform.position);
                thatWeight.gameObject.GetComponent<Rigidbody2D>().AddForce(
                    new Vector2(dist * -GameManager.Instance.Player.Forward.x, 0).normalized * (1000),
                    ForceMode2D.Force);
            }
        }

    }

    public override void Deactivate()
    {
        
    }

}
