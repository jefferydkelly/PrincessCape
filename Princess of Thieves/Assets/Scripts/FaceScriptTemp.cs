using UnityEngine.UI;
using System.Collections;
using UnityEngine;

public class FaceScriptTemp : MonoBehaviour {

    public Sprite face1, face2;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Player")
            GetComponent<SpriteRenderer>().sprite = face2;
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.name == "Player")
            GetComponent<SpriteRenderer>().sprite = face1;
    }
}
