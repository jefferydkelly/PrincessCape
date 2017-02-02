using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrip : MonoBehaviour {

    [SerializeField]
    GameObject waterDroplet;

    float timeToDrop;
	// Use this for initialization
	void Start () {
        timeToDrop = Random.Range(1, 5);
        InvokeRepeating("Drip", 1f, timeToDrop);
	}
	
	// Update is called once per frame
	void Update () {

	}

    void Drip()
    {
        GameObject temp = Instantiate(waterDroplet, transform.position, transform.rotation);
        Destroy(temp,2f);
        GetComponent<AudioSource>().Play();
    }
}
