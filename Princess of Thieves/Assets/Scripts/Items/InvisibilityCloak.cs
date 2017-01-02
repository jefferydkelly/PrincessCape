using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibilityCloak : UsableItem {

    
    public override void Activate()
    {
        GameManager.Instance.Player.IsUsingInvisibilityCloak = true;
    }

    public override void Deactivate()
    {
        GameManager.Instance.Player.IsUsingInvisibilityCloak = false;
    }

    public override void UseMana()
    {
        
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
