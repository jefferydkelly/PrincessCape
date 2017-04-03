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
	[SerializeField]
	AudioClip capeFX;
    // Use this for initialization
    void Start () {
		CreateCooldownTimer ();
		deactivateTimer = new Timer (() => {
			Deactivate ();
		}, timeActive); 
	}

    // We assume that projectiles are tagged as such

    public override void Activate()
    {
		if (!GameManager.Instance.Player.IsUsingReflectCape && !onCooldown)
        {
			//GameManager.Instance.Player.Animator.SetBool("Floating", true);

            GameManager.Instance.Player.ShowAimArrow = true;
            GameManager.Instance.Player.IsUsingReflectCape = true;
			itemActive = true;
			AudioManager.Instance.PlaySound (capeFX);
        }
    }

    public override void Deactivate()
    {
		//GameManager.Instance.Player.Animator.SetBool("Floating", false);
        if (IsActive) {
            
            GameManager.Instance.Player.ShowAimArrow = false;
			GameManager.Instance.Player.IsUsingReflectCape = false;
			StartCooldown ();
		}

    }

    public override void Use()
    {
       
    }
}
