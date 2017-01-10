﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MenuController : MonoBehaviour {

	static Controller controller = null;
	List<UIElement> elements;
	UIElement selected = null;
	// Use this for initialization
	float waitTime = 0.2f;
	float timeWaited = 0.0f;
	void Start () {
		if (controller == null) {
			controller = new Controller ();
			GetElements ();
		}
        
	}
	
    protected void GetElements()
    {
        elements = FindObjectsOfType<UIElement>().ToList();
        elements.Reverse();
		if (elements.Count > 0)

        {
            Selected = elements[0];
        }
    }
	// Update is called once per frame
	void Update () {
		
		timeWaited += Time.deltaTime;

		if (controller.Jump) {
			Selected.OnClick.Invoke ();
		} else if (timeWaited >= waitTime) {
			timeWaited -= waitTime;

			if (controller.Vertical < 0) {
				SelectedElement--;
			} else if (controller.Vertical > 0) {
				SelectedElement++;
			}

		}


	}

	UIElement Selected
	{
		set
		{
			if (selected != null)
			{
				selected.OnMouseLeave.Invoke ();
			}

			selected = value;
			selected.OnMouseOver.Invoke ();
		}

		get
		{
			return selected;
		}
	}

	int SelectedElement
	{
		set
		{
			int val = value;

			while (val < 0)
			{
				val += elements.Count;
			}

			val %= elements.Count;
			Selected = elements[val];
		}

		get
		{
			return elements.IndexOf(selected);
		}
	}
}
