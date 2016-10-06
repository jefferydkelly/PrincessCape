using UnityEngine;
using System.Collections;

public class EarthProjectile : SpellProjectile {

	// Use this for initialization
	float riseTime = 1.0f;
	float stayTime = 5.0f;
	float pushForce = 25.0f;
	PillarState state = PillarState.Rising;

	void Start () {
		SpriteRenderer sr = gameObject.AddComponent<SpriteRenderer>();
		sr.sprite = Resources.Load<Sprite>("Sprites/StoneWall");
		gameObject.AddComponent<BoxCollider2D>();
		Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
		rb.gravityScale = 0;
		rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
		transform.localScale = new Vector3(1, 0, 1);
		StartCoroutine(Rise());
	}

	IEnumerator Rise()
	{
		float totalTime = 0.0f;
		float tPer = 0.0f;
		state = PillarState.Rising;
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
		GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
		state = PillarState.Standing;
		Invoke("Crumble", stayTime);
		yield return null;
	}

	IEnumerator Melt()
	{
		float totalTime = 0.0f;
		float tPer = 0.0f;
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		rb.gravityScale = 1.0f;
		rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
		do
		{
			totalTime += Time.deltaTime;
			tPer = 1 - (totalTime / riseTime);
			Vector3 lScale = transform.localScale;
			lScale.y = tPer;
			transform.localScale = lScale;
			yield return null;
		} while (totalTime < riseTime);

		state = PillarState.Standing;
		Invoke("Crumble", stayTime);
		yield return null;
	}

	void Crumble()
	{
		Destroy(gameObject);	
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (state == PillarState.Rising)
		{
			Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();

			if (rb)
			{
				rb.AddForce(Vector2.up * pushForce, ForceMode2D.Impulse);
			}

			DamageableObject dObj = col.gameObject.GetComponent<DamageableObject>();

			if (dObj != null)
			{
				dObj.TakeDamage(new DamageSource(DamageType.Earth, damage, allegiance));
			}
		}
		else if (state == PillarState.Standing)
		{
			if (col.gameObject.GetComponent<WaterProjectile>() || col.gameObject.GetComponent<FireballProjectile>())
			{
				Destroy(col.gameObject);
				CancelInvoke();
				state = PillarState.Melting;
				StartCoroutine(Melt());
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (state == PillarState.Standing)
		{
			WindProjectile wp = col.gameObject.GetComponent<WindProjectile>();

			if (wp)
			{
				Rigidbody2D mine = GetComponent<Rigidbody2D>();
				mine.constraints = RigidbodyConstraints2D.FreezeRotation;
				mine.gravityScale = 1.0f;
				mine.AddForce(wp.FWD * 10, ForceMode2D.Impulse);
			}
		}
	}
}

public enum PillarState
{
	Rising, Standing, Melting
}
