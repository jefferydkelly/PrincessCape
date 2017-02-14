using UnityEngine;
using System.Collections;

public class CutsceneElement
{
	public CutsceneElement nextElement = null;
	public CutsceneElement prevElement = null;
}

public class CutsceneDialog : CutsceneElement
{
	public string speaker = "Character";
	public string dialog = "Hi, I'm a character";
}

public class CameraPan : CutsceneElement
{
	public Vector2 panDistance = Vector2.zero;
	public float time;
	private Vector3 panEnding;
	private bool panTo;

	public CameraPan(Vector2 pd, float t)
	{
		panDistance = pd;
		time = t;
		panTo = false;
	}

	public CameraPan(Vector3 pd, float t)
	{
		panEnding = pd;
		time = t;
		panTo = true;
	}
	public void Start()
	{
		if (panTo)
		{
			panDistance = panEnding - Camera.main.transform.position;
		}
	}
}

public class CutsceneWait : CutsceneElement
{
	public float time;
}

public enum MoveTypes
{
	XY, Rotate
}

public class CutsceneMovement : CutsceneElement
{
	public string mover = "Character";
	public MoveTypes moveType = MoveTypes.XY;
	public float x = 0;
	public float y = 0;
	public float ang = 0;
	public float time = 0;
}

public class CutsceneEffect : CutsceneElement
{
	public EffectType type = EffectType.Show;
	public string affected = "Character";
	public float time = 0.0f;
	public float x = 0.0f;
	public float y = 0.0f;
}

public class CutsceneCreation : CutsceneElement {
	GameObject prefab;
	Vector3 position;
	public CutsceneCreation(string name, string dx, string dy, string dz) {
		prefab = Resources.Load<GameObject> (name);
		position = new Vector3 (float.Parse (dx), float.Parse (dy), float.Parse (dz));
	}

	public void Create() {
		GameObject go = GameObject.Instantiate (prefab);
		go.transform.position = position;
	}
}
public enum EffectType
{
	FadeIn, FadeOut, Show, Hide, FlipHorizontal, FlipVertical, Scale, ScaleX, ScaleY, ScaleXYInd
}

public enum CutsceneElements
{
	Dialog, Move, Effect, SpriteChange
}

[System.Serializable]
public class CutsceneSpriteChange : CutsceneElement
{
	public string affected = "Character";
	public string newSprite = "Character";
}
