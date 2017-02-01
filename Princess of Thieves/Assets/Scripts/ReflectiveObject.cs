using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectiveObject : MonoBehaviour {

	[SerializeField]
	Vector2 fwd;
	int reflectingLayers;

	public void Start() {
		int reflectiveLayer = 1 << LayerMask.NameToLayer ("Reflective");
		int playerLayer = 1 << LayerMask.NameToLayer ("Player");
		int platformLayer = 1 << LayerMask.NameToLayer ("Platforms");
		reflectingLayers = reflectiveLayer | platformLayer | playerLayer;
	}
	public Vector2 FWD {
		get {
			return fwd;
		}
	}

	public List<Vector3> Reflect(LightEmitter le, float range) {
		List<Vector3> positions = new List<Vector3> ();
		Vector2 stopPos = transform.position + (Vector3)FWD * range;
		float closeDist = range;
		foreach (RaycastHit2D hit in Physics2D.BoxCastAll(transform.position, new Vector2(0.9f, 0.9f), fwd.GetAngle(), fwd, range, reflectingLayers)) {
			if (hit.distance < closeDist) {
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
						//CheckForReflectiveObjects (pVec, player.TrueAim.normalized, range - hit.distance, noPlayerMask);
						return positions;
					}
				} else if (hit.collider.OnLayer ("Reflective")) {
					if (hit.collider.gameObject != gameObject) {
						ReflectiveObject ro = hit.collider.GetComponent<ReflectiveObject> ();

						if (ro) {
							positions.Add (ro.transform.position);
							foreach (Vector3 v in ro.Reflect(le, range - hit.distance)) {
								positions.Add (v);
							}
							//CheckForReflectiveObjects(ro.transform.position, ro.FWD, range - hit.distance, allLayerMask);
							return positions;
						} else {
							
						}
					}
				} else {
					closeDist = hit.distance;
					stopPos = hit.point;
				}
			}
		}

		positions.Add (stopPos);
		return positions;
	}
}
