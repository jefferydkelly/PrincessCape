using UnityEngine;
using System.Collections;
using System;

public class DashBoots : UsableItem {

    public override void Use()
    {
        if (!onCooldown)
        {
            onCooldown = true;
            GameManager.Instance.Player.IsDashing = true;
        }
    }

    public void StartCooldown()
    {
        WaitDelegate w = () => { onCooldown = false; };
        StartCoroutine(gameObject.RunAfter(w, cooldownTime));
    }
}
