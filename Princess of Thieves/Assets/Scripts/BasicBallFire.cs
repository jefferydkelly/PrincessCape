using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBallFire : MonoBehaviour {

    [SerializeField]
    GameObject projectile;

    float lastTimeFire = 0f;
    public float timeToFire = 3f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time - lastTimeFire >= timeToFire)
        {
            Fire();
            lastTimeFire = Time.time;
        }
	}
    void Fire()
    {
        GameObject temp = Instantiate(projectile, transform.position,transform.rotation);
        temp.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 0)*150,ForceMode2D.Force);
    }

}
