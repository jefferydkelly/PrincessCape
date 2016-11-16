using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LevelMap))]
public class MapEditor : Editor {

	public LevelMap map;
	TileBrush brush;
	Vector3 mouseHitPos;
	Vector2 spriteUnitDims;

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
		UpdateBrush(map.SelectedSprite);
		GameObject oldSel = map.selected;
		map.selected = EditorGUILayout.ObjectField("GameObject", map.selected, typeof(GameObject), false) as GameObject;

		if (map.selected != null)
		{
			if (map.selected.GetComponent<JDMappableObject>() == null)
			{
				EditorGUILayout.HelpBox("You need to select an object that is a child of JDMappableObject", MessageType.Warning);
				map.selected = oldSel;
			}
			if (map.selected != oldSel)
			{

				SpriteRenderer sr = map.selected.GetComponent<SpriteRenderer>();
				if (sr != null)
				{
					spriteUnitDims = map.SelectedSprite.textureRect.size / map.pixelsToUnits;
					spriteUnitDims.x = Mathf.Max(Mathf.Ceil(spriteUnitDims.x), 1);
					spriteUnitDims.y = Mathf.Max(Mathf.Ceil(spriteUnitDims.y), 1);
					UpdateBrush(map.SelectedSprite);
				}
				else {
					UpdateBrush(null);
				}
			}
			else {
				UpdateBrush(null);
			}
		}
	

		if (map.selected == null)
		{
			EditorGUILayout.HelpBox("You have not selected a GameObject.", MessageType.Warning);
		}
		else {
			EditorGUILayout.LabelField("Object Size: ", spriteUnitDims.ToXString());
			EditorGUILayout.LabelField("Tile Size: ", map.tileSize.ToXString());
			EditorGUILayout.LabelField("Grid Size In Units", map.gridSize.ToXString());
			EditorGUILayout.LabelField("Pixels to Units", map.pixelsToUnits.ToString());
			UpdateBrush(map.SelectedSprite);

			if (GUILayout.Button("Clear Level"))
			{
				if (EditorUtility.DisplayDialog("Clear The Level", "Are you sure?", "Clear", "Do Not Clear"))
				{
					ClearMap();
				}
			}
		}
		EditorGUILayout.EndVertical();
	}

	void OnEnable()
	{
		map = target as LevelMap;
		Tools.current = Tool.View;
		if (map.level == null)
		{
			
			map.level = new GameObject("Level");
			map.level.transform.SetParent(map.transform);
			map.level.transform.position = Vector3.zero;
			map.foreGround = new GameObject[(int)map.mapSize.x, (int)map.mapSize.y];
			map.background = new GameObject[(int)map.mapSize.x, (int)map.mapSize.y];
			map.wall = new GameObject[(int)map.mapSize.x, (int)map.mapSize.y];
		}
		map.gridSize = new Vector2(map.tileSize.x * map.mapSize.x, map.tileSize.y * map.mapSize.y) / map.pixelsToUnits;
		if (map.selected != null)
		{
			spriteUnitDims = map.SelectedSprite.textureRect.size / map.pixelsToUnits;
			spriteUnitDims.x = Mathf.Max(Mathf.Ceil(spriteUnitDims.x), 1);
			spriteUnitDims.y = Mathf.Max(Mathf.Ceil(spriteUnitDims.y), 1);

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

			if (map.selected != null && MouseOnMap)
			{
				Event cEvent = Event.current;

				if (cEvent.type == EventType.KeyUp)
				{
					if (cEvent.keyCode == KeyCode.Space)
					{
						Draw();
						cEvent.Use();
					}
				}
				else if (cEvent.alt)
				{
					RemoveTile();
				}
			}
		}
	}

	void CreateBrush()
	{
		if (map.SelectedSprite != null)
		{
			GameObject go = new GameObject("Brush");
			go.transform.SetParent(map.transform);

			brush = go.AddComponent<TileBrush>();
			brush.renderer2D = go.AddComponent<SpriteRenderer>();
			brush.renderer2D.sortingOrder = 1000;
			brush.size = map.SelectedSprite.textureRect.size / map.pixelsToUnits;
			brush.UpdateBrush(map.SelectedSprite);
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
		else {
			NewBrush();
		}
	}

	void UpdateCalculations()
	{
		ClearMap();
		map.foreGround = new GameObject[(int)map.mapSize.x, (int)map.mapSize.y];
		map.background = new GameObject[(int)map.mapSize.x, (int)map.mapSize.y];
		map.wall = new GameObject[(int)map.mapSize.x, (int)map.mapSize.y];
		map.gridSize = new Vector2(map.tileSize.x * map.mapSize.x, map.tileSize.y * map.mapSize.y) / map.pixelsToUnits;
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

		if ((int)spriteUnitDims.x % 2 == 0)
		{
			x += tileSize / 2;
		} 
		if ((int)spriteUnitDims.y % 2 == 0)
		{
			y += tileSize / 2;
		}


		float row = Mathf.FloorToInt(x / tileSize);
	;
		float column = Mathf.Abs(Mathf.FloorToInt(y / tileSize)) - 1;

		if (column - Mathf.Floor(spriteUnitDims.y / 2) < 0 || column + Mathf.Floor(spriteUnitDims.y / 2) > map.gridSize.y)
		{
			return;
		}

		if (row - Mathf.Floor(spriteUnitDims.x / 2) < -1 || row + Mathf.Floor(spriteUnitDims.x / 2) > map.gridSize.x - 1)
		{
			return;
		}

		if (!MouseOnMap)
		{
			return;
		}

		x += map.transform.position.x + tileSize / 2;
		y += map.transform.position.y + tileSize / 2;
		brush.transform.position = new Vector3(x, y, map.transform.position.z);
	}

	void Draw()
	{
		float tileSize = map.tileSize.x / map.pixelsToUnits;
		float x = Mathf.Floor(mouseHitPos.x / tileSize) * tileSize;
		float y = Mathf.Floor(mouseHitPos.y / tileSize) * tileSize;

		if ((int)spriteUnitDims.x % 2 == 0)
		{
			x += tileSize / 2;
		}
		if ((int)spriteUnitDims.y % 2 == 0)
		{
			y += tileSize / 2;
		}


		int row = Mathf.FloorToInt(x / tileSize);

		int column = Mathf.Abs(Mathf.FloorToInt(y / tileSize)) - 1;
		Vector2 startPos = new Vector2(row, column) - (spriteUnitDims / 2);
	
		map.Add(map.selected, startPos, spriteUnitDims, brush.transform.position);

		/*
		string id = brush.tileID.ToString();

		Vector2 pos = brush.transform.position;

		GameObject tile = GameObject.Find(map.name + "/Tiles/tile_" + id);

		if (tile == null)
		{
			tile = new GameObject("tile_" + id);
			tile.transform.SetParent(map.level.transform);
			tile.transform.position = pos;
			tile.AddComponent<SpriteRenderer>();
		}

		tile.GetComponent<SpriteRenderer>().sprite = brush.renderer2D.sprite;*/
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
		for (int i = 0; i < map.level.transform.childCount; i++)
		{
			Transform t = map.level.transform.GetChild(i);
			DestroyImmediate(t.gameObject);
			i--;
		}
	}
}
