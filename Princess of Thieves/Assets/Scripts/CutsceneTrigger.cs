using UnityEngine;
using System.Collections;

public class CutsceneTrigger : MonoBehaviour {
	bool triggered = false;
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Player"))
		{
			if (!triggered)
			{
				triggered = true;
				GameManager.Instance.StartCutscene("testCutscene");
			}
		}
	}
}
