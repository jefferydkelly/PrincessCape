using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HowToPlayScreen : MenuController {

	// Use this for initialization
	void Start () {
		if (controller == null) {
			controller = new Controller ();
		}
        GameObject.Find("MoveText").GetComponent<Text>().text = controller.MovementInfo;
        GameObject.Find("ActionText").GetComponent<Text>().text = controller.ActionInfo;
		SelectedElement = 0;
    }
}
