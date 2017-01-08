using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bugs
//Cooldown coroutine doesn't run
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
		if (!GameManager.Instance.Player.IsUsingReflectCape && !onCooldown)
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
			onCooldown = true;
			WaitDelegate w = () => { onCooldown = false; };
			StartCoroutine(gameObject.RunAfter(w, cooldownTime));
        }
    }

    public override void UseMana()
    {
       
    }
}
