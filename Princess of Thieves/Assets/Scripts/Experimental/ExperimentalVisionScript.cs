using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentalVisionScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 8f);
        foreach(Collider2D col in cols)
        {
            if (col.GetComponent<SpriteRenderer>())
            {
                Color colColor = col.GetComponent<SpriteRenderer>().color;
                colColor.a = 0.5f;
                col.GetComponent<SpriteRenderer>().color = colColor;
            }
            else
            {
                //col.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                Color colColor = col.GetComponent<MeshRenderer>().material.color;
                colColor.a = 0.5f;
                col.GetComponent<MeshRenderer>().material.color = colColor;
            }
        }
        cols = Physics2D.OverlapCircleAll(transform.position, 5f);
        foreach (Collider2D col in cols)
        {
            if (col.GetComponent<SpriteRenderer>())
            {
                Color colColor = col.GetComponent<SpriteRenderer>().color;
                colColor.a = 0.7f;
                col.GetComponent<SpriteRenderer>().color = colColor;
            }
            else
            {
                //col.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                Color colColor = col.GetComponent<MeshRenderer>().material.color;
                colColor.a = 0.7f;
                col.GetComponent<MeshRenderer>().material.color = colColor;
            }
        }
        cols = Physics2D.OverlapCircleAll(transform.position, 3f);
        foreach (Collider2D col in cols)
        {
            if (col.GetComponent<SpriteRenderer>())
            {
                Color colColor = col.GetComponent<SpriteRenderer>().color;
                colColor.a = 1f;
                col.GetComponent<SpriteRenderer>().color = colColor;
            }
            else
            {
                //col.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                Color colColor = col.GetComponent<MeshRenderer>().material.color;
                colColor.a = 1f;
                col.GetComponent<MeshRenderer>().material.color = colColor;
            }
        }
    }
}
