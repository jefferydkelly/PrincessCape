using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIElement: MonoBehaviour{
	bool mouseOver = false;
	public void MouseOver() {
		if (!mouseOver) {
			mouseOver = true;
			MenuController.Current.Selected = GetComponent<Button> ();
		}
	}

	public void MouseExit() {
		mouseOver = false;
	}
}
