using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectCape : UsableItem
{


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // We assume that projectiles are tagged as such

    public override void Activate()
    {
        if (Time.time - GameManager.Instance.Player.lastTimeReflectUsed >= 3f)
        {
            GameManager.Instance.Player.IsUsingReflectCape = true;
        }
        else
        {
            Debug.Log("Reflection fizzles");
        }
    }

    public override void Deactivate()
    {
        if (GameManager.Instance.Player.IsUsingReflectCape == true)
        {
            GameManager.Instance.Player.IsUsingReflectCape = false;
            GameManager.Instance.Player.lastTimeReflectUsed = Time.time;
        }
    }

    public override void UseMana()
    {
       
    }
}
