using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightCorpse : JDMappableObject, InteractiveObject {

	SpriteRenderer myRenderer;
	Color startColor;
	[SerializeField]
	TextAsset cutsceneFile;

	private void Start()
	{
		myRenderer = GetComponent<SpriteRenderer>();
		startColor = myRenderer.color;
	}

	public void Interact()
	{
		myRenderer.color = startColor;
		UIManager.Instance.ShowInteraction("");
		GameManager.Instance.StartCutscene (cutsceneFile);
	}

	public void Highlight()
	{
		UIManager.Instance.ShowInteraction("Loot");
		myRenderer.color = Color.blue;
		//myRenderer.sprite = highlightedSprite;
	}

	public void Dehighlight()
	{
		UIManager.Instance.ShowInteraction("");
		myRenderer.color = startColor;
		//myRenderer.sprite = closedSprite;
	}

	void OnDestroy() {
        Player p = GameManager.Instance.Player;
        if (p != null)
        {
            p.HighlightedDestroyed(this);
        }
	}

	bool Highlighted {
		get {
			return myRenderer.color == Color.blue;
		}
	}

	void OnMouseEnter() {
		if (!GameManager.Instance.IsPaused) {
			Highlight ();
		}
	}

	void OnMouseExit() {
		if (!GameManager.Instance.IsPaused) {
			Dehighlight ();
		}
	}

	void OnMouseOver() {
		if (Highlighted) {
			if (GameManager.Instance.InPlayerInteractRange (gameObject)) {
				if (GameManager.Instance.Player.Controller.Interact) {
					Interact ();
					Input.ResetInputAxes ();
				}
			} else {
				Dehighlight ();
			}
		}
	}
}