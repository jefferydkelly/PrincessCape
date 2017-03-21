﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public float lifeTime = 5.0f;
	bool reflected = false;
	Timer lifeTimer;
    Rigidbody2D myRigidbody;
	// Use this for initialization
	void Start () {
		lifeTimer = new Timer (() => {
			Destroy (gameObject);
		}, lifeTime);

		lifeTimer.Start ();
        myRigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col) {
		if ((col.CompareTag ("Player") && !GameManager.Instance.Player.IsUsingReflectCape) || (!col.CompareTag("Player") && !(col.OnLayer ("Background") || col.OnLayer ("Interactive") || col.CompareTag("Reflective")))) {
			Destroy (gameObject);
		}
	}

	void OnDestroy() {
		if (TimerManager.Instance) {
			lifeTimer.Stop ();
		}
	}

    public bool Reflect(Vector2 fwd)
    {
        if (myRigidbody)
        {
            Vector2 vel = myRigidbody.velocity;
            float dot = vel.normalized.Dot(fwd.normalized);

            if (dot == -1)
            {
                myRigidbody.velocity = vel.Rotated(Mathf.PI);
                transform.Rotate(Vector3.forward, 180);
                return true;
            } else if (vel.Dot(fwd) <= 0)
            {
                float rot = (vel.GetAngle() - fwd.GetAngle()) * 2;
                
                myRigidbody.velocity = vel.Rotated(rot);
                transform.Rotate(Vector3.forward, rot.ToDegrees());
                return true;
            }
        }
        return false;
    }

	public bool Reflected {
		get {
			return reflected;
		}

		set {
			reflected = value;
		}
	}

}
