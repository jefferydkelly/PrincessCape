using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBallFire : MonoBehaviour {

    [SerializeField]
    GameObject projectile;

    float lastTimeFire = 0f;
    public float timeToFire = 3f;
	Timer fireTimer;
	// Use this for initialization
	void Start () {
		WaitDelegate wd;
		wd = () => {
			Fire();
		};

		fireTimer = new Timer (wd, timeToFire, true);
		TimerManager.Instance.AddTimer (fireTimer);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void Fire()
    {
        GameObject temp = Instantiate(projectile, transform.position,transform.rotation);
        temp.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.5f, 0.5f)*150,ForceMode2D.Force);
    }

}
