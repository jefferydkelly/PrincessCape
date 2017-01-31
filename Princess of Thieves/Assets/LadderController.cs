using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : JDMappableObject, InteractiveObject {
	SpriteRenderer myRenderer;

	void Start() {
		myRenderer = GetComponent<SpriteRenderer> ();
	}
	public void Interact() {
		Player player = GameManager.Instance.Player;
		if (!player.IsClimbing) {
			player.IsClimbing = true;
			player.transform.position = transform.position - new Vector3 (0, gameObject.HalfHeight () - player.HalfHeight);
			myRenderer.color = Color.white;
		} else {
			player.IsClimbing = false;
		}
	}

	public void Highlight() {
		if (!GameManager.Instance.Player.IsClimbing) {
			myRenderer.color = Color.blue;
			UIManager.Instance.ShowInteraction ("Climb");
		} else {
			UIManager.Instance.ShowInteraction ("Get Off");
		}
	}

	public void Dehighlight() {
		myRenderer.color = Color.white;
		UIManager.Instance.ShowInteraction ("");
	}
}
