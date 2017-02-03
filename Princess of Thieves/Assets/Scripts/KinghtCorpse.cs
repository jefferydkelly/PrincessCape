using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinghtCorpse : JDMappableObject, InteractiveObject {

	public Sprite openedSprite;
	public Sprite closedSprite;
	public Sprite highlightedSprite;
	bool looted = false;
	[SerializeField]
	GameObject contents;
	SpriteRenderer myRenderer;
	Color startColor;

	private void Start()
	{
		myRenderer = GetComponent<SpriteRenderer>();
		startColor = myRenderer.color;
	}

	public void Interact()
	{
		if (!looted)
		{
			UIManager.Instance.ShowInteraction("");
			GameManager.Instance.Player.AddItem(contents);
			myRenderer.color = startColor;
			if (openedSprite != null) {
				myRenderer.sprite = openedSprite;
			}
			looted = true;
			enabled = false;
		}
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
