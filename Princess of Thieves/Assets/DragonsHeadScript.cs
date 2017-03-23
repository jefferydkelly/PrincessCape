using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonsHeadScript : MonoBehaviour {
    public float timeTilActivate = 3f;
	// Use this for initialization
	void Awake () {
        Timer t = new Timer(() => {
            ActivateCutscene(); 
        }, timeTilActivate);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void ActivateCutscene()
    {
        Debug.Log("Hello");
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "TopCeiling") //jank, but works
        {
            Debug.Log("Hello");

            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
