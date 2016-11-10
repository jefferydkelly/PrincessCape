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
        Flip();
        faceLeft = false; // faces right
        //fwdX = 1; //still right
    }
	// Update is called once per frame
	void FixedUpdate () {

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
                newXY.x += 0.001f;
                newXY2.x += 0.001f;
                newXY3.x += 0.001f;
                //newXY.y += 0.001f;
                newXY2.y += 0.001f;
                newXY3.y -= 0.001f;
            }
            else
            {
                newXY.x -= 0.001f;
                newXY2.x -= 0.001f;
                newXY3.x -= 0.001f;
                //newXY.y += 0.001f;
                newXY2.y += 0.001f;
                newXY3.y -= 0.001f;
            }
            lightningEnds[0].gameObject.transform.position = newXY;
            lightningEnds[1].gameObject.transform.position = newXY2;
            lightningEnds[2].gameObject.transform.position = newXY3;
        }
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Player"))
		{
			GameManager.Instance.Player.TakeDamage(new DamageSource(DamageType.Fire, 10, Allegiance.Enemy));
			Destroy(gameObject);
		}
	}

    private void Flip()
    {
        transform.localScale = transform.localScale * fwdX;
        if (fwdX > 0)
        {
            fwdX = ~fwdX;
            
        }
        else
        {
            fwdX = ~fwdX;
        }
    }
}
