using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour {
	Image myImage;
	Vector2 dimensions = new Vector2(32, 32);
	Vector2 worldPosition = Vector2.zero;
	bool mouseOver = false;
	InventoryMenu menu;
	// Use this for initialization
	void Awake () {
		myImage = GetComponent<Image> ();
		worldPosition = transform.position;
		menu = GetComponentInParent<InventoryMenu> ();
	}

	void OnEnable() {
		worldPosition = transform.position;
	}

	void Update() {
		Vector2 dif = (Vector2)Input.mousePosition - worldPosition;

		if (Mathf.Abs (dif.x) <= dimensions.x && Mathf.Abs (dif.y) <= dimensions.y) {
			if (!mouseOver) {
				OnMouseEnter ();
			} else if (Input.GetMouseButtonDown (0)) {
				//Assign to left item
				menu.EquipItem(this, true);
			} else if (Input.GetMouseButtonDown (1)) {
				//Assign to the right item
				menu.EquipItem(this, false);
			}
		} else if (mouseOver) {
			OnMouseExit ();
		}

	}
	void OnMouseEnter() {
		if (myImage.sprite) {
			myImage.color = Color.blue;
			mouseOver = true;
		}
	}

	void OnMouseExit() {
		if (myImage.sprite) {
			myImage.color = Color.white;
			mouseOver = false;
		}
	}

	public Image Image {
		get {
			return myImage;
		}
	}

	public Sprite Sprite {
		get {
			return myImage.sprite;
		}

		set {
			if (myImage) {
				myImage.sprite = value;
			}
		}
	}
}
