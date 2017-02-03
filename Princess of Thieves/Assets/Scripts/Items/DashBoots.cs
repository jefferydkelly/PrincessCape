using UnityEngine;
using System.Collections;
using System;

public class DashBoots : UsableItem {

    float timeIWentOnCooldown;
    
	void Start() {
		CreateCooldownTimer ();
	}

    public override void Activate()
    {
        
		if (!onCooldown && GameManager.Instance.Player.IsOnGround)
        {
            onCooldown = true;
            GameManager.Instance.Player.IsDashing = true;
        }
    }

    public override void Deactivate()
    {
       
        if (!GameManager.Instance.Player.IsDashing)
        {
			StartCooldown ();
        }
    }


	public override void Use() {
	}
}
