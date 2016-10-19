using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class TriggerBase : MonoBehaviour {
	protected bool triggered = false;
	protected abstract void Trigger();

    void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Player"))
		{
			Trigger();
		}
	}
}
