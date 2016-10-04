using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivatorObject : MonoBehaviour {
	public List<GameObject> activatedObjects;
	protected List<ActivateableObject> activators;

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
