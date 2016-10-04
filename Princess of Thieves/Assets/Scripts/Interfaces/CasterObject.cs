using UnityEngine;
using System.Collections;

public interface CasterObject {
	Vector3 Forward { get; }
	Rigidbody2D RigidBody { get;}
}
