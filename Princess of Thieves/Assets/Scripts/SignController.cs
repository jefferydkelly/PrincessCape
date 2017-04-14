using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignController : MonoBehaviour, InteractiveObject {
	[SerializeField]
	TextAsset sourceFile;
  
	SpriteRenderer myRenderer;

	public void Start() {
		myRenderer = GetComponent<SpriteRenderer> ();
	}

    private void Update()
    {
        if (!GameManager.Instance.IsPaused && GameManager.Instance.Player.Controller.Interact && Highlighted)
        {
            Interact();
        }
    }

    public void Interact() {
		myRenderer.material.color = Color.white;
        UIManager.Instance.HideMessage();
		GameManager.Instance.StartCutscene (sourceFile);
	}

	public void Highlight() {
		UIManager.Instance.ShowInteraction ("Read");
		myRenderer.material.color = Color.blue;
	}

	public void Dehighlight() {
        UIManager.Instance.HideInteraction ();
		myRenderer.material.color = Color.white;
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!GameManager.Instance.IsPaused && !Highlighted)
            {
                Highlight();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!GameManager.Instance.IsPaused)
            {
                Dehighlight();
            }
        }
    }

	bool Highlighted {
		get {
			return myRenderer.material.color == Color.blue;
		}
	}
}
