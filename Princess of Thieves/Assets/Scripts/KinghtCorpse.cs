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

	private void Start()
	{
		myRenderer = GetComponent<SpriteRenderer>();
	}

	public void Interact()
	{
		if (!looted)
		{
			UIManager.Instance.ShowInteraction("");
			GameManager.Instance.Player.AddItem(contents);
			//myRenderer.sprite = openedSprite;
			looted = true;
		}
	}

	public void Highlight()
	{
		if (!looted)
		{
			UIManager.Instance.ShowInteraction("Loot");
			//myRenderer.sprite = highlightedSprite;
		}
	}

	public void Dehighlight()
	{
		if (!looted)
		{
			UIManager.Instance.ShowInteraction("");
			//myRenderer.sprite = closedSprite;
		}
	}
}
