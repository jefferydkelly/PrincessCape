using UnityEngine;
using System.Collections;

public class QuitButton : UIElement {

	public override void Click()
	{
		Application.Quit();
	}
}
