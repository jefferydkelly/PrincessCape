using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class LightAffectedByPlayer : MonoBehaviour {
    Light myLight;
    Rigidbody2D myRigidBody;
	// Use this for initialization
	void Start ()
    {
        myLight = GetComponent<Light>();
        myRigidBody = GameManager.Instance.Player.GetComponent<Rigidbody2D>();

    }
	
	// Update is called once per frame
	void Update () {
      //  Debug.Log(myRigidBody.velocity);
        myLight.intensity = 4 + Math.Abs(myRigidBody.velocity.x);
        //lightOnPlayer = GetLocalLightLevel();
    }
}
