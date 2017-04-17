using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempWaveScript : MonoBehaviour
{
    public GameObject target;
    float curScaleX;
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
        transform.rotation = Quaternion.AngleAxis(((Vector2)(target.transform.position - transform.position)).GetAngle().ToDegrees() - 90, Vector3.forward);
        transform.localScale = new Vector3(curScaleX + 0.1f, 1, 1);
        curScaleX = transform.localScale.x;
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.OnLayer("Metal"))
        {
            Destroy(gameObject);
        }
    }
}
