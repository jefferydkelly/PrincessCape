using UnityEngine;
using System.Collections;

public class SeedShooter : UsableItem {
    public GameObject seedObj;
    bool aiming = false;
    public override void Use()
    {
        Debug.Log("Can I be used?: " + !onCooldown);
        if (!onCooldown)
        {
            aiming = true;
            GameManager.Instance.Player.IsFrozen = true;
            
            
        }
    }

    private void Update()
    {
        if (aiming && Input.GetKeyUp(KeyCode.F))
        {
            aiming = false;
            Player p = GameManager.Instance.Player;
            p.IsFrozen = false;
            GameObject seed = Instantiate(seedObj);
            seed.transform.position = p.transform.position;
            seed.GetComponent<Rigidbody2D>().AddForce(p.Aiming * 20, ForceMode2D.Impulse);
            onCooldown = true;
            WaitDelegate w = () => { onCooldown = false; };
            StartCoroutine(gameObject.RunAfter(w, cooldownTime));
        }
    }
}
