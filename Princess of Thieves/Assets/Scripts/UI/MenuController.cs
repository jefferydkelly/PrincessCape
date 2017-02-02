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
	protected WaitDelegate inputDelegate;
	protected Timer inputTimer;

	void Start() {
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
			inputTimer.Start ();
		}
	}
		
	protected void CheckInput() {
		if (TheController.Submit) {
			Selected.onClick.Invoke ();
		}
		int vert = TheController.Vertical;

		if (vert != 0) {
			IndexOfSelected -= vert;
		}

	}
	public void ChangeScene(string sceneName) {
		inputTimer.Stop ();
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
			if (buttons.Contains (selected)) {
				if (value != selected) {
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
			Selected = buttons [index];
		}
	}
}
