using UnityEngine;
using System.Collections;

public class JDMappableObject : MonoBehaviour {

	public MapLayer mapLayer;
}

public enum MapLayer
{
	Foreground,
	Background,
	Field,
	Wall
}
