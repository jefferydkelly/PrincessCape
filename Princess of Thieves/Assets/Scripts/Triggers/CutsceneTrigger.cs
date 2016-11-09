using UnityEngine;
using System.Collections;

public class CutsceneTrigger : TriggerBase {
	public TextAsset cutscene;
	protected override void Trigger()
	{
		if (!triggered)
		{
			triggered = true;
			GameManager.Instance.StartCutscene(cutscene);
		}
	}
}
