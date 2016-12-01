using UnityEngine;
using System.Collections;
using System;

public class BlinkAmulet : UsableItem {

    public override void Activate()
    {

        GameObject temp = (GameObject)Instantiate(gameObject, transform.position, transform.rotation);
        temp.AddComponent<Rigidbody2D>();
        temp.AddComponent<BoxCollider2D>();
        temp.GetComponent<BoxCollider2D>().isTrigger = true;
        Debug.Log(GameManager.Instance.Player.Forward);
        temp.transform.position = new Vector2(transform.position.x + 5 * GameManager.Instance.Player.Forward.x, transform.position.y+0.5f);
        Collider2D[] cols = Physics2D.OverlapCircleAll(temp.transform.position, 1f);
        foreach (Collider2D col in cols)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                return;
            }
        }
        //didn't breadk
        Debug.Log("All gucci");
        GameManager.Instance.Player.transform.position = temp.transform.position;
        Destroy(temp);

    }

    public override void Deactivate()
    {
        
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
