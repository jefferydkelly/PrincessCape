using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Deprecated for now. Don't delete
/// A simple class, used to wrap around variables that the 'cages' will use to determine if the hand is captured.
/// </summary>
public class HandCageClass
{
    //public bool handLockedInCage = false;
    //public bool handInsideCage = false;
    //public float timeLockedInCage = 0f;
}

public class MordilHandCage : MonoBehaviour
{

    //public:
    public bool handLockedInCage = false;
    public bool handInsideCage = false;
    public float timeLockedInCage = 0f;
    /// <summary>
    /// used to keep track of which doors are down.
    /// ideally 0 is left, 1 is right
    /// </summary>
    public List<PressureSwitch> gateSwitches;

    //private:
    private float lastCheck = 0;
    private GameObject internalHand;



    /// <summary>
    /// Grabs and 'sedates' the internal hand until it breaks out after x time. Which is time.time - timeLockedInCage
    /// </summary>
    GameObject GrabInternalHand()
    {
        //has all the local colliders
        Collider2D[] cols = Physics2D.OverlapCircleAll(this.transform.position, 5f);
        foreach (Collider2D item in cols)
        {
            if(item.name == "HandOfMordil") //Could be seperated to its own layer to allow for layer mask and not have to do a
                                            //bool comparison
            {
                //internalHand = item.gameObject;
                timeLockedInCage = Time.time;
                handLockedInCage = true;
                item.GetComponent<HandOfMordil>().fullStop = true;
                return item.gameObject;
            }
        }
        return null;
    }

    /// <summary>
    /// You know how we do it
    /// </summary>
    void FixedUpdate()
    {
        //every 3 sec determing if handtrapped
        if(Time.time-lastCheck >= 3f)
        {
            handInsideCage = HandTrapped();
            lastCheck = Time.time;  
        }
        if (handInsideCage)
        {
           internalHand =  GrabInternalHand();
        }
    }
    bool HandTrapped()
    {
        foreach (PressureSwitch item in gateSwitches)
        {
            if (item.triggered)
            {
                if(GrabInternalHand() == null)
                    handInsideCage = false;
              
                return false;
            }else
            {
                break;
            }

        }
//        Debug.Log("TRAPPED");
        return true;
    }
}
