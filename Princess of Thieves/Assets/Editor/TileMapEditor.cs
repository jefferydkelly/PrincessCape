using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TileMap))]
public class TileMapEditor : Editor
{
	public TileMap map;
	TileBrush brush;
	Vector3 mouseHitPos;

	bool MouseOnMap
	{
		get
		{
			return mouseHitPos.x.BetweenEx(0, map.gridSize.x) && mouseHitPos.y.BetweenEx(-map.gridSize.y, 0);
		}
	}
	public override void OnInspectorGUI()
	{
		EditorGUILayout.BeginVertical();
		Vector2 oldSize = map.mapSize;
		map.mapSize = EditorGUILayout.Vector2Field("Map Size", map.mapSize);

		if (map.mapSize != oldSize)
		{
			UpdateCalculations();
		}
		UpdateBrush(map.CurrentTileBrush);
		Texture2D oldTexture = map.texture2d;
		map.texture2d = EditorGUILayout.ObjectField("Texture2D", map.texture2d, typeof(Texture2D), false) as Texture2D;
	
		if (oldTexture != map.texture2d)
		{
			UpdateCalculations();
			map.tileID = 1;
			CreateBrush();
		}
		if (map.texture2d == null)
		{
			EditorGUILayout.HelpBox("You have not select a texture 2d yet.", MessageType.Warning);
		}
		else {
			EditorGUILayout.LabelField("Tile Size: ", map.tileSize.x + "x" + map.tileSize.y);
			EditorGUILayout.LabelField("Grid Size In Units", map.gridSize.x + "x" + map.gridSize.y);
			EditorGUILayout.LabelField("Pixels to Units", map.pixelsToUnits.ToString());
			UpdateBrush(map.CurrentTileBrush);

			if (GUILayout.Button("Clear Tiles"))
			{
				if (EditorUtility.DisplayDialog("Clear Map's Tile", "Are you sure?", "Clear", "Do Not Clear"))
				{
					ClearMap();
				}
			}
		}
		EditorGUILayout.EndVertical();
	}

	void OnEnable()
	{
		map = target as TileMap;
		Tools.current = Tool.View;
		if (map.tiles == null)
		{
			map.tiles = new GameObject("Tiles");
			map.tiles.transform.SetParent(map.transform);
			map.tiles.transform.position = Vector3.zero;

		}
		if (map.texture2d != null)
		{
			UpdateCalculations();
			NewBrush();
		}
	}

	void OnDisable()
	{
		DestroyBrush();
	}

	void OnSceneGUI()
	{
		if (brush != null)
		{
			UpdateHitPosition();
			MoveBrush();

			if (map.texture2d != null && MouseOnMap)
			{
				Event cEvent = Event.current;

				if (cEvent.shift)
				{
					Draw();
				}
				else if (cEvent.alt)
				{
					RemoveTile();
				}
			}
		}
	}
	void UpdateCalculations()
	{
		string path = AssetDatabase.GetAssetPath(map.texture2d);
		map.spriteReferences = AssetDatabase.LoadAllAssetsAtPath(path);
		Sprite sp = map.spriteReferences[1] as Sprite;
		float width = sp.textureRect.width;
		float height = sp.textureRect.height;
		map.tileSize = new Vector2(width, height);
		map.pixelsToUnits = (int)(sp.rect.width / sp.bounds.size.x);
		map.gridSize = new Vector2(map.tileSize.x * map.mapSize.x, map.tileSize.y * map.mapSize.y) / map.pixelsToUnits;
	}

	void CreateBrush()
	{
		Sprite sprite = map.CurrentTileBrush;
		if (sprite != null)
		{
			GameObject go = new GameObject("Brush");
			go.transform.SetParent(map.transform);

			brush = go.AddComponent<TileBrush>();
			brush.renderer2D = go.AddComponent<SpriteRenderer>();
			brush.renderer2D.sortingOrder = 1000;
			brush.size = sprite.textureRect.size / map.pixelsToUnits;
			brush.UpdateBrush(sprite);
		}
	}

	void NewBrush()
	{
		if (brush == null)
		{
			CreateBrush();
		}
	}

	void DestroyBrush()
	{
		if (brush != null)
		{
			DestroyImmediate(brush.gameObject);
		}
	}

	public void UpdateBrush(Sprite sprite)
	{
		if (brush != null)
		{
			brush.UpdateBrush(sprite);
		}
	}

	void UpdateHitPosition()
	{
		Plane p = new Plane(map.transform.TransformDirection(Vector3.forward), Vector3.zero);
		Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
		Vector3 hit = Vector3.zero;
		float dist = 0f;

		if (p.Raycast(ray, out dist))
		{
			hit = ray.origin + ray.direction.normalized * dist;
		}

		mouseHitPos = map.transform.InverseTransformPoint(hit);
	}

	void MoveBrush()
	{
		
		float tileSize = map.tileSize.x / map.pixelsToUnits;
		float x = Mathf.Floor(mouseHitPos.x / tileSize) * tileSize;
		float y = Mathf.Floor(mouseHitPos.y / tileSize) * tileSize;


		float row = (int)(x / tileSize);
		float column = Mathf.Abs(y / tileSize) - 1;

		if (!MouseOnMap)
		{
			return;
		}
		int id = (int)(column * map.mapSize.x + row);
		brush.tileID = id;
		x += map.transform.position.x + tileSize / 2;
		y += map.transform.position.y + tileSize / 2;
		brush.transform.position = new Vector3(x, y, map.transform.position.z);
	}

	void Draw()
	{
		string id = brush.tileID.ToString();

		Vector2 pos = brush.transform.position;

		GameObject tile = GameObject.Find(map.name + "/Tiles/tile_" + id);

		if (tile == null)
		{
			tile = new GameObject("tile_" + id);
			tile.transform.SetParent(map.tiles.transform);
			tile.transform.position = pos;
			tile.AddComponent<SpriteRenderer>();
		}

		tile.GetComponent<SpriteRenderer>().sprite = brush.renderer2D.sprite;
	}

	void RemoveTile()
	{
		string id = brush.tileID.ToString();

		GameObject tile = GameObject.Find(map.name + "/Tiles/tile_" + id);

		if (tile != null)
		{
			DestroyImmediate(tile);
		}
	}

	void ClearMap()
	{
		for (int i = 0; i < map.tiles.transform.childCount; i++)
		{
			Transform t = map.tiles.transform.GetChild(i);
			DestroyImmediate(t.gameObject);
			i--;
		}
	}
}
