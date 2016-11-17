using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MordilManager : MonoBehaviour {
    static MordilManager instance;
    [SerializeField]
    List<BossEyeScript> mordEyes;
    [SerializeField]
    List<HandOfMordil> mordHands; //0 left 1 right
    [SerializeField]
    Sprite[] facesOfMordil;

    float handSlamCD;

    SpriteRenderer sRender;
    //Player Related Variables
    Player player;
    bool playerInSight = false;
	// Use this for initialization
	void Start () {
        player = GameManager.Instance.Player;
        instance = this;
        sRender = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        VisionPolling();
	    if(Time.time - handSlamCD > 5)
        {
            CallHandSlam();
            handSlamCD = Time.time;
        }
	}

    /// <summary>
    /// calls the correct hand to slam using the position of the player.
    /// </summary>
    void CallHandSlam()
    {

        //get whether the player is to the left or right of the head
        if(transform.position.x > player.transform.position.x) //easier than what it was, don't ask lmao
        {
            
            //left hand slam
            mordHands[0].SlamLeft(player.gameObject);
        }
        else
        {
            //left hand slam
            mordHands[1].SlamLeft(player.gameObject);
        }

    }

    /// <summary>
    /// goes through every eye to see if one has player within sight. If none do, there is no player in sight.
    /// This works because otherwise an eye not seeing the player would overwrite all others.
    /// </summary>
    void VisionPolling()
    {
        foreach(BossEyeScript eye in mordEyes)
        {
         //   Debug.Log("eye " + eye.name + " is able to see: " + eye.playerInSight);
            if (eye.playerInSight)
            {
                playerInSight = true;
                return;

            }

        }
        foreach (HandOfMordil item in mordHands)
        {
            item.StopBeingBusy();
        }
        
        playerInSight = false;

    }
    void FixedUpdate()
    {
        if (playerInSight)
        {
            sRender.sprite = facesOfMordil[1];
        }
        else
        {
            sRender.sprite = facesOfMordil[0];
        }

    }
    public static MordilManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MordilManager();
            }

            return instance;
        }
    }

    public bool PlayerInSight
    {
        get
        {
            return playerInSight;
        }
        set
        {
            playerInSight = value;
        }

    }
}
