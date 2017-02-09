using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivatorObject : MonoBehaviour {
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
	protected void Activate()
	{
		foreach (ActivateableObject a in activators)
		{
			a.Activate();
		}
	}

	protected void Deactivate()
	{
		foreach (ActivateableObject a in activators)
		{
			a.Deactivate();
		}
	}
}
