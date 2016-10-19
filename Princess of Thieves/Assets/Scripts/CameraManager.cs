using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public GameObject target;
    private Camera cam;
    // Use this for initialization
    void Start () {
        target = GameObject.FindGameObjectWithTag("Player");
        Vector3 camPos = target.transform.position;
        camPos.z = -10;
        transform.position = camPos;
        cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        if (target)
        {
            Vector3 point = cam.WorldToViewportPoint(target.transform.position);
            Vector3 delta = target.transform.position - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
    }
}
