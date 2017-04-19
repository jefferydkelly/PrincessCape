using UnityEngine;
using System.Collections;

public interface ActivateableObject
{
	void Activate();
	void Deactivate();

	bool IsActive { get; }
	float ActivationTime{get;}
}

[System.Serializable]
public struct ActivatorConnection
{
    public GameObject activatedObject;
    ActivateableObject activated;
    public bool inverted;

    public void Activate()
    {
        if (activatedObject)
        {
            ActivateableObject activated = activatedObject.GetComponent<ActivateableObject>();
            if (activated != null)
            {

                if (inverted)
                {
                    activated.Deactivate();
                }
                else
                {
                    activated.Activate();
                }
            }
        }
    }

    public void Dectivate()
    {
        if (activatedObject)
        {
            ActivateableObject activated = activatedObject.GetComponent<ActivateableObject>();
            if (activated != null)
            {
                if (inverted)
                {
                    activated.Activate();
                }
                else
                {
                    activated.Deactivate();
                }
            }
        }
    }
}