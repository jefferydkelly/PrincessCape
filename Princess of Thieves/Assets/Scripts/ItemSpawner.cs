using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour, ActivateableObject {

	[SerializeField]
	GameObject prefab;
	[SerializeField]
	bool activateOnce = true;
	bool activated = false;
	[SerializeField]
	bool isActivationInverted = false;

	public void Activate() {
		if (!(activateOnce && activated)) {
			GameObject obj = Instantiate (prefab);
			obj.transform.position = transform.position;
			activated = true;
		}
	}

	public void Deactivate() {
		
	}

	public bool IsActive {
		get {
			return activated;
		}
	}

	public float ActivationTime {
		get {
			return 0;
		}
	}

	public bool IsInverted {
		get {
			return isActivationInverted;
		}
	}
}
