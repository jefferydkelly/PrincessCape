using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameController : MonoBehaviour, BurnableObject {

	public void Douse() {
		Destroy (gameObject);
	}

	public void Burn() {
	}
}
