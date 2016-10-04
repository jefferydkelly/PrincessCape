using UnityEngine;
using System.Collections;

public class EarthProjectile : SpellProjectile {

	// Use this for initialization
	float riseTime = 1.0f;
	float stayTime = 1.0f;
	float pushForce = 25.0f;
	bool rising = false;

	void Start () {
		SpriteRenderer sr = gameObject.AddComponent<SpriteRenderer>();
		sr.sprite = Resources.Load<Sprite>("Sprites/StoneWall");
		gameObject.AddComponent<BoxCollider2D>();
		Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
		rb.gravityScale = 0;
		rb.freezeRotation = true;
		transform.localScale = new Vector3(1, 0, 1);
		StartCoroutine(Rise());
	}

	IEnumerator Rise()
	{
		float totalTime = 0.0f;
		float tPer = 0.0f;
		rising = true;
		do
		{
			totalTime += Time.deltaTime;
			tPer = totalTime / riseTime;
			Vector3 lScale = transform.localScale;
			lScale.y = tPer;
			transform.localScale = lScale;
			yield return null;
		} while (totalTime < riseTime);
		transform.localScale = new Vector3(1, 1, 1);
		rising = false;
		Invoke("Crumble", stayTime);
		yield return null;
	}

	void Crumble()
	{
		Destroy(gameObject);	
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (rising)
		{
			Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();

			if (rb)
			{
				rb.AddForce(Vector2.up * pushForce, ForceMode2D.Impulse);
			}
		}
	}
}
