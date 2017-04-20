using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hogi : MonoBehaviour, LightActivatedObject {
    bool isActive = false;
    float rotated = 0;
    public bool IsActive {
        get
        {
            return isActive;
        }
    }

    public void Activate()
    {
        if (!isActive)
        {
            StartCoroutine(TipOver());
        }
    }

    IEnumerator TipOver()
    {
        while (rotated < 90)
        {
            float dt = Time.deltaTime;
            rotated += dt * 3f;
            transform.Rotate(Vector3.forward, dt * 3f);
            yield return null;
        }
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Cracked"))
        {
            Destroy(go);
            yield return new WaitForSeconds(0.5f);
        }
        yield return null;
    }
    public void Deactivate()
    {
        
    }

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("LightBeam"))
        {
            if (!isActive)
            {
                Activate();
            }
        }
    }
}
