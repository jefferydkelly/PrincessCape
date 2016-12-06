using UnityEngine;
using System.Collections;
using System;

public class MagnetGloves : UsableItem {

    bool toggled = false;
    public Sprite pushSprite;
    public Sprite pullSprite;
    // Use this for initialization


    public override void Activate()
    {
        //Shoot a ray fowards
        RaycastHit2D hit;
        hit = (Physics2D.Raycast(GameManager.Instance.Player.gameObject.transform.position, GameManager.Instance.Player.Aiming,
            100f, ~1<<LayerMask.NameToLayer("Player") | LayerMask.NameToLayer("SpellStatue") ));
        Debug.Log("Hit is what: " + hit.collider.name);
        if (hit.collider.gameObject.GetComponent<ObjectWeight>())
        {//first hit object has an ObjectWeight
            if(hit.collider.gameObject.GetComponent<Rigidbody2D>().constraints == RigidbodyConstraints2D.FreezeAll)
            {
                hit.collider.gameObject.GetComponent<Rigidbody2D>().constraints = ~RigidbodyConstraints2D.FreezePositionX;
            }
            if (toggled)
            {
                ObjectWeight thatWeight = hit.collider.gameObject.GetComponent<ObjectWeight>();
                if (thatWeight.objectWeight > GameManager.Instance.Player.gameObject.GetComponent<ObjectWeight>().objectWeight)
                {
                    //Heavier object, so the player gets moved
                    float dist = Vector3.Distance(thatWeight.gameObject.transform.position, GameManager.Instance.Player.gameObject.transform.position);
                    GameManager.Instance.Player.gameObject.GetComponent<Rigidbody2D>().AddForce(
                        new Vector2(dist * GameManager.Instance.Player.Forward.x, 0).normalized * (2500),
                        ForceMode2D.Force);
                }
                else
                {
                    float dist = Vector3.Distance(thatWeight.gameObject.transform.position, GameManager.Instance.Player.gameObject.transform.position);
                    thatWeight.gameObject.GetComponent<Rigidbody2D>().AddForce(
                        new Vector2(dist * -GameManager.Instance.Player.Forward.x, 0).normalized * (1000),
                        ForceMode2D.Force);
                }
            }
            else
            {
                ObjectWeight thatWeight = hit.collider.gameObject.GetComponent<ObjectWeight>();
                if (thatWeight.objectWeight > GameManager.Instance.Player.gameObject.GetComponent<ObjectWeight>().objectWeight)
                {
                    //Heavier object, so the player gets moved
                    float dist = Vector3.Distance(thatWeight.gameObject.transform.position, GameManager.Instance.Player.gameObject.transform.position);
                    GameManager.Instance.Player.gameObject.GetComponent<Rigidbody2D>().AddForce(
                        new Vector2(dist * -GameManager.Instance.Player.Forward.x, 0).normalized * (2500),
                        ForceMode2D.Force);
                }
                else
                {
                    float dist = Vector3.Distance(thatWeight.gameObject.transform.position, GameManager.Instance.Player.gameObject.transform.position);
                    thatWeight.gameObject.GetComponent<Rigidbody2D>().AddForce(
                        new Vector2(dist * GameManager.Instance.Player.Forward.x, 0).normalized * (1000),
                        ForceMode2D.Force);
                }
            }
            StartCoroutine(toggleLater(hit, 1f));
        }
      
      
    }

    IEnumerator toggleLater(RaycastHit2D hit, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        //if (hit.collider.gameObject.GetComponent<Rigidbody2D>().constraints == ~RigidbodyConstraints2D.FreezePosition)
            hit.collider.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        Debug.Log("I'm here");
    }
    public override void Deactivate()
    {
        Toggled = !Toggled;
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
