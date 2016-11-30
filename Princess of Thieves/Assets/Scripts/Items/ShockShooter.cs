using UnityEngine;
using System.Collections;
using System;

public class ShockShooter : UsableItem {
    [SerializeField]
    GameObject shockObj;

    public override void Use()
    {
        if (!onCooldown)
        {
            Player p = GameManager.Instance.Player;
            GameObject shockBall = Instantiate(shockObj);
            onCooldown = true;
            WaitDelegate w = () => { onCooldown = false; };
            StartCoroutine(gameObject.RunAfter(w, cooldownTime));
        }
    }
}
