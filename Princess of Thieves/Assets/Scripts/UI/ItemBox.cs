using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBox {

	protected GameObject background;
	protected Image bgImg;
	protected Image itemRenderer;
	protected Text keyText;

	public ItemBox(string s)
	{
		background = GameObject.Find(s);
		bgImg = background.GetComponentsInChildren<Image>()[0];
		itemRenderer = background.GetComponentsInChildren<Image>()[1];
		keyText = background.GetComponentsInChildren<Text>()[0];
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

public class ComboBox : ItemBox {
	Text interactText;
	public ComboBox(string s): base(s) {
		interactText = background.GetComponentsInChildren<Text> () [1];
		interactText.enabled = false;
	}

	public void ShowInteraction(string s) {
		itemRenderer.enabled = false;
		interactText.enabled = true;
		interactText.text = s;
	}

	public void HideInteraction() {
		interactText.enabled = false;
		itemRenderer.enabled = true;
	}

	public bool IsShowingInteraction {
		get {
			return interactText.enabled;
		}
	}
}
