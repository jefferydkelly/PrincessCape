using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignController : MonoBehaviour, InteractiveObject {
	[SerializeField]
	TextAsset sourceFile;
	SpriteRenderer myRenderer;

	public void Start() {
		myRenderer = GetComponent<SpriteRenderer> ();
	}
	public void Interact() {
		myRenderer.material.color = Color.white;
        UIManager.Instance.HideMessage();
		GameManager.Instance.StartCutscene (sourceFile);
	}

	public void Highlight() {
		UIManager.Instance.ShowInteraction ("Read");
		myRenderer.material.color = Color.blue;
	}

	public void Dehighlight() {
		UIManager.Instance.HideInteraction ();
		myRenderer.material.color = Color.white;
	}

	void OnMouseEnter() {
		if (!GameManager.Instance.IsPaused) {
			if (GameManager.Instance.InPlayerInteractRange (gameObject)) {
				Highlight ();
			}
		}
	}

	void OnMouseExit() {
		if (!GameManager.Instance.IsPaused) {
			Dehighlight ();
		}
	}

	void OnMouseOver() {
		if (!GameManager.Instance.IsPaused) {
			if (Highlighted) {
				if (GameManager.Instance.InPlayerInteractRange (gameObject)) {
					if (GameManager.Instance.Player.Controller.Interact) {
						Interact ();
						Input.ResetInputAxes ();
					}
				} else {
					Dehighlight ();
				}
			} else {
				Highlight ();
			}
		}
	}

	bool Highlighted {
		get {
			return myRenderer.material.color == Color.blue;
		}
	}
}
