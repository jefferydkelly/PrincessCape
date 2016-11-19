using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelMap : MonoBehaviour {
	public Vector2 mapSize = new Vector2(20, 10);
	public Vector2 tileSize = new Vector2(64, 64);
	public GameObject[,] wall;
	public GameObject[,] field;
	public GameObject[,] background;
	public GameObject[,] foreGround;
	public Vector2 gridSize = new Vector2();
	public int pixelsToUnits = 64;
	public GameObject level = null;
	public GameObject selected;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmosSelected()
	{
		Vector2 pos = transform.position;

		if (level != null)
		{
			Gizmos.color = Color.gray;

			int row = 0;
			int maxColumns = (int)mapSize.x;
			int numCells = (int)(mapSize.x * mapSize.y);
			Vector3 tile = new Vector3(tileSize.x, tileSize.y) / pixelsToUnits;
			Vector2 offset = tile / 2;

			for (int i = 0; i < numCells; i++)
			{
				var column = i % maxColumns;

				float newX = column * tile.x + offset.x + pos.x;
				float newY = -(row * tile.y) - offset.x + pos.y;
				Gizmos.DrawWireCube(new Vector2(newX, newY), tile);

				if (column == maxColumns - 1)
				{
					row++;
				}
			}
			Gizmos.color = Color.white;

			float centerX = pos.x + gridSize.x / 2;
			float centerY = pos.y - gridSize.y / 2;

			Gizmos.DrawWireCube(new Vector2(centerX, centerY), gridSize);
		}
	}

	public Sprite SelectedSprite
	{
		get
		{
			if (selected != null) {
				SpriteRenderer sr = selected.GetComponent<SpriteRenderer>();
				if (sr != null)
				{
					return sr.sprite;
				}
			}
			return null;
		}
	}

	public GameObject GetOccupant(Vector2 v) {
		if (v.x.Between (0, gridSize.x) && v.y.Between (0, gridSize.y)) {
			int ix = (int)v.x;
			int iy = (int)v.y;
			if (foreGround [ix, iy] != null) {
				return foreGround [ix, iy];
			} else if (background [ix, iy] != null) {
				return background [ix, iy];
			} else if (field [ix, iy] != null) {
				return field [ix, iy];
			} else if (wall [ix, iy] != null) {
				return wall [ix, iy];
			}
		}
		return null;
	}
	public bool IsOccupied(Vector2 v, MapLayer ml = MapLayer.Foreground)
	{
		
		GameObject[,] arr;
		switch (ml)
		{
			case MapLayer.Foreground:
				arr = foreGround;
				Debug.Log("Is Foregound Null?" + (foreGround == null));
				break;
			case MapLayer.Background:
				arr = background;
				Debug.Log("Is Backgound Null?" + (background == null));
				break;
			default:
				arr = wall;
				Debug.Log("Is Wall Null?" + (wall == null));
				break;
		}

		//Debug.Log("Is The Array Null? " + (arr == null));

		if (v.x.Between(0, arr.Length - 1) && v.y.Between(0, arr.GetLength((int)v.x) - 1))
		{
			return true;
		}
		return arr[(int)v.x, (int)v.y] != null;
	}

	public void Add(GameObject go, Vector2 startPos, Vector2 dims, Vector3 pos)
	{
		
		MapLayer ml = MapLayer.Wall;
		JDMappableObject mo = go.GetComponent<JDMappableObject>();
		if (mo != null)
		{
			ml = mo.mapLayer;
		}

		GameObject[] collisions = GetCollisions(startPos, dims, ml);

		if (collisions.Length >= 2)
		{
			return;
		}
		GameObject inst = Instantiate(go);
		inst.transform.SetParent(level.transform);
		Debug.Log ((float)ml);
		pos.z = (float)ml;
		inst.transform.position = pos;
		inst.name.Replace("(Clone)", "");

		Vector2 endPos = startPos + dims;
		GameObject[,] arr;
		switch (ml)
		{
			case MapLayer.Foreground:
				arr = foreGround;
				break;
			case MapLayer.Background:
				arr = background;
				break;
			case MapLayer.Field:
				arr = field;
				break;
			default:
				arr = wall;
				break;
		}

		for (int i = (int)startPos.x; i < endPos.x; i++)
		{
			for (int j = (int)startPos.y; j < endPos.y; j++)
			{
				arr[i, j] = inst;
			}
		}

		if (collisions.Length == 1)
		{
			DestroyImmediate(collisions[0]);	
		}
	}

	public GameObject[] GetCollisions(Vector2 startPos, Vector2 dims, MapLayer ml)
	{
		Vector2 endPos = startPos + dims;
		GameObject[,] arr = foreGround;
		List<GameObject> collisions = new List<GameObject>();
		if (ml == MapLayer.Background)
		{
			arr = background;
		}
		else if (ml == MapLayer.Wall)
		{
			arr = wall;
		}
		for (int i = (int)startPos.x; i < endPos.x; i++)
		{
			for (int j = (int)startPos.y; j < endPos.y; j++)
			{
				GameObject go = arr[i, j];
				if (go != null)
				{
					if (!collisions.Contains(go))
					{
						collisions.Add(go);
					}
				}
			}
		}
		return collisions.ToArray();
	}
}
