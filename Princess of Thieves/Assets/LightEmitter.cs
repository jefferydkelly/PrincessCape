using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEmitter : MonoBehaviour {

	[SerializeField]
	float range;
	[SerializeField]
	Vector2 fwd;

	LineRenderer myLineRenderer;
	// Use this for initialization
	void Start () {
		myLineRenderer = GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		foreach (RaycastHit2D hit in Physics2D.BoxCastAll(transform.position, new Vector2(1, 1), fwd.GetAngle(), fwd, range)) {
			if (hit.collider.CompareTag ("Player")) {
				if (GameManager.Instance.Player.IsUsingReflectCape) {
					//Reflect - Add a new set of point that reflect according to the player's forward
				}
				//Otherwise, if there's another reflective object, reflect it as well.
				//If there's a light activated object, activate it.
			}
		}
	}
}
