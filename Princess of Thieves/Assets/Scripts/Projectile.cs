using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public float lifeTime = 5.0f;
	Timer lifeTimer;
	// Use this for initialization
	void Start () {
		lifeTimer = new Timer (() => {
			Destroy (gameObject);
		}, lifeTime);
		TimerManager.Instance.AddTimer (lifeTimer);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col) {
		if ((col.CompareTag ("Player") && !GameManager.Instance.Player.IsUsingReflectCape) || (!col.CompareTag("Player") && !(col.OnLayer ("Background") || col.OnLayer ("Interactive")))) {
			Destroy (gameObject);
		}
	}

	void OnDestroy() {
		if (TimerManager.Instance) {
			TimerManager.Instance.RemoveTimer (lifeTimer);
		}
	}

}
