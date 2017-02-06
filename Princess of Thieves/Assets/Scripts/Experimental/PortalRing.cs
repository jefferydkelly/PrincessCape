using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalRing : UsableItem {
    [SerializeField]
    GameObject portalPrefab;
    List<GameObject> portals;
    Player player;

    // Use this for initialization
    void Start ()
    {
        portals = new List<GameObject>();
        player = GameManager.Instance.Player;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public override void Activate()
    {
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, player.Aiming, 10f, ~1 << LayerMask.NameToLayer("Player"));
        if (hit)
        {
            //open a goddamn portal
            if (portals.Count >= 2)
            {
                Destroy(portals[0], 0.1f);
                portals.Remove(portals[0]);
            }
            GameObject port = Instantiate(portalPrefab, hit.point, transform.rotation);
            port.GetComponent<Portal>().otherPort = portals[0].GetComponent<Portal>();
            portals.Add(port);
        }
    }

    public override void Deactivate()
    {
       //no need I think
    }

    public override void Use()
    {
       
    }

}
