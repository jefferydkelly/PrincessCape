using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObjectScript : JDMappableObject, InteractiveObject {

    public enum Purpose { dropTarget };
    Purpose purpose = Purpose.dropTarget;
    [SerializeField]
    GameObject targetGameObject;
    public void Dehighlight()
    {
        //throw new NotImplementedException();
    }

    public void Highlight()
    {
       // throw new NotImplementedException();
    }

    public void Interact()
    {
        Debug.Log("Hello");
        switch (purpose)
        {
            case (Purpose.dropTarget):
                targetGameObject.GetComponent<DynamicObjectScript>().Release();
                break;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Release()
    {
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            Destroy(col.gameObject);
        }
    }

}
