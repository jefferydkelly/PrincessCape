using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBox {

	GameObject background;
	Image bgImg;
	Image itemRenderer;
	Text keyText;

	public ItemBox(string s)
	{
		background = GameObject.Find(s);
		bgImg = background.GetComponentsInChildren<Image>()[0];
		itemRenderer = background.GetComponentsInChildren<Image>()[1];
		keyText = background.GetComponentInChildren<Text>();
	}

	public Sprite ItemSprite
	{
		get
		{
			return itemRenderer.sprite;
		}

		set
		{
			itemRenderer.sprite = value;
		}
	}

	public Color BGColor {
		get {
			return bgImg.color;
		}

		set {
			bgImg.color = value;
			itemRenderer.color = value;
		}
	}

	public bool Enabled
	{
		get
		{
			return background.activeSelf;
		}

		set
		{
			background.SetActive(value);
		}
	}

	public string Key
	{
		get
		{
			return keyText.text;
		}

		set
		{
			keyText.text = value;
		}
	}
}
