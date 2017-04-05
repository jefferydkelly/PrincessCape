using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour, LightActivatedObject {

	[SerializeField]
	protected List<ActivatorConnection> connectedObjects;
	//protected List<ActivateableObject> activators;
    GameObject light;
    Animator myAnimator;

	bool isActive;

	void Start() {
        myAnimator = GetComponent<Animator>();
            /*
            activators = new List<ActivateableObject> ();
            foreach (GameObject go in connectedObjects) {
                ActivateableObject ao = go.GetComponent<ActivateableObject> ();
                if (ao != null) {
                    activators.Add (ao);
                }
            }*/
        }
	public void Activate()
	{
		isActive = true;
        myAnimator.SetTrigger("Activated");
        foreach(ActivatorConnection ac in connectedObjects)
        {
            ac.Activate();
        }
        /*
		foreach (ActivateableObject a in activators)
		{
			a.Activate();
		}*/
	}

    private void Update()
    {
        if (!light && isActive)
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
        myAnimator.SetTrigger("Deactivated");
        isActive = false;

        foreach (ActivatorConnection ac in connectedObjects)
        {
            ac.Dectivate();
        }
        /*
        foreach (ActivateableObject a in activators)
		{
			a.Deactivate();
		}*/
	}

	public bool IsActive {
		get {
			return isActive;
		}
	}
}
