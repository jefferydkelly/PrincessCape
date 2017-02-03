using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrip : MonoBehaviour {

    [SerializeField]
    GameObject waterDroplet;

	[SerializeField]
	AudioClip dripSound;

	Timer dripTimer;

	[SerializeField]
	float dripTime = 3.0f;
	// Use this for initialization
	void Start () {
		dripTimer = new Timer(() =>
			{
				Drip();
			}, dripTime, true);
	
        //InvokeRepeating("Drip", 1f, timeToDrop);
	}

	void OnBecameVisible() {
		dripTimer.Reset ();
		dripTimer.Start ();
	}

    void OnBecameInvisible()
    {
		dripTimer.Stop ();
    }
    void Drip()
    {
		GameObject temp = Instantiate (waterDroplet, transform.position, transform.rotation);
		AudioManager.Instance.PlaySound (dripSound);
    }

	void OnDestroy() {
		if (TimerManager.Instance) {
			dripTimer.Stop ();
		}
	}
}
