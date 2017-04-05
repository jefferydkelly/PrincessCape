using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Destroy(gameObject, 0.25f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
