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

    private void Update()
    {
        if (!GameManager.Instance.IsPaused && GameManager.Instance.Player.Controller.Interact && Highlighted)
        {
            Interact();
        }
    }
    public void Interact()
	{
		myRenderer.color = startColor;
		UIManager.Instance.ShowInteraction("");
		GameManager.Instance.StartCutscene (cutsceneFile);
	}

	public void Highlight()
	{
        CursorManager.Instance.State = CursorState.Sign;
		UIManager.Instance.ShowInteraction("Loot");
		myRenderer.color = Color.blue;
		//myRenderer.sprite = highlightedSprite;
	}

	public void Dehighlight()
	{
        CursorManager.Instance.State = CursorState.Normal;
        UIManager.Instance.ShowInteraction("");
		myRenderer.color = startColor;
		//myRenderer.sprite = closedSprite;
	}

	bool Highlighted {
		get {
			return myRenderer.color == Color.blue;
		}
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!GameManager.Instance.IsPaused)
            {
                Highlight();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!GameManager.Instance.IsPaused)
        {
            Dehighlight();
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!GameManager.Instance.IsPaused && !Highlighted)
            {
                Highlight();
            }
        }
    }
}