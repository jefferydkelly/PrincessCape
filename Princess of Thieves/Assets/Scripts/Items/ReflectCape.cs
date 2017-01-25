using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bugs
//Cooldown coroutine doesn't run
public class ReflectCape : UsableItem
{
	Timer deactivateTimer;
	[SerializeField]
	float timeActive = 1.0f;
    // Use this for initialization
    void Start () {
		CreateCooldownTimer ();
		deactivateTimer = new Timer (() => {
			Deactivate ();
		}, timeActive); 
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
			deactivateTimer.Reset ();
			TimerManager.Instance.AddTimer (deactivateTimer);
        }
        else
        {
            Debug.Log("Reflection fizzles");
        }
    }

    public override void Deactivate()
    {
		
		if (GameManager.Instance.Player.IsUsingReflectCape) {
			GameManager.Instance.Player.IsUsingReflectCape = false;
			StartCooldown ();
		}

    }

    public override void Use()
    {
       
    }
}
