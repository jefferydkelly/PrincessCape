using UnityEngine;
using System.Collections;

public class AreaLoadTrigger : TriggerBase{

	public float loadFwd = 1;
	public string areaToLoad;
	protected override void Trigger()
	{
		if (GameManager.Instance.Player.Forward.x / loadFwd > 0)
		{
			//LoadArea
			GameManager.Instance.LoadScene(areaToLoad);
			StartCoroutine(GameManager.Instance.SetActiveScene(areaToLoad));
		}
	}

	void OnTriggerExit2D()
	{
		if (GameManager.Instance.Player.Forward.x / loadFwd < 0)
		{
			StartCoroutine(GameManager.Instance.UnloadScene(areaToLoad));
		}
	}
}
