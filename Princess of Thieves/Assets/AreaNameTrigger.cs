using UnityEngine;
using System.Collections;

public class AreaNameTrigger : TriggerBase {

	public string areaName = "Entrance Hall";
	protected override void Trigger()
	{
		UIManager.Instance.EnterArea(areaName);
	}
}
