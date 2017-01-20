using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour {

	protected static MenuController current;
	private static Controller controller = null;
	[SerializeField]
	protected List<Button> buttons;
	protected Button selected;
	bool jumpPushed = false;

	void Start() {
		current = this;
		GameObject curSelected = EventSystem.current.firstSelectedGameObject;
		if (curSelected) {
			selected = curSelected.GetComponent<Button> ();
		}
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
		int vert = TheController.Vertical;

		if (vert != 0) {
			IndexOfSelected -= vert;
		}

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

	public static MenuController Current {
		get {
			return current;
		}
	}

	public Button Selected {
		get {
			return selected;
		}

		set {
			Debug.Log ("Setting new selected");
			if (buttons.Contains (selected)) {
				Debug.Log ("We have it.");
				if (value != selected) {
					Debug.Log ("It's not the old one");
					selected.OnDeselect (new BaseEventData (EventSystem.current));
					selected = value;
					EventSystem.current.SetSelectedGameObject (Selected.gameObject);
				}
			}
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
			Debug.Log (index);
			Selected = buttons [index];
		}
	}
}
