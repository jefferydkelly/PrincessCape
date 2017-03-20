using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour {
	static TimerManager instance;
	List<Timer> timers;
	List<Timer> toAdd;
	List<Timer> toRemove;
	static bool quitting = false;
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
		if (!quitting && !GameManager.Instance.IsPaused) {
			if (!toAdd.IsEmpty ()) {
				foreach (Timer t in toAdd) {
					timers.AddExclusive (t);
				}

				toAdd.Clear ();
			}
			float dt = Time.deltaTime;
			foreach (Timer t in timers) {
				if (t.Update (dt)) {
                
					toRemove.Add (t);
				}
			}

			if (!toRemove.IsEmpty ()) {
				foreach (Timer t in toRemove) {
					timers.Remove (t);
				}

				toRemove.Clear ();
			}
		}
	}

	public static TimerManager Instance {
		get {
			if (quitting) {
				return null;
			}
			if (instance == null) {
				GameObject gameObj = new GameObject ("Timer Manager");
				DontDestroyOnLoad (gameObj);
				instance = gameObj.AddComponent<TimerManager> ();
			}
			return instance;
		}
	}

	public void Clear() {
		timers.Clear ();
	}

	public void AddTimer(Timer t) {
		toAdd.Add (t);
	}

	public void RemoveTimer(Timer t) {
		t.Paused = true;
		toRemove.Add (t);
	}

	void OnApplicationQuit() {
		quitting = true;
	}

    private void OnDestroy()
    {
        instance = null;
    }
}

public class Timer {
	bool paused = false;
	bool repeating = false;
	float runTime;
	float curTime;
	WaitDelegate funcToRun;
	int repeatTimes = 0;
	int repsRun = 0;
	TimerState state = TimerState.Unactivated;
    
	/// <summary>
	/// Initializes a new instance of the <see cref="Timer"/> class.
	/// </summary>
	/// <param name="wd">The function to be run when the timer is complete.</param>
	/// <param name="time">The amount of time until the timer is completed.</param>
	/// <param name="rep">Whether or not the timer will repeat indefinitely.</param>
	public Timer(WaitDelegate wd, float time, bool rep = false) {
		runTime = time;
		curTime = time;
		funcToRun = wd;
		repeating = rep;
		repeatTimes = 0;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Timer"/> class.
	/// </summary>
	/// <param name="wd">The function to be run when the timer is complete.</param>
	/// <param name="time">The time until one rep of the timer is complete.</param>
	/// <param name="numReps">The number of times the timer will run.</param>
	public Timer(WaitDelegate wd, float time, int numReps) {
		funcToRun = wd;
		runTime = time;
		curTime = time;
		repeatTimes = numReps;
	}

	public bool Update(float dt) {
        if (!paused) {
           
            curTime -= dt;
			if (curTime <= 0) {
              
				funcToRun ();
				repsRun++;
				if (repeating || repsRun < repeatTimes) {
					curTime += runTime;
				} 
				return !(repeating || repsRun < repeatTimes);
			}
		}

		return false;
	}

	public void Start() {
		
		if (state == TimerState.Unactivated && TimerManager.Instance) {
			state = TimerState.Running;
			TimerManager.Instance.AddTimer (this);
		}
	}

	public void Stop() {
		state &= ~TimerState.Running;
		state |= TimerState.Stopped;
		if (TimerManager.Instance) {
			
			TimerManager.Instance.RemoveTimer (this);
		}
	}

	public float RunPercent {
		get {
			return curTime / runTime;
		}
	}

	public float RunPercentWithRepeats {
		get {
			if (repeatTimes == 0 || repeating) {
				return RunPercent;
			} else {
				return ((repsRun * runTime) + runTime) / ((repeatTimes + 1) * curTime);
			}
		}
	}

	public void Restart() {
		Reset ();

		Start ();
	}

	public bool Paused {
		get {
			return (state & (TimerState.Paused | TimerState.Unactivated)) > 0;
		}

		set {
			if (value) {
				state |= TimerState.Paused;
			} else {
				state &= ~TimerState.Paused;
			}
		}
	}

	public bool Activated {
		get {
			return (state & TimerState.Unactivated) == 0;
		}
	}

	public void Reset() {
		curTime = runTime;
		repsRun = 0;
		state = TimerState.Unactivated;
	}
}

[System.Flags]
public enum TimerState {
	Default = 0,
	Unactivated = 1,
	Running = 2,
	Paused = 4,
	Stopped = 8
}
