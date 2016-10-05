using UnityEngine;
using System.Collections;

public class TorchControler : MonoBehaviour, BurnableObject {
	Light yagami;
	// Use this for initialization
	void Start () {
		yagami = GetComponent<Light>();
	}

	public void Burn()
	{
		yagami.enabled = true;
	}

	public void Douse()
	{
		yagami.enabled = false;

	}
}
