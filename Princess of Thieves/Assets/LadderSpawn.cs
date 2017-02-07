using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderSpawn : MonoBehaviour, ActivateableObject {

	[SerializeField]
	GameObject prefab;
	GameObject[] segments;
	[SerializeField]
	int numSegments = 2;
	int segmentsSpawned = 0;
	[SerializeField]
	float spawnTime = 1.0f;

	Timer spawnTimer;
	Timer destroyTimer;

	bool isActive = false;
	// Use this for initialization
	void Start () {
		spawnTimer = new Timer (() => {
			SpawnSegment();
		}, spawnTime, numSegments);

		destroyTimer = new Timer (() => {
			DestroySegment();
		}, spawnTime, numSegments);

		segments = new GameObject[numSegments];
	}
	
	public void Activate() {
		destroyTimer.Stop ();
		spawnTimer.Restart ();
	}

	public void Deactivate() {
		isActive = false;
		spawnTimer.Stop ();
		destroyTimer.Restart ();
	}

	public bool IsActive {
		get {
			return isActive;
		}
	}

	void SpawnSegment() {
		GameObject segment = Instantiate (prefab);
		segment.transform.position = transform.position - new Vector3 (0, prefab.HalfHeight() * 2 * (segmentsSpawned + 0.5f));
		segments [segmentsSpawned] = segment;
		segmentsSpawned++;

		if (segmentsSpawned == numSegments) {
			foreach (GameObject go in segments) {
				LadderController lc = go.GetComponent<LadderController> ();
				lc.CheckForConnections ();
			}
			spawnTimer.Stop ();
			isActive = true;
		}
	}

	void DestroySegment() {
		segmentsSpawned--;
		Destroy (segments[segmentsSpawned]);

		if (segmentsSpawned == 0) {
			destroyTimer.Stop ();
		}
	}
				
}
