using UnityEngine;
using System.Collections;

public class CutsceneTrigger : TriggerBase {
	public string cutsceneName = "testCutscene";
	protected override void Trigger()
	{
		if (!triggered)
		{
			triggered = true;
			GameManager.Instance.StartCutscene(cutsceneName);
		}
	}
}
