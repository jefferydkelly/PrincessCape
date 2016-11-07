using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIElement: MonoBehaviour {

	public virtual void Click()
	{
		Debug.Log("I was clicked");
	}

	public virtual void Selected()
	{
		GetComponent<Image>().color = Color.blue;
		GetComponentInChildren<Text>().color = Color.white;
	}

	public virtual void Unselected()
	{
		GetComponent<Image>().color = Color.white;
		GetComponentInChildren<Text>().color = Color.black;
	}
}
