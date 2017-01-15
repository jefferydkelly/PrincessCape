using UnityEngine;
using System.Collections;
using System;
/// <summary>
/// Ten-Ton Tunic
/// </summary>
public class HeavyArmor : UsableItem {

    bool activated = false;
    Rigidbody2D myRigidBody;
	// Use this for initialization
	void Start () {
        myRigidBody = GameManager.Instance.Player.gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public override void Deactivate()
    {

        activated = false;
        myRigidBody.gravityScale = 1.5f;
        return;
    }

    public override void Activate()
    {
            activated = true;
            myRigidBody.gravityScale = 5f;
            return;
        
       
    }

    public override void Use()
    {
        
    }
}
