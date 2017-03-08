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
		GameManager.Instance.StartCutscene (sourceFile);
	}

	public void Highlight() {
		UIManager.Instance.ShowInteraction ("Read");
		myRenderer.material.color = Color.blue;
	}

	public void Dehighlight() {
		UIManager.Instance.ShowInteraction ("");
		myRenderer.material.color = Color.white;
	}

	void OnMouseEnter() {
		if (!GameManager.Instance.IsInCutscene) {
			if (GameManager.Instance.InPlayerInteractRange (gameObject)) {
				Highlight ();
			}
		}
	}

	void OnMouseExit() {
		if (!GameManager.Instance.IsInCutscene) {
			Dehighlight ();
		}
	}

	void OnMouseOver() {
		if (!GameManager.Instance.IsInCutscene) {
			if (Highlighted) {
				if (GameManager.Instance.InPlayerInteractRange (gameObject)) {
					if (Input.GetMouseButtonDown (0)) {
						Interact ();
						Input.ResetInputAxes ();
					}
				} else {
					Dehighlight ();
				}
			}
		}
	}

	bool Highlighted {
		get {
			return myRenderer.material.color == Color.blue;
		}
	}
}
