using UnityEngine;
using System.Collections;

public class HPMPBars : MonoBehaviour {

	public Vector2 pos = new Vector2(20, 40);
	public Vector2 size = new Vector2(200, 20);
	public float spacing = 10;
	public Texture2D backgroundTexture;
	public Texture2D hpForegroundTexture;
	public Texture2D mpForegroundTexture;

	void OnGUI()
	{
		GUI.BeginGroup(new Rect(pos, size));
			GUI.Box(new Rect(pos, size), backgroundTexture);

			GUI.BeginGroup(new Rect(0, 0, size.x * GameManager.Instance.Player.HPPercent, size.y));
				GUI.Box(new Rect(Vector2.zero, size), hpForegroundTexture);
			GUI.EndGroup();
		GUI.EndGroup();

		Vector3 mpBarPos = pos + new Vector2 (0, size.y + spacing);
		GUI.BeginClip (new Rect (mpBarPos, size));
			GUI.Box (new Rect (mpBarPos, size), backgroundTexture);
			
		GUI.BeginGroup (new Rect (0, 0, size.x * GameManager.Instance.Player.MPPercent, size.y));
				GUI.Box(new Rect(Vector2.zero, size), mpForegroundTexture);
			GUI.EndGroup ();
		GUI.EndGroup ();

	}
}
