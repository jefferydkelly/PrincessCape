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
       if (!(collision.OnLayer("Background") || collision.CompareTag("Player") || collision.TagContains("Enemy")))
        {
            Debug.Log(collision.gameObject.name);
            Destroy(gameObject);
        }
    }
	
}
