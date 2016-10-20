using UnityEngine;
using System.Collections;

public class GravityReversalArea : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col)
	{
		
		Rigidbody2D rb = col.GetComponent<Rigidbody2D>();

		if (rb)
		{
			rb.gravityScale *= -1;
			Debug.Log("New Gravity: " + rb.gravityScale);
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		
		Rigidbody2D rb = col.GetComponent<Rigidbody2D>();

		if (rb)
		{
			rb.gravityScale *= -1;
			Debug.Log("Resetting polarity back to: " + rb.gravityScale);
		}
	}
}
