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

    /// <summary>
    /// Fullstop is only to be used to make up for other colliders being slightly wrong, such as hitting the door and exploding.
    /// </summary>
    public bool fullStop = false;
    public bool busy = false;
    public bool slamming = false;
    /// <summary>
    /// let's just say the player has 4 seconds or the hand has 4 seconds to attack the player or get stuck
    /// </summary>
    private float timeOfAttack = 0f;
    /// <summary>
    /// Whether or not the hand is about to / moving to attack the player
    /// </summary>
    private bool movingToAttack = true;
    /// <summary>
    /// because move to seems to not work with one call, we need to capture that location and move towards it in FixedU
    /// </summary>
    private bool moveToPlayerLoc; 

    // Use this for initialization
    void Start () {
        upLoc = new Vector2(transform.position.x, transform.position.y + plusY);
        downLoc = new Vector2(transform.position.x, transform.position.y - minusY);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Position is : " + transform.position);
        if (!fullStop)
        {
            if (!busy)
            {
                if (moveUp)
                {
                    Debug.Log("moving");
                    transform.position = Vector3.MoveTowards(transform.position, upLoc, 1f * Time.deltaTime);
                    if (Vector3.Distance(this.transform.position, upLoc) < 1)
                    {
                        moveUp = false;
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, downLoc, 1f * Time.deltaTime);
                    if (Vector3.Distance(this.transform.position, downLoc) < 1)
                    {
                        moveUp = true;
                    }
                }
            }
            //The Hand is busy, moving towards the player's Y or going in for the kill
            else
            {

                Vector2 temp = new Vector2(transform.position.x, GameManager.Instance.Player.transform.position.y);
                transform.position = Vector3.MoveTowards(transform.position, temp, 2f * Time.deltaTime);
                if (Vector3.Distance(this.transform.position, temp) < 1)
                {
                    movingToAttack = false;
                    timeOfAttack = Time.time;
                }
                if (!movingToAttack)
                {
                    transform.position = Vector3.MoveTowards(transform.position,
                        GameManager.Instance.Player.transform.position, 2f * Time.deltaTime);
                }
            }
        }else
        {
            //full stopped
            
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        //fullStop = true;
    }
    /// <summary>
    /// Stops a hand being busy. Didn't put it in a get;set; on choice. 
    /// </summary>
    public void StopBeingBusy()
    {
        busy = false;
    }
    /// <summary>
    /// Slam left is used by the left hand in order to move towards a player location.
    /// Slam right is the same but for the Right hand.
    /// 
    /// ***Subject to Change/Deprecation***
    /// </summary>
    public void SlamLeft(GameObject player)
    {
        //Debug.Log("I'm bradberry");
        //should be fast


        //Vector3.MoveTowards(transform.position, player.transform.position, 5f * Time.deltaTime);
        busy = true;
    }

    public void SlamRight(GameObject player)
    {
        //Vector3.MoveTowards(transform.position,
        //    new Vector3(transform.position.x, player.transform.position.y), 1.5f * Time.deltaTime);
        //Vector3.MoveTowards(transform.position, player.transform.position, 2f * Time.deltaTime);
        busy = true;
    }

}
