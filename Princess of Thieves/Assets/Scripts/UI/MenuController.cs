using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MenuController : MonoBehaviour {

	List<UIElement> elements;
	UIElement selected = null;
	// Use this for initialization
	float waitTime = 0.2f;
	float timeWaited = 0.0f;
	void Start () {
        GetElements();
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

		if (Input.GetKeyDown(KeyCode.Return))
		{
			Selected.Click();
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			SelectedElement--;
			timeWaited = 0;
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			SelectedElement++;
			timeWaited = 0;
		} else if (timeWaited >= waitTime)
		{
			timeWaited -= waitTime;

			if (Input.GetKey(KeyCode.DownArrow))
			{
				SelectedElement--;
			}
			else if (Input.GetKey(KeyCode.UpArrow))
			{
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
				selected.Unselected();
			}

			selected = value;
			selected.Selected();
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
