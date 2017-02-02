using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrop : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (!(col.gameObject == GameManager.Instance.Player && GameManager.Instance.Player.IsUsingReflectCape)) {
			Destroy (gameObject);
		}
	}
}
