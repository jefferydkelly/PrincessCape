﻿using System.Collections;
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
		GameManager.Instance.Player.HighlightedDestroyed (this);
	}
}