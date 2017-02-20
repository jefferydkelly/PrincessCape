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
}
