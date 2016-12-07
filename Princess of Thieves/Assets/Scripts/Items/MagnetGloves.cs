using UnityEngine;
using System.Collections;
using System;

public class MagnetGloves : UsableItem {

    bool toggled = false;
    public Sprite pushSprite;
    public Sprite pullSprite;
    public float force = 100;
    // Use this for initialization
    ObjectWeight target;
    Player player;
    ObjectWeight playerWeight;

    private void Start()
    {
        player = GameManager.Instance.Player;
        playerWeight = player.GetComponent<ObjectWeight>();
    }


    public override void Activate()
    {
        //Shoot a ray fowards
        RaycastHit2D hit;
        hit = (Physics2D.Raycast(player.transform.position, player.Aiming,
            100f, 1<<LayerMask.NameToLayer("Metal") ));

        if (hit && hit.collider.GetComponent<ObjectWeight>())
        {//first hit object has an ObjectWeight

            target = hit.collider.GetComponent<ObjectWeight>();
            player.IsPushing = true;
        }
            
      
    }

    public void Use()
    {
        Vector3 distance = player.transform.position - target.transform.position;
        if (distance.sqrMagnitude <= 10000)
        {
            Debug.Log("Player Weight: " + playerWeight.objectWeight + " object weight " + target.objectWeight);
            if (toggled)
            {
                
                if (target.objectWeight > playerWeight.objectWeight)
                {
                    //Heavier object, so the player gets moved

                    //float dist = Vector3.Distance(thatWeight.transform.position, player.transform.position);
                    player.GetComponent<Rigidbody2D>().AddForce(
                        distance.normalized * -force,
                        ForceMode2D.Force);
                }
                else
                {
                    Debug.Log("lessed");
                    target.GetComponent<Rigidbody2D>().AddForce(
                        distance.normalized * force,
                        ForceMode2D.Force);
                }
            }
            else
            {
                if (target.objectWeight > playerWeight.objectWeight)
                {
                    //Heavier object, so the player gets moved

                    player.GetComponent<Rigidbody2D>().AddForce(
                        distance.normalized * force,
                        ForceMode2D.Force);
                }
                else
                {
                    Debug.Log("lessed");
                    target.GetComponent<Rigidbody2D>().AddForce(
                        distance.normalized * -force,
                        ForceMode2D.Force);
                }
            }
        }
    }

IEnumerator toggleLater(RaycastHit2D hit, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        //if (hit.collider.gameObject.GetComponent<Rigidbody2D>().constraints == ~RigidbodyConstraints2D.FreezePosition)
            hit.collider.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        Debug.Log("I'm here");
    }
    public override void Deactivate()
    {
        Toggled = !Toggled;
        player.IsPushing = false;
        target = null;
    }

    private bool Toggled
    {
        get
        {
            return toggled;
        }

        set
        {
            toggled = value;
            uiSprite = toggled ? pullSprite : pushSprite;
            UIManager.Instance.UpdateUI();
        }
    }

}
