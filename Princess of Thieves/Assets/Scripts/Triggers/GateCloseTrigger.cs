using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(BoxCollider2D))]
public class GateCloseTrigger : TriggerBase {
    public GameObject connected;
    protected override void Trigger()
    {
        if (!triggered)
        {
            ActivateableObject ao = connected.GetComponent<ActivateableObject>();

            if (ao != null)
            {
                ao.Deactivate();
            }
            triggered = true;
        }
    }
}
