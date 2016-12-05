using UnityEngine;
using System.Collections;
using UnityEditor;

public class LevelMapMenu {

	[MenuItem("GameObject/LevelMap")]
	public static void CreateTileMap()
	{
        GameObject canvas = GameObject.Find("Canvas");

        if (canvas == null)
        {
            string path = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("Canvas")[0]);
            canvas = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath(path, typeof(GameObject))) as GameObject;
        }

        if (Camera.main.GetComponent<CameraManager>() == null)
        {
            CameraManager camMan = Camera.main.gameObject.AddComponent<CameraManager>();
            camMan.canvas = canvas.GetComponent<Canvas>();
        }
        canvas.SetActive(false);
		GameObject go = new GameObject("levelmap");
		go.AddComponent<LevelMap>();
	}
}
