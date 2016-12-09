using UnityEngine;
using System.Collections;
using System;

public class MagnetGloves : UsableItem {

    bool toggled = false;
    public Sprite pushSprite;
    public Sprite pullSprite;
    public float force = 100;
    // Use this for initialization
    GameObject target;
    Rigidbody2D targetBody;
    Player player;
    Rigidbody2D playerBody;

    private void Start()
    {
        player = GameManager.Instance.Player;
        playerBody = player.GetComponent<Rigidbody2D>();
    }


    public override void Activate()
    {
        //Shoot a ray fowards
        RaycastHit2D hit;
        hit = (Physics2D.Raycast(player.transform.position, player.Aiming,
            100f, 1<<LayerMask.NameToLayer("Metal") ));

        if (hit)
        {//first hit object has an ObjectWeight

            target = hit.collider.gameObject;
            target.GetComponent<SpriteRenderer>().color = Color.blue;
            targetBody = target.GetComponent<Rigidbody2D>();
            player.IsPushing = true;
        }
            
      
    }

    public void Use()
    {
        if (target != null)
        {
            Vector3 distance = player.transform.position - target.transform.position;
        
            if (distance.sqrMagnitude <= 10000)
            {
                if (toggled)
                {

                    if (targetBody == null || targetBody.mass > playerBody.mass)
                    {
                        //Heavier object, so the player gets moved

                        //float dist = Vector3.Distance(thatWeight.transform.position, player.transform.position);
                        player.GetComponent<Rigidbody2D>().AddForce(
                            distance.normalized * -force,
                            ForceMode2D.Force);
                    }
                    else
                    {
                        target.GetComponent<Rigidbody2D>().AddForce(
                            distance.normalized * force,
                            ForceMode2D.Force);
                    }
                }
                else
                {
                    if (targetBody == null || targetBody.mass > playerBody.mass)
                    {
                        //Heavier object, so the player gets moved

                        player.GetComponent<Rigidbody2D>().AddForce(
                            distance.normalized * force,
                            ForceMode2D.Force);
                    }
                    else
                    {
                        target.GetComponent<Rigidbody2D>().AddForce(
                            distance.normalized * -force,
                            ForceMode2D.Force);
                    }
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
        if (player.IsPushing)
        {
            Toggled = !Toggled;
            player.IsPushing = false;
            target.GetComponent<SpriteRenderer>().color = Color.white;
            target = null;
            targetBody = null;
        }
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
