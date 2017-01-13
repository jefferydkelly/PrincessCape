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
	public UnityEvent OnMouseOver;
	public UnityEvent OnMouseLeave;
	public UnityEvent OnClick;
	MenuController controller;
	void Start() {
		rect = GetComponent<RectTransform> ();
		min = (Vector2)rect.transform.position - rect.rect.size / 2;
		max = (Vector2)rect.transform.position + rect.rect.size / 2;
		controller = FindObjectOfType<MenuController> ();

	}
	void Update() {
		
		Vector2 mousePos = Input.mousePosition;
		bool newMouseOver = mousePos.x.Between (min.x, max.x) && mousePos.y.Between (min.y, max.y);
		if (newMouseOver) {
			
		
			if (Input.GetMouseButtonDown (0)) {
				OnClick.Invoke ();
			} else {
				if (!mouseOver) {
					if (controller) {
						controller.Selected = this;
					} else {
						OnMouseOver.Invoke ();
					}
				}
			}
		} else if (mouseOver) {
			OnMouseLeave.Invoke ();
		}
		mouseOver = newMouseOver;
	}
	public virtual void Click()
	{
		Debug.Log("I was clicked");
	}

	public virtual void Selected()
	{
		GetComponent<Image>().color = Color.red;
		GetComponentInChildren<Text>().color = Color.white;
	}

	public virtual void Unselected()
	{
		GetComponent<Image>().color = Color.white;
		GetComponentInChildren<Text>().color = Color.black;
	}

	public void ChangeScene(string newScene) {
		SceneManager.LoadScene (newScene);
	}

	public void Quit() {
		Application.Quit ();
	}
}
