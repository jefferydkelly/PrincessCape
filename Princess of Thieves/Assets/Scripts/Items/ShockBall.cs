using UnityEngine;
using System.Collections;

public class ShockBall : MonoBehaviour {

    Vector3 fwd;
    float speed = 20;
	// Use this for initialization
	void Start () {
        Player p = GameManager.Instance.Player;
        transform.position = p.transform.position;
        fwd = p.Forward;
	}

    void Update()
    {
        transform.position += fwd * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TagContains("Enemy"))
        {
            //Get the enemy and shock them
            Debug.Log("Hit an enemy");
            Destroy(gameObject);
        } else if (!collision.OnLayer("Background") && !collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
	
}
