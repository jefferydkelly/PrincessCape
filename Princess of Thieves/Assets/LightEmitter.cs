﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEmitter : MonoBehaviour {

	[SerializeField]
	float range;
	[SerializeField]
	Vector2 fwd;
	List<Vector3> positions = new List<Vector3>();

	LineRenderer myLineRenderer;
	int allLayerMask;
	int noPlayerMask;
	LightActivatedObject lao = null;
	// Use this for initialization
	void Start () {
		myLineRenderer = GetComponent<LineRenderer> ();
		myLineRenderer.startColor = Color.yellow;
		myLineRenderer.endColor = Color.yellow;
		int playerLayer = 1 << LayerMask.NameToLayer ("Player");
		int platformLayer = 1 << LayerMask.NameToLayer ("Platforms");
		int reflectLayer = 1 << LayerMask.NameToLayer ("Reflective");
		allLayerMask =   playerLayer | platformLayer | reflectLayer;
		noPlayerMask = platformLayer | reflectLayer;
	}
	
	// Update is called once per frame
	void Update () {
		positions.Clear ();
		positions.Add (transform.position);
		CheckForReflectiveObjects (transform.position, fwd, range, allLayerMask);
		/*
		bool hitPlayer = false;
		Vector3 stopPos = transform.position + (Vector3)(fwd * range);
		int layerMask = 1 << LayerMask.NameToLayer ("Player") | LayerMask.NameToLayer ("Platforms") | LayerMask.NameToLayer ("Reflective");
		foreach(RaycastHit2D hit in Physics2D.BoxCastAll(transform.position, new Vector2(0.9f, 0.9f), 0, fwd, range, layerMask)) {
			if (hit.collider.CompareTag ("Player")) {
				Player player = GameManager.Instance.Player;
				if (player.IsUsingReflectCape) {

					//Reflect - Add a new set of point that reflect according to the player's forward
					Vector3 pVec = player.transform.position;
					if (fwd.x == 0) {
						pVec.x = transform.position.x;
					} else if (fwd.y == 0) {
						pVec.y = transform.position.y;
					}
					positions.Add (pVec);
					positions.Add (pVec + (Vector3)(player.TrueAim * range));
					hitPlayer = true;
					break;
				} 
				//Otherwise, if there's another reflective object, reflect it as well.
				//If there's a light activated object, activate it.
			} else {
				if (hit.collider.transform.position.y >= stopPos.y) {
					stopPos.y = hit.collider.transform.position.y + hit.collider.gameObject.HalfHeight();
				}
			}
		}
		if (!hitPlayer) {
			positions.Add (stopPos);
		}*/

		for (int i = 0; i < positions.Count; i++) {
			Vector3 pos = positions [i];
			pos.z = 1.0f;
			positions [i] = pos;
		}
		myLineRenderer.numPositions = positions.Count;
		myLineRenderer.SetPositions (positions.ToArray());
		
	}

	void CheckForReflectiveObjects(Vector2 startPos, Vector2 forward, float range, int layerMask) {
		Vector2 stopPos = startPos + forward * range;
		float closeDist = range;
		LightActivatedObject laObj = null;
		foreach (RaycastHit2D hit in Physics2D.BoxCastAll(startPos, new Vector2(0.9f, 0.9f), forward.GetAngle(), forward, range, layerMask)) {
			if (hit.distance < closeDist) {
				

				if (hit.collider.CompareTag ("Player")) {
					Player player = GameManager.Instance.Player;
					if (player.IsUsingReflectCape) {

						//Reflect - Add a new set of point that reflect according to the player's forward
						Vector3 pVec = player.transform.position;
						if (forward.x == 0) {
							pVec.x = transform.position.x;
						} else if (forward.y == 0) {
							pVec.y = transform.position.y;
						}

						positions.Add (pVec);
						CheckForReflectiveObjects (pVec, player.TrueAim.normalized, range - hit.distance, noPlayerMask);
						return;
					}
				} else if (hit.collider.OnLayer ("Reflective")) {
						
					ReflectiveObject ro = hit.collider.GetComponent<ReflectiveObject> ();

					if (ro) {
						positions.Add (ro.transform.position);
						foreach (Vector3 v in ro.Reflect(this, range - hit.distance)) {
							positions.Add (v);
						}
						//CheckForReflectiveObjects(ro.transform.position, ro.FWD, range - hit.distance, allLayerMask);
						return;
					} else {
						laObj = hit.collider.GetComponent<LightActivatedObject> ();

						if (laObj != lao) {
							if (lao != null) {
								lao.Deactivate ();
							}
							lao = laObj;

							if (lao != null) {
								lao.Activate ();

							}

						}

						positions.Add (hit.point);
						return;
					}
				} else {
					closeDist = hit.distance;
					stopPos = hit.point;
				}
			}
		}

		if (lao != null) {
			lao.Deactivate ();
			lao = null;
		}

		positions.Add (stopPos);
	}
}
