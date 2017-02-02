using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBallFire : MonoBehaviour {

    [SerializeField]
    GameObject projectile;
	bool isActive = false;
  
    public float timeToFire = 3f;
	[SerializeField]
	Vector3 fwd = new Vector3(1,0);
	Timer fireTimer;
	// Use this for initialization
	void Start () {
		WaitDelegate wd;
		wd = () => {
			Fire();
		};

		fireTimer = new Timer (wd, timeToFire, true);
		fireTimer.Start ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void Fire()
    {
        GameObject temp = Instantiate(projectile);
		temp.transform.position = transform.position + fwd * (gameObject.HalfWidth () + temp.HalfWidth () + 0.25f);
		temp.GetComponent<Rigidbody2D>().AddForce(fwd*5,ForceMode2D.Impulse);
    }
}
