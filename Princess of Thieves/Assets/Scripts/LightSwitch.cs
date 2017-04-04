using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour, LightActivatedObject {

	[SerializeField]
	protected List<GameObject> connectedObjects;
	protected List<ActivateableObject> activators;
    GameObject light;

	bool isActive;

	void Start() {
		activators = new List<ActivateableObject> ();
		foreach (GameObject go in connectedObjects) {
			ActivateableObject ao = go.GetComponent<ActivateableObject> ();
			if (ao != null) {
				activators.Add (ao);
			}
		}
	}
	public void Activate()
	{
		isActive = true;
		foreach (ActivateableObject a in activators)
		{
			a.Activate();
		}
	}

    private void Update()
    {
        if (!light)
        {
            Deactivate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.OnLayer("Light"))
        {
            light = collision.gameObject;
            Activate();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.OnLayer("Light"))
        {
            light = null;
            Deactivate();
        }
    }

    public void Deactivate()
	{
		isActive = false;
		foreach (ActivateableObject a in activators)
		{
			a.Deactivate();
		}
	}

	public bool IsActive {
		get {
			return isActive;
		}
	}
}
