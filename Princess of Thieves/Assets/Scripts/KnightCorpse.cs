using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightCorpse : JDMappableObject, InteractiveObject {

	bool looted = false;
	[SerializeField]
	GameObject contents;
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
		if (!looted)
		{
			myRenderer.color = startColor;
			looted = true;
			UIManager.Instance.ShowInteraction("");
			GameManager.Instance.StartCutscene (cutsceneFile);
		}
	}

	void GiveItem() {
		GameManager.Instance.Player.AddItem(contents);
	}

	public void Highlight()
	{
		if (!looted)
		{
			UIManager.Instance.ShowInteraction("Loot");
			myRenderer.color = Color.blue;
			//myRenderer.sprite = highlightedSprite;
		}
	}

	public void Dehighlight()
	{
		if (!looted)
		{
			UIManager.Instance.ShowInteraction("");
			myRenderer.color = startColor;
			//myRenderer.sprite = closedSprite;
		}
	}
}