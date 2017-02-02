﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour {
	static TimerManager instance;
	List<Timer> timers;
	List<Timer> toAdd;
	List<Timer> toRemove;
	bool quitting = false;
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
		if (!GameManager.Instance.IsPaused) {
			foreach (Timer t in toAdd) {
				timers.Add (t);
			}

			toAdd.Clear ();
			float dt = Time.deltaTime;
			foreach (Timer t in timers) {
				if (t.Update (dt)) {
                
					toRemove.Add (t);
				}
			}

			foreach (Timer t in toRemove) {
				timers.Remove (t);
			}

			toRemove.Clear ();
		}
	}

	public static TimerManager Instance {
		get {
			
			if (instance == null) {
				GameObject gameObj = new GameObject ("Timer Manager");
				DontDestroyOnLoad (gameObj);
				instance = gameObj.AddComponent<TimerManager> ();
			} else if (instance.quitting) {
				return null;
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

	void OnApplicationQuit() {
		quitting = true;
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
		runTime = time;
		curTime = time;
		funcToRun = wd;
		repeating = rep;
	}

	public bool Update(float dt) {
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
