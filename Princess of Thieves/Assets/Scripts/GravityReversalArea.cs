using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GravityReversalArea : MonoBehaviour, ActivateableObject {

	bool active = true;
	List<Rigidbody2D> touching = new List<Rigidbody2D>();

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Z))
		{
			if (active)
			{
				Deactivate();
			}
			else {
				Activate();
			}
		}
	}
	void OnTriggerEnter2D(Collider2D col)
	{
		Rigidbody2D rb = col.GetComponent<Rigidbody2D>();

		if (rb)
		{
			if (IsActive)
			{

				rb.ReverseGravity();
			}

			touching.Add(rb);
		}
		
	}

	void OnTriggerExit2D(Collider2D col)
	{
		
		Rigidbody2D rb = col.GetComponent<Rigidbody2D>();

		if (rb && touching.Contains(rb))
		{
			if (IsActive)
			{
				rb.ReverseGravity();
			}

			touching.Remove(rb);
		}
	}

	public void Activate()
	{
		if (!active)
		{
			active = true;

			foreach (Rigidbody2D rb in touching)
			{
				rb.ReverseGravity();
			}
		}
	}

	public void Deactivate()
	{
		if (active)
		{
			active = false;

			foreach (Rigidbody2D rb in touching)
			{
				rb.ReverseGravity();
			}
		}
	}

	public bool IsActive
	{
		get
		{
			return active;
		}
	}
}
