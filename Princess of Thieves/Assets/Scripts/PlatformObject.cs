using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Collider2D))]
public class PlatformObject: JDMappableObject, ActivateableObject{

    public bool active = true;
    public bool atFinalLoc = false;
    public bool movingPlatform = false;
    public bool moveLeft = false;
    public bool moveRight = true;
    public bool moveUp = false;
    public float moveDist = 0f;
    Vector3 startingloc, endingLoc;

    Collider2D myCollider = null;

    public bool IsActive
    {
        get
        {
            return active;
        }
    }

    void Awake()
	{
		myCollider = GetComponent<Collider2D>();
        startingloc = transform.position;
        if (moveLeft)
        {
            endingLoc = new Vector3(transform.position.x - moveDist, transform.position.y, 0);
        }
        else if (moveUp)
        {
            endingLoc = new Vector3(transform.position.x, transform.position.y + moveDist, 0);
        }
        else
        {
            endingLoc = new Vector3(transform.position.x + moveDist, transform.position.y, 0);
        }
	}


    void Update()
    {
        if (movingPlatform && active)
        {
            MovePlatform();
        }
    }
    void MovePlatform()
    {
        if (!atFinalLoc)
        {
            if (Vector3.Distance(transform.position, endingLoc) <= 1)
            {
                atFinalLoc = true;

            }
            Vector3 pos2 = Vector3.MoveTowards(transform.position,
                            endingLoc, 2f * Time.deltaTime);//next spot on the curve
                                                            // NOTE: p2-pos is direction to next spot from old spot
                                                            //      normalized*speed is standard way to turn direction into constant speed
            Vector3 spd = (pos2 - transform.position).normalized * 2f;
            GetComponent<Rigidbody2D>().velocity = spd;
        }
        else
        {
            if (Vector3.Distance(transform.position, startingloc) <= 1)
            {
                atFinalLoc = false;

            }
            Vector3 pos2 = Vector3.MoveTowards(transform.position,
                            startingloc, 2f * Time.deltaTime);//next spot on the curve
                                                            // NOTE: p2-pos is direction to next spot from old spot
                                                            //      normalized*speed is standard way to turn direction into constant speed
            Vector3 spd = (pos2 - transform.position).normalized * 2f;
            GetComponent<Rigidbody2D>().velocity = spd;
        }
    }

    public void Activate()
    {
        active = true;
    }

    public void Deactivate()
    {
        active = false;
    }
}
