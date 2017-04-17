using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempWaveScript : MonoBehaviour
{
    public GameObject target;
    float curScaleY;
    // Use this for initialization
    void Start()
    {
        curScaleY = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        //0.2
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 0.25f);
        //transform.localScale = new Vector3(0, curScaleY+0.1f, 0);
        //curScaleY = transform.localScale.y;
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.OnLayer("Metal"))
        {
            Destroy(gameObject);
        }
    }
}
