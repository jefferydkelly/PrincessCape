using UnityEngine;
using System.Collections;

public class SpellStatue : MonoBehaviour, InteractiveObject {

	bool activated = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Interact()
	{
		if (!activated)
		{
			activated = true;
		}
	}
}
