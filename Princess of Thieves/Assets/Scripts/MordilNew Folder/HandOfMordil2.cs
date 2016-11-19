using UnityEngine;
using System.Collections;

//I think enum works better than flags in this state

    /// <summary>
    /// rewrote because movetowards is bad and removes collisions :V
    /// </summary>
public class HandOfMordil2 : MonoBehaviour {
    public enum handStatus { Moving,Seeking,Attacking,Holding,Slamming,Reset,Trapped};
    public handStatus curStatus = handStatus.Moving;


    #region//Location Variables
    Vector3 startPosition;
    [SerializeField]
    float upY, downY;
    bool atUpLocation = false;
    Vector2 upXLoc, downXLoc;
    #endregion

    #region Attack
    float grabbedThePlayerWhen = 0; 


    #endregion
    // Use this for initialization
    void Start () {
        upXLoc = new Vector2(transform.position.x + upY, transform.position.y);
        downXLoc = new Vector2(transform.position.x - downY, transform.position.y );
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("where am I " + atUpLocation);
        if (!atUpLocation)
        {
           // Debug.Log("Distance is : " + Vector3.Distance(transform.position, upXLoc));
            if (Vector3.Distance(transform.position, upXLoc) <= 1)
            {
                atUpLocation = true;

            }
            else
            {
                Vector3 pos2 = Vector3.MoveTowards(transform.position,
                        upXLoc, 2f * Time.deltaTime);//next spot on the curve
                                                       // NOTE: p2-pos is direction to next spot from old spot
                                                       //      normalized*speed is standard way to turn direction into constant speed
                Vector3 spd = (pos2 - transform.position).normalized * 2f;
                GetComponent<Rigidbody2D>().velocity = spd;
            }
        }
        else
        {
           // Debug.Log("Distance is DOWN : " + Vector3.Distance(transform.position, downXLoc));
            if (Vector3.Distance(transform.position, downXLoc) <= 1)
            {
                atUpLocation = false;

            }
            else
            {
                Vector3 pos2 = Vector3.MoveTowards(transform.position,
                        downXLoc, 2f * Time.deltaTime);//next spot on the curve
                                                     // NOTE: p2-pos is direction to next spot from old spot
                                                     //      normalized*speed is standard way to turn direction into constant speed
                Vector3 spd = (pos2 - transform.position).normalized * 2f;
                GetComponent<Rigidbody2D>().velocity = spd;
            }
        }
        //Debug.Log(curStatus);
        //switch (curStatus)
        //{
        //    case handStatus.Moving:
        //        Moving();
        //        break;
        //    case handStatus.Seeking:
        //        Seeking();
        //        break;
        //    case handStatus.Attacking:
        //        Attack();
        //        break;
        //    case handStatus.Slamming:
        //        SlamAttack();
        //        foreach(Transform child in transform)
        //        {
        //            child.transform.position = transform.position;
        //        }

        //        break;
        //}
    }

    //void SlamAttack()
    //{
    //    Debug.Log("pos x " + transform.position.x);
    //    if (Time.time - grabbedThePlayerWhen < 1)
    //    {
    //        //needs to move to Mordil's face
    //        transform.position = MordilManager.Instance.gameObject.transform.position;
    //    }
    //    else if (Time.time - grabbedThePlayerWhen >= 1 && Time.time - grabbedThePlayerWhen < 3)
    //    {

    //        Vector3 pos2 = new Vector2(transform.position.x, transform.position.y + 5f);
    //        Vector3 spd = (pos2 - transform.position).normalized * 5f;
    //        GetComponent<Rigidbody2D>().velocity = spd;

    //    }
    //    else
    //    {
    //        //Debug.Log("Super Slam");
    //        Vector3 pos2 = new Vector2(transform.position.x, transform.position.y-10f);
    //        Vector3 spd = (pos2 - transform.position).normalized * 15f;
    //        GetComponent<Rigidbody2D>().velocity = spd;
    //    }
    //}


    //void Attack()
    //{
    //    Vector3 pos2 = GameManager.Instance.Player.transform.position;
    //    Vector3 spd = (pos2 - transform.position).normalized * 2f;
    //    GetComponent<Rigidbody2D>().velocity = spd;
    //    //if (Vector3.Distance(this.transform.position, pos2) < 0.5f)
    //    //{
    //    //    curStatus = handStatus.Attacking;
    //    //}
    //}

    void OnCollisionEnter2D(Collision2D col)
    {

        if(col.gameObject.name == "Player")
        {
            GameManager.Instance.Player.ResetBecauseINeed();
        }
        //    if(curStatus != handStatus.Slamming)
        //    {
        //        if (col.gameObject.tag == "Player")
        //        {
        //            curStatus = handStatus.Slamming;
        //            grabbedThePlayerWhen = Time.time;
        //            col.transform.position = transform.position;
        //            col.gameObject.transform.SetParent(transform);
        //            col.gameObject.GetComponent<Player>().IsFrozen = true;


        //        }
        //    }else if(col.gameObject.tag == "Platform")
        //    {
        //        foreach (Transform child in transform)
        //        {
        //            if(child.name == "Player")
        //            {
        //                curStatus = handStatus.Reset;
        //                child.parent = null;
        //                child.GetComponent<Player>().IsFrozen = false;
        //            }
        //        }
    }

    //}
    //void Slam()
    //{
    //    curStatus = handStatus.Seeking;
    //    //seeking the player
    //}

    //void Seeking()
    //{
    //    Vector3 pos2 = new Vector2(transform.position.x, GameManager.Instance.Player.transform.position.y);
    //    Vector3 spd = (pos2 - transform.position).normalized * 2f;
    //    GetComponent<Rigidbody2D>().velocity = spd;
    //    if (Vector3.Distance(this.transform.position, pos2) < 0.5f)
    //    {
    //        curStatus = handStatus.Attacking;
    //    }
    //}
    //void Moving()
    //{
    //    if (atUpLocation)
    //    {
    //        if (Vector3.Distance(transform.position, upYLoc) >= 1)
    //        {
    //            atUpLocation = true;

    //        }else
    //        {
    //            Vector3 pos2 = Vector3.MoveTowards(transform.position,
    //                    downYLoc, 2f * Time.deltaTime);//next spot on the curve
    //                                                                                    // NOTE: p2-pos is direction to next spot from old spot
    //                                                                                    //      normalized*speed is standard way to turn direction into constant speed
    //            Vector3 spd = (pos2 - transform.position).normalized * 2f;
    //            GetComponent<Rigidbody2D>().velocity = spd;
    //        }
    //    }
    //    else
    //    {
    //        if (Vector3.Distance(transform.position, downYLoc) >= 1)
    //        {
    //            atUpLocation = false;

    //        }
    //        else
    //        {
    //            Vector3 pos2 = Vector3.MoveTowards(transform.position,
    //                    upYLoc, 2f * Time.deltaTime);//next spot on the curve
    //                                                   // NOTE: p2-pos is direction to next spot from old spot
    //                                                   //      normalized*speed is standard way to turn direction into constant speed
    //            Vector3 spd = (pos2 - transform.position).normalized * 2f;
    //            GetComponent<Rigidbody2D>().velocity = spd;
    //        }
    //    }
    //}
}



//Deprecated
[System.Flags]
public enum HandState
{
    Moving = 0,
    Seeking = 1,
    Attacking = 2,
    Holding = 4,
    Slamming = 8,
    Reset = 16,
    Trapped = 32
}
