using UnityEngine;
using System.Collections;

public class MPBar : MonoBehaviour {

	public Vector2 pos = new Vector2(20, 40);
	public Vector2 size = new Vector2(200, 20);
	public Texture2D backgroundTexture;
	public Texture2D foregroundTexture;

	void OnGUI()
	{
		GUI.BeginGroup(new Rect(pos, size));
		GUI.Box(new Rect(pos, size), backgroundTexture);
		GUI.BeginGroup(new Rect(0, 0, size.x * GameManager.Instance.Player.MPPercent, size.y));
		GUI.Box(new Rect(Vector2.zero, size), foregroundTexture);
		GUI.EndGroup();
		GUI.EndGroup();

	}
}
