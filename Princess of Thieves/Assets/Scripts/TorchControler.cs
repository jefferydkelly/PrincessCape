using UnityEngine;
using System.Collections;

public class TorchControler : JDMappableObject, BurnableObject {
	Light yagami;
	public Sprite onSprite;
	public Sprite offSprite;
	SpriteRenderer myRenderer;
	// Use this for initialization
	void Start () {
		yagami = GetComponent<Light>();
		myRenderer = GetComponent<SpriteRenderer>();
	}

	public void Burn()
	{
		yagami.enabled = true;
		myRenderer.sprite = onSprite;
	}

	public void Douse()
	{
		yagami.enabled = false;
		myRenderer.sprite = offSprite;

	}
}
