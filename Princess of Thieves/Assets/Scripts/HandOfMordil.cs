using UnityEngine;
using System.Collections;

public class HandOfMordil : MonoBehaviour {

    /// <summary>
    /// because the hands should only move up and down on the y, you should use this value to move it back to the same x value.
    /// </summary>
    Vector2 baseXValue;
    MordilManager manager;
    bool slammable = false;
    [SerializeField]
    int plusY, minusY;
    bool moveUp = true;
    Vector2 upLoc, downLoc;

    public bool busy = false;
    // Use this for initialization
    void Start () {
        upLoc = new Vector2(transform.position.x, transform.position.y + plusY);
        downLoc = new Vector2(transform.position.x, transform.position.y - minusY);
    }
	
	// Update is called once per frame
	void Update () {
        if (!busy && moveUp)
        {
            if (Vector3.Distance(this.transform.position, upLoc) < 1)
            {
                moveUp = false;
            }
        }
    }

    /// <summary>
    /// Slam left is used by the left hand in order to move towards a player location.
    /// Slam right is the same but for the Right hand.
    /// </summary>
    public void SlamLeft(GameObject player)
    {
        //should be fast
        //Vector3.MoveTowards(transform.position,
        //    new Vector3(transform.position.x, player.transform.position.y), 1.5f * Time.deltaTime);
        Vector3.MoveTowards(transform.position, player.transform.position, 2f * Time.deltaTime);
        busy = true;
    }

    public void SlamRight(GameObject player)
    {
        //Vector3.MoveTowards(transform.position,
        //    new Vector3(transform.position.x, player.transform.position.y), 1.5f * Time.deltaTime);
        Vector3.MoveTowards(transform.position, player.transform.position, 2f * Time.deltaTime);
        busy = true;
    }

}
