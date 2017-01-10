using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnVisibleScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
		
	}


    // Update is called once per frame
    void Update () {
        Camera mCam = Camera.main;
        //Screen.height, Screen.width;
        float halfWidth = Screen.width / 2;
        float halfHeight = Screen.height / 2;
        float quarterWidth = Screen.width / 4;
        float quarterHeight = Screen.height / 4;
        if (GetComponent<Renderer>().isVisible == false)
        {
            OnBecameInvisible();
        }
        else
        {
            Debug.Log("Hello");
            OnBecameVisible();
        }
	}

    void OnBecameInvisible()
    {

    }

    void OnBecameVisible()
    {

    }

}
