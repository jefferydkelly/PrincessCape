using UnityEngine;
using System.Collections.Generic;

public class ChargeShotScript : MonoBehaviour {

    public List<GameObject> lightningEnds;
    public bool faceLeft = true;
    int fwdX = -1;
    SpriteRenderer spriteControl;
    Vector2 newXY;
    Vector2 newXY2;
    Vector2 newXY3;
    // Use this for initialization
    void Start () {
        spriteControl = GetComponent<SpriteRenderer>();
	}
	
    public void SwitchFace()
    {
        faceLeft = false; // faces right
        fwdX = 1; //still right
    }
	// Update is called once per frame
	void Update () {

        //needs to move each object down a little
        for (int i = 0; i < 3; i++)
        { 
            //this doesn't seem like a good idea
            newXY = new Vector2(lightningEnds[0].gameObject.transform.position.x, 
                lightningEnds[0].gameObject.transform.position.y);
            newXY2 = new Vector2(lightningEnds[1].gameObject.transform.position.x,
                    lightningEnds[1].gameObject.transform.position.y);
            newXY3 = new Vector2(lightningEnds[2].gameObject.transform.position.x,
                    lightningEnds[2].gameObject.transform.position.y);
            if (faceLeft) //faces left, obv
            {

            }
            else
            {

            }
        }
	}

    private void Flip()
    {
        if (fwdX > 1)
        {
            fwdX = ~fwdX;
        }
        else
        {
            fwdX = ~fwdX;
        }
    }
}
