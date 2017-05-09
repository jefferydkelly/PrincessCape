using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bugs
//Cooldown coroutine doesn't run
public class ReflectCape : UsableItem
{
	[SerializeField]
	AudioClip capeFX;
    // Use this for initialization
    void Start () {
		CreateCooldownTimer ();
	}

    // We assume that projectiles are tagged as such

    public override void Activate()
    {
		if (!GameManager.Instance.Player.IsUsingCape && !onCooldown)
        {
			//GameManager.Instance.Player.Animator.SetBool("Floating", true);

            GameManager.Instance.Player.IsUsingCape = true;
			itemActive = true;
			AudioManager.Instance.PlaySound (capeFX);
        }
    }

    public override void Deactivate()
    {
		//GameManager.Instance.Player.Animator.SetBool("Floating", false);
        if (IsActive) {
            itemActive = false;
			GameManager.Instance.Player.IsUsingCape = false;
            if (!GameManager.Instance.Player.IsDead)
            {
                StartCooldown();
            }
		}

    }

    public override void Use()
    {
       
    }
}
