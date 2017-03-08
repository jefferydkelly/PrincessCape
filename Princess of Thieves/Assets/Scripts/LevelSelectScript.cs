using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelSelectScript : MonoBehaviour {

    GameObject player;
    // Use this for initialization
	void Start () {
	    	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Level1Load()
    {
        Debug.Log("Hello");
        SceneManager.LoadScene("JDCapeTestScene");
    }
}
