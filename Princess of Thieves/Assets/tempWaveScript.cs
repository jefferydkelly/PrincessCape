using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempWaveScript : MonoBehaviour
{
    public GameObject target;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        //0.2
    
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Contains("Metal"))
        {
            Destroy(gameObject);
        }
    }
}
