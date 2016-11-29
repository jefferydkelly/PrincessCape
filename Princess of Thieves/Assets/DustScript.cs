using UnityEngine;
using System.Collections;

public class DustScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Destroy(gameObject, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {

        transform.Rotate(0, 0, 50 * Time.deltaTime); //rotates 50 degrees per second around z axis
        float targetScale = 2f;
        float shrinkSpeed = 0.1f;
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(targetScale, targetScale, targetScale), Time.deltaTime* shrinkSpeed);
    }
}
