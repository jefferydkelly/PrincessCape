using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GoToSceneButton : UIElement {
	public string scene;
	public override void Click()
	{
		SceneManager.LoadScene(scene);
	}
}
