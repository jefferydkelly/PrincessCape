using UnityEngine;
using System.Collections;

public class EarthProjectile : SpellProjectile {

	// Use this for initialization
	float riseTime = 1.0f;
	float stayTime = 5.0f;
	float pushForce = 25.0f;
	PillarState state = PillarState.Rising;
	public CasterObject caster;
	bool isSliding = false;
	Rigidbody2D myRigidbody;
	void Start () {
		name = "Stone Pillar";
		SpriteRenderer sr = gameObject.AddComponent<SpriteRenderer>();
		sr.sprite = Resources.Load<Sprite>("Sprites/StoneWall");
		gameObject.AddComponent<BoxCollider2D>();
		myRigidbody = gameObject.AddComponent<Rigidbody2D>();
		myRigidbody.gravityScale = 0;
		myRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
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
		myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
		state = PillarState.Standing;
		Invoke("Crumble", stayTime);
		yield return null;
	}

	IEnumerator Melt()
	{
		float totalTime = 0.0f;
		float tPer = 0.0f;
		myRigidbody.gravityScale = 1.0f;
		myRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
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
		if (isSliding && !col.collider.CompareTag("Platform"))
		{
			IsSliding = false;
		}
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
				IsSliding = true;
				myRigidbody.AddForce(wp.FWD * 10, ForceMode2D.Impulse);
			}
		}

		if (isSliding && !col.CompareTag("Platform"))
		{
			IsSliding = false;
		}
	}

	private bool IsSliding
	{
		get
		{
			return isSliding;
		}

		set
		{
			isSliding = value;

			if (isSliding)
			{
				myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
				myRigidbody.gravityScale = 1.0f;
			}
			else {
				myRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
				if (state == PillarState.Standing)
				{
					myRigidbody.constraints |= RigidbodyConstraints2D.FreezePositionY;
					myRigidbody.gravityScale = 0f;
				}

			}
		}
	}
	public override void Diminish()
	{
		IsSliding = true;
		myRigidbody.AddForce((caster.Position - transform.position).normalized * pushForce, ForceMode2D.Impulse);
	}

	public override void Enhance()
	{
		//This will do something
	}
}

[System.Flags]
public enum PillarState
{
	Rising, Standing, Melting
}
