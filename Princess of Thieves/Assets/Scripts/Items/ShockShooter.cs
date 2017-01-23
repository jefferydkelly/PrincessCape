using UnityEngine;
using System.Collections;
using System;

public class ShockShooter : UsableItem {
    [SerializeField]
    GameObject shockObj;

	void Start() {
		CreateCooldownTimer ();
	}
    public override void Activate()
    {
        if (!onCooldown)
        {
            Player p = GameManager.Instance.Player;
            GameObject shockBall = Instantiate(shockObj);
			StartCooldown ();
        }
    }

    public override void Deactivate()
    {
        
    }

    public override void Use()
    {
        
    }
}
