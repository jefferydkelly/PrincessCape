using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivatorObject : ResettableObject {
	[SerializeField]
	protected List<GameObject> connectedObjects;
	protected List<ActivateableObject> activators;

	protected void Initialize() {
		
		activators = new List<ActivateableObject> ();
		if (connectedObjects.Count > 0) {
			foreach (GameObject go in connectedObjects) {
				ActivateableObject ao = go.GetComponent<ActivateableObject> ();
				if (ao != null) {
					activators.Add (ao);
				}
			}
		}
	}
	protected virtual void Activate()
	{
		foreach (ActivateableObject a in activators)
		{
			if (a.IsInverted) {
				a.Deactivate ();
			} else {
				a.Activate ();
			}
		}
	}

	protected virtual void Deactivate()
	{
		foreach (ActivateableObject a in activators)
		{
			if (a.IsInverted) {
				a.Activate ();
			} else {
				a.Deactivate ();
			}
		}
	}

	public override void Reset ()
	{
		Deactivate ();
	}
}
