using UnityEngine;
using System.Collections;

public class BounceSeed : MonoBehaviour
{

    bool bounced = false;
    // Use this for initialization
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            if (!bounced && collision.OnLayer("Platforms"))
            {
                bounced = true;
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                if (collision.TagContains("Gate"))
                {
                    rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
                    return;
                }
                else if (collision.TagContains("Platform"))
                {
                    rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
                    return;
                } 
            }

            if (!collision.OnLayer("Background"))
            {

                Destroy(gameObject);
            }

        }
    }
}
