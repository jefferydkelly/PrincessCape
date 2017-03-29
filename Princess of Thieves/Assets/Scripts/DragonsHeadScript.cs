using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonsHeadScript : MonoBehaviour {
    public TextAsset cutscene;
    public float timeTilActivate = 3f;
    bool timed = false;
    public Timer t;
	// Use this for initialization
	void Awake () {
        t = new Timer(() => {
            timed = true;
        }, timeTilActivate);
        t.Start();
    }
	
	// Update is called once per frame
	void Update () {

        if (timed)
        {
            //ActivateCutscene();
            timed = false;
        }
	}

    void ActivateCutscene()
    {
        //incoming bad code lmao
        GameObject temp = Instantiate(gameObject, transform.position, transform.rotation);
        temp.AddComponent<CutsceneTrigger>();
        temp.GetComponent<CutsceneTrigger>().cutscene = cutscene;
        temp.GetComponent<SpriteRenderer>().enabled = false;
        temp.GetComponent<DragonsHeadScript>().t.Stop();

        //wait some time, then load an ending scene.
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "TopCeiling") //jank, but works
        {
            t.Stop();
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
