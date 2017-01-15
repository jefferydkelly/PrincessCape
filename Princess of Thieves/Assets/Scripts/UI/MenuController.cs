using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	private static Controller controller = null;
	[SerializeField]
	protected List<Button> buttons;
	Button selected;
	bool jumpPushed = false;
	void Start() {
		if (buttons.Count > 0) {
			WaitDelegate wd = () => {
				CheckInput ();
			};
			StartCoroutine (gameObject.RunAfterRepeatingUI (wd, 0.2f));

		}
	}

	void Update() {
		if (TheController.Jump) {
			jumpPushed = true;
		}
	}

	protected void CheckInput() {
		if (jumpPushed) {
			Selected.onClick.Invoke ();
		}
		IndexOfSelected -= TheController.Vertical;
	}
	public void ChangeScene(string sceneName) {
		SceneManager.LoadScene(sceneName);
	}

	public void Quit() {
		Application.Quit ();
	}

	public static Controller TheController {
		get {
			if (controller == null) {
				controller = new Controller ();
			}

			return controller;
		}
	}
	protected Button Selected {
		get {
			return selected;
		}

		set {
			selected = value;
			selected.Select ();
		}
	}
	protected int IndexOfSelected {
		get {
			if (selected == null) {
				return 0;
			}
			return buttons.IndexOf (selected);
		}

		set {
			int index = value;
			while (index < 0) {
				index += buttons.Count;
			}

			index %= buttons.Count;
			Selected = buttons [index];
		}
	}
}
