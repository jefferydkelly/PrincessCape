using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrip : MonoBehaviour {

    [SerializeField]
    GameObject waterDroplet;

	[SerializeField]
	AudioClip dripSound;
    
	Timer dripTimer;
	// Use this for initialization
	void Start () {
		
        //InvokeRepeating("Drip", 1f, timeToDrop);
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Contains("Player"))
        {
            float timeToDrop = Random.Range(1, 5);
            dripTimer = new Timer(() =>
            {
                Drip();
            }, timeToDrop, true);
            TimerManager.Instance.AddTimer(dripTimer);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.name.Contains("Player"))
        {
            TimerManager.Instance.RemoveTimer(dripTimer);
        }
    }
    void Drip()
    {
		GameObject temp = Instantiate (waterDroplet, transform.position, transform.rotation);
		AudioManager.Instance.PlaySound (dripSound);
    }

	void OnDestroy() {
		TimerManager.Instance.RemoveTimer (dripTimer);
	}
}
