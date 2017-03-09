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

	[SerializeField]
	AudioClip dropSound;

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

    void SpawnSegment()
    {
        if (transform.childCount < numSegments)
        {
            GameObject segment = Instantiate(prefab);
            segment.transform.parent = transform;
            segment.transform.localPosition = -new Vector3(0, prefab.HalfHeight() * 2 * (transform.childCount - 0.5f), -1);



            segment.name = "Segment " + transform.childCount;
            AudioManager.Instance.PlaySound(dropSound);
            if (transform.childCount >= numSegments)
            {
                foreach (LadderController lc in GetComponentsInChildren<LadderController>())
                {
                    lc.CheckForConnections();
                }
                spawnTimer.Stop();
                isActive = true;
            }
        }
    }

	void DestroySegment() {
		if (transform.childCount > 0) {
			Destroy (transform.GetChild(transform.childCount - 1).gameObject);
		} else {
			destroyTimer.Stop ();
		}
	}

	public float ActivationTime {
		get {
			return spawnTime * numSegments;
		}
	}
				
}
