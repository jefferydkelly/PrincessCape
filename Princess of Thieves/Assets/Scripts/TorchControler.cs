using UnityEngine;
using System.Collections;

public class TorchControler : JDMappableObject, BurnableObject {
	Light yagami; //my god
	public Sprite onSprite;
	public Sprite offSprite;
	SpriteRenderer myRenderer;
	// Use this for initialization
	void Start () {
		yagami = GetComponent<Light>();
		myRenderer = GetComponent<SpriteRenderer>();
	}

    void Update()
    {
        yagami.range = Random.Range(9, 12);
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
