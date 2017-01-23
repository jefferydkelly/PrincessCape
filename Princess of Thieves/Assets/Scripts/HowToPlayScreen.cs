using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HowToPlayScreen : MenuController {

	// Use this for initialization
	void Start () {
        GameObject.Find("MoveText").GetComponent<Text>().text = TheController.MovementInfo;
        GameObject.Find("ActionText").GetComponent<Text>().text = TheController.ActionInfo;
		current = this;
		GameObject curSelected = EventSystem.current.firstSelectedGameObject;
		if (curSelected) {
			selected = curSelected.GetComponent<Button> ();
		}

		inputDelegate = () => {
			CheckInput();
		};
		inputTimer = new Timer(inputDelegate, 0.25f, true);
		if (buttons.Count > 0) {
			IndexOfSelected = 0;
		}
    }
}
