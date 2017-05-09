using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ReflectiveObject{
    Vector2 GetSurfaceForward(Vector2 fwd);
	GameObject GameObject{get;}
	bool IsReflecting{get;}
}
