using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivatorObject : ResettableObject {
	[SerializeField]
	protected List<ActivatorConnection> connectedObjects;
	//protected List<ActivateableObject> activators;

	protected void Initialize() {
        /*
		activators = new List<ActivateableObject> ();
		if (connectedObjects.Count > 0) {
			foreach (ActivatorConnection ac in connectedObjects) {
                ActivateableObject ao = ac.activated.GetComponent<ActivateableObject>();
				if (ao != null) {
					activators.Add (ao);
				}
			}
		}*/
    }
	protected virtual void Activate()
	{
        foreach(ActivatorConnection ac in connectedObjects)
        {
            ac.Activate();
        }
        /*
		foreach (ActivateableObject a in activators)
		{
			if (a.IsInverted) {
				a.Deactivate ();
			} else {
				a.Activate ();
			}
		}*/
	}

	protected virtual void Deactivate()
	{
        foreach (ActivatorConnection ac in connectedObjects)
        {
            ac.Dectivate();
        }
        /*
        foreach (ActivateableObject a in activators)
		{
			if (a.IsInverted) {
				a.Activate ();
			} else {
				a.Deactivate ();
			}
		}*/
	}

	public override void Reset ()
	{
		Deactivate ();
	}
}
