using UnityEngine;
using System.Collections;
using System;

public class DashBoots : UsableItem {

    public override void Activate()
    {
        if (!onCooldown)
        {
            onCooldown = true;
            GameManager.Instance.Player.IsDashing = true;
        }
    }

    public override void Deactivate()
    {
        if (!GameManager.Instance.Player.IsDashing)
        {
            WaitDelegate w = () => { onCooldown = false; };
            StartCoroutine(gameObject.RunAfter(w, cooldownTime));
        }
    }
}
