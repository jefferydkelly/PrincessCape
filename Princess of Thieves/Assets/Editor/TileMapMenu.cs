using UnityEngine;
using System.Collections;
using UnityEditor;
public class TileMapMenu {

	[MenuItem("GameObject/TileMap")]
	public static void CreateTileMap()
	{
		GameObject go = new GameObject("tilemap");
		go.AddComponent<TileMap>();
	}
}
