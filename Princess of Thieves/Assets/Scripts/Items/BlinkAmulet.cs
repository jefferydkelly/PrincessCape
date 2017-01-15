using UnityEngine;
using System;
//Bug---------------------------
//Doesn't seem to go on cooldown - 1/13/17
//Note: Doesn't always last four seconds either
//Jamming the button quickly overrides the cooldown apparently
public class BlinkAmulet : UsableItem {

    float cdTime;
    public override void Activate()
    {
        if (onCooldown)
        {
            Debug.Log("fizzles");
            return;
        }
        GameObject temp = (GameObject)Instantiate(gameObject, transform.position, transform.rotation);
        temp.AddComponent<Rigidbody2D>();
        temp.AddComponent<BoxCollider2D>();
        temp.GetComponent<BoxCollider2D>().isTrigger = true;
       // Debug.Log(GameManager.Instance.Player.Forward);
        temp.transform.position = new Vector2(transform.position.x + 5 * GameManager.Instance.Player.Forward.x, transform.position.y+0.5f);
        Collider2D[] cols = Physics2D.OverlapCircleAll(temp.transform.position, 1f);
        foreach (Collider2D col in cols)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                return;
            }
        }
        //didn't break
        GameManager.Instance.Player.transform.position = temp.transform.position;
        Destroy(temp);

    }

    //Is there an issue with this being called over and over?
    public override void Deactivate()
    {
        Debug.Log("Goes on Cooldown");
        if (!onCooldown)
        {
            onCooldown = true;
            cdTime = Time.time;
        }

        
        //WaitDelegate w = () => { onCooldown = false; Debug.Log("I'm in a thing");}; //well it works
        //StartCoroutine(gameObject.RunAfter(w, cooldownTime));
    }

    public override void UseMana()
    {
        throw new NotImplementedException();
    }

    // Use this for initialization
    void Start () {
	    
	}   
	
	// Update is called once per frame
	void Update () {
        Debug.Log(onCooldown + " : Cooldown");  
        transform.position = GameManager.Instance.Player.transform.position;
        if(Time.time - cdTime >= 4)
        {
            onCooldown = false;
        }
    }
}
