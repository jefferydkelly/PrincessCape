using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetractableSpikes : MonoBehaviour, ActivateableObject {
    [SerializeField]
    bool startActive = true;
    bool isActive = false;
    float retractTime = 0.25f;
    int moveDirection = -1;

    Vector3 extendedPosition;
    Vector3 retractedPosition;
    public bool IsActive {
        get
        {
            return isActive;
        }
    }

    public float ActivationTime
    {
        get
        {
            return retractTime;
        }
    }

    public void Activate()
    {
        moveDirection = 1;
        StartCoroutine(Popup());
    }

    IEnumerator Popup ()
    {
        while ((moveDirection == 1 && transform.position.y < extendedPosition.y) || (moveDirection == -1 && transform.position.y > retractedPosition.y))
        {
            transform.position += new Vector3(0, gameObject.HalfHeight() * 2) * moveDirection * Time.deltaTime / retractTime;
            yield return null;
        }
        yield return null;
    }

    public void Deactivate()
    {
        moveDirection = -1;
        StartCoroutine(Popup());
    }

    // Use this for initialization
    void Start () {
		if (!startActive)
        {
            retractedPosition = transform.position;
            extendedPosition = retractedPosition + new Vector3(0, gameObject.HalfHeight() * 2);
        } else
        {
            extendedPosition = transform.position;
            retractedPosition = extendedPosition - new Vector3(0, gameObject.HalfHeight() * 2);
        }
	}
}
