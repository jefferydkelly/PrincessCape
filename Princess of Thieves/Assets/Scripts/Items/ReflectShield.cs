using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectShield : UsableItem
{
    public override void Activate()
    {
        itemActive = true;
        GameManager.Instance.Player.IsUsingShield = true;
    }

    public override void Deactivate()
    {
        itemActive = false;
        GameManager.Instance.Player.IsUsingShield = false;
    }

    public override void Use()
    {
        
    }
}
