using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour {
	static TimerManager instance;
	List<Timer> timers;
	List<Timer> toAdd;
	List<Timer> toRemove;
	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
			timers = new List<Timer> ();
			toAdd = new List<Timer> ();
			toRemove = new List<Timer> ();
		} else {
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("TImer is updating");
        foreach (Timer t in toAdd) {
			timers.Add (t);
            Debug.Log(timers);
		}

		toAdd = new List<Timer> ();
		float dt = Time.time;
		foreach (Timer t in timers) {
            Debug.Log("Updating T");
            if (t.Update2 (dt)) {
                
                toRemove.Add (t);
			}
		}

		foreach (Timer t in toRemove) {
			timers.Remove (t);
		}

		toRemove = new List<Timer> ();
	}

	public static TimerManager Instance {
		get {
			if (instance == null) {
				if (Camera.main.gameObject) {
					Camera.main.gameObject.AddComponent<TimerManager> ();
				}
			}
			return instance;
		}
	}

	public void AddTimer(Timer t) {
        //never gets called?
		toAdd.Add (t);
        //toAdd.Remove(t);
	}

	public void RemoveTimer(Timer t) {
		t.Paused = true;
		toRemove.Add (t);
	}
}

public class Timer {
	bool paused = false;
	bool repeating = false;
	float runTime;
	float curTime;
	WaitDelegate funcToRun;
    //Creation
	public Timer(WaitDelegate wd, float time, bool rep = false) {
        Debug.Log("Timer Created");
		runTime = time;
		curTime = time;
		funcToRun = wd;
		repeating = rep;
	}

	public bool Update2(float dt) {
        Debug.Log("Here");
        if (!paused) {
           
            curTime -= dt;

			if (curTime <= 0) {
              
				funcToRun ();
				if (repeating) {
					curTime += runTime;
				}
				return !repeating;
			}
		}

		return false;
	}

	public bool Paused {
		get {
			return paused;
		}

		set {
			paused = value;
		}
	}

	public void Reset() {
		curTime = runTime;
	}
}
