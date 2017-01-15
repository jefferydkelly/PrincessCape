using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIElement: MonoBehaviour {
	protected bool mouseOver = false;
	protected Vector2 max;
	protected Vector2 min;
	protected RectTransform rect;
	protected Rect myRect;
	public UnityEvent OnMouseOver;
	public UnityEvent OnMouseLeave;
	public UnityEvent OnClick;
	MenuController controller;
	void Start() {
		rect = GetComponent<RectTransform> ();
		controller = FindObjectOfType<MenuController> ();

	}
}
