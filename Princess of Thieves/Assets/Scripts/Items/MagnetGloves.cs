using UnityEngine;
using System.Collections;
using System;

public class MagnetGloves : UsableItem {

    int range = 10;
    bool toggled = false;
    public Sprite pushSprite;
    public Sprite pullSprite;
    public float force = 100;
    // Use this for initialization
    GameObject target;
    Rigidbody2D targetBody;
    Player player;
    Rigidbody2D playerBody;
    PushPullDirection direction;
    bool pushingOnTarget = true;
    LineRenderer lineRenderer;

    private void Start()
    {
        player = GameManager.Instance.Player;
        playerBody = player.GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }


    public override void Activate()
    {
        //Shoot a ray fowards
        RaycastHit2D hit;
        hit = (Physics2D.Raycast(player.transform.position, player.Aiming,
            range, 1<<LayerMask.NameToLayer("Metal") ));

        if (hit)
        {//first hit object has an ObjectWeight

            target = hit.collider.gameObject;
            Color col = toggled ? Color.blue : Color.red;
            target.GetComponent<SpriteRenderer>().color = col;
            lineRenderer.enabled = true;
            lineRenderer.startColor = col;
            lineRenderer.endColor = col;
            lineRenderer.SetPositions(new Vector3[]{ player.transform.position, target.transform.position});
            targetBody = target.GetComponent<Rigidbody2D>();
            if (targetBody)
            {
                pushingOnTarget = targetBody.mass < playerBody.mass;
                targetBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            player.IsUsingMagnetGloves = true;

            if (player.Aiming.y == 1)
            {
                direction = PushPullDirection.Up;
            } else if (player.Aiming.y == -1)
            {
                direction = PushPullDirection.Down;
            } else if (player.Aiming.x == 1)
            {
                direction = PushPullDirection.Right;
            } else
            {
                direction = PushPullDirection.Left;
            }
        }
            
      
    }

    public void Use()
    {
        if (target != null)
        {
            Vector3 distance = player.transform.position - target.transform.position;
            Vector2 moveDir = Vector3.zero;
            
            if (distance.sqrMagnitude <= range * range)
            {
                if (direction == PushPullDirection.Up)
                {
                    moveDir = Vector2.up;
                }
                else if (direction == PushPullDirection.Down)
                {
                    moveDir = Vector2.down;
                }
                else
                {
                    moveDir = new Vector2(direction == PushPullDirection.Right ? 1 : -1, 0);
                }
                if (pushingOnTarget && (direction == PushPullDirection.Up || direction == PushPullDirection.Down))
                {
                    moveDir += player.Aiming.XVector();
                }

                moveDir.Normalize();
                if (toggled)
                {

                    if (pushingOnTarget)
                    {
                        //Heavier object, so the player gets moved

                        playerBody.AddForce(
                            moveDir * force,
                            ForceMode2D.Force);
                    }
                    else
                    {
                        targetBody.AddForce(
                            moveDir * -force,
                            ForceMode2D.Force);
                    }
                }
                else
                {
                    if (pushingOnTarget)
                    {
                        //Heavier object, so the player gets moved
                        moveDir.y *= -1;
                        playerBody.AddForce(
                            moveDir * force,
                            ForceMode2D.Force);
                    }
                    else
                    {
                        targetBody.AddForce(
                            moveDir * force,
                            ForceMode2D.Force);
                    }
                }
            }

            lineRenderer.SetPositions(new Vector3[] { player.transform.position, target.transform.position });
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
        if (player.IsUsingMagnetGloves)
        {
            
            player.IsUsingMagnetGloves = false;
            target.GetComponent<SpriteRenderer>().color = Color.white;
            if (targetBody)
            {
                targetBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                targetBody = null;
            }
            target = null;
            pushingOnTarget = true;
            lineRenderer.enabled = false;
        }

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

public enum PushPullDirection
{
    Up, Down, Left, Right
}
