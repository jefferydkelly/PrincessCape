using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {


    public Portal otherPort;
    // Use this for initialization
    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="oP"></param>
    Portal(Portal oP)
    {
        otherPort = oP;
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Contains("Player"))
        {
            if (otherPort != null)
            {
                col.gameObject.transform.position = otherPort.gameObject.transform.position;
            }
        }

    }
    
}
