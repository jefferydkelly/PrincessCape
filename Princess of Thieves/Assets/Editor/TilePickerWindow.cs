using UnityEngine;
using System.Collections;
using UnityEditor;

public class TilePickerWindow : EditorWindow {
	public enum Scale
	{
		x1 = 1,
		x2 = 2,
		x3 = 3,
		x4 = 4,
		x5 = 5
	}

	Scale scale;

	public Vector2 scrollPosition = Vector2.zero;
	Vector2 currentSelection;
	[MenuItem("Window/Tile Picker")]
	public static void OpenTilePickerWindow()
	{
		EditorWindow window = GetWindow(typeof(TilePickerWindow));
		GUIContent title = new GUIContent();
		title.text = "Tile Picker";
		window.titleContent = title;

	}

	void OnGUI()
	{
		if (Selection.activeGameObject == null)
		{
			return;
		}

		TileMap tm = Selection.activeGameObject.GetComponent<TileMap>();

		if (tm != null && tm.texture2d != null)
		{
			scale = (Scale)EditorGUILayout.EnumPopup("Zoom", scale);
			Vector2 newTextureSize = new Vector2(tm.texture2d.width, tm.texture2d.height) * (int)scale;
			Vector2 offSet = new Vector2(10, 25);
			Rect viewPort = new Rect(0, 0, position.width - 5, position.height - 5);
			Rect contentSize = new Rect(Vector2.zero, newTextureSize + offSet);
			scrollPosition = GUI.BeginScrollView(viewPort, scrollPosition, contentSize);
			GUI.DrawTexture(new Rect(offSet, newTextureSize), tm.texture2d);

			Vector2 tile = tm.tileSize * (int)scale;
			Vector2 grid = new Vector2(newTextureSize.x / tile.x, newTextureSize.y / tile.y);
			Vector2 selectionPos = new Vector2(tile.x * currentSelection.x, tile.y * currentSelection.y) + offSet;
			Texture2D boxTex = new Texture2D(1, 1);
			boxTex.SetPixel(0, 0, new Color(0, 0.5f, 1f, 0.4f));
			boxTex.Apply();
			GUIStyle style = new GUIStyle(GUI.skin.customStyles[0]);
			style.normal.background = boxTex;
			GUI.Box(new Rect(selectionPos, tile), "", style);

			Event cEvent = Event.current;
			Vector2 mousePos = cEvent.mousePosition;

			if (cEvent.type == EventType.mouseDown && cEvent.button == 0)
			{
				currentSelection.x = Mathf.Floor((mousePos.x + scrollPosition.x) / tm.tileSize.x);
				currentSelection.y = Mathf.Floor((mousePos.y + scrollPosition.y) / tm.tileSize.y);

				currentSelection.x = Mathf.Min(currentSelection.x, grid.x - 1);
				currentSelection.y = Mathf.Min(currentSelection.y, grid.y - 1);

				tm.tileID = (int)(currentSelection.x + (currentSelection.y * grid.x) + 1);
				Repaint();
			}
			GUI.EndScrollView();
		}
	}
}
