using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HowToPlayScreen : MenuController {

	// Use this for initialization
	void Start () {
        GetElements();
        Controller c = new Controller();
        GameObject.Find("MoveText").GetComponent<Text>().text = c.MovementInfo;
        GameObject.Find("ActionText").GetComponent<Text>().text = c.ActionInfo;
    }
}
