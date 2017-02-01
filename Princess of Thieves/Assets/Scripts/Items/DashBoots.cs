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
        
        if (!onCooldown)
        {
           // Debug.Log("Timer manager name " + TimerManager.Instance.gameObject.name);
            onCooldown = true;
            Debug.Log("I'm on cooldown");
            timeIWentOnCooldown = Time.time;
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

    public override void Use()
    {

    }
    public  void Update()
    {
        if (onCooldown)
        {
            if(Time.time - timeIWentOnCooldown >= cooldownTime)
            {
                onCooldown = false;
            }
        }
    }
}
