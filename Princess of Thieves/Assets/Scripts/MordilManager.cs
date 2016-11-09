using UnityEngine;
using System.Collections.Generic;

public class MordilManager : MonoBehaviour {
    static MordilManager instance = null;
    List<BossEyeScript> mordEyes;
    List<HandOfMordil> mordHands;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public static MordilManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MordilManager();
            }

            return instance;
        }
    }
}
