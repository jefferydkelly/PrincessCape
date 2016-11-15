using UnityEngine;
using System.Collections;

public class LevelMap : MonoBehaviour {
	public Vector2 mapSize = new Vector2(20, 10);
	public Vector2 tileSize = new Vector2(64, 64);
	public GameObject[] objReferences;
	public Vector2 gridSize = new Vector2();
	public int pixelsToUnits = 64;
	public GameObject level = null;
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
}
