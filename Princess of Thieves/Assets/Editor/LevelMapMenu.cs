using UnityEngine;
using System.Collections;
using UnityEditor;

public class LevelMapMenu {

	[MenuItem("GameObject/LevelMap")]
	public static void CreateTileMap()
	{
		GameObject go = new GameObject("levelmap");
		go.AddComponent<LevelMap>();
	}
}
