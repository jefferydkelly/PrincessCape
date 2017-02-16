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
	string objectName;
	bool destroy = false;
	public CutsceneCreation(string name, string dx, string dy, string dz) {
		prefab = Resources.Load<GameObject> (name);
		position = new Vector3 (float.Parse (dx), float.Parse (dy), float.Parse (dz));
	}

	public CutsceneCreation(string name) {
		objectName = name;
		destroy = true;
	}

	public void Create() {
		if (!destroy) {
			GameObject go = GameObject.Instantiate (prefab);
			go.name = prefab.name;
			go.transform.position = position;
		} else {
			GameObject go = GameObject.Find (objectName);
			if (go) {
				GameObject.Destroy (go);
			}
		}
	}
}

public class CutsceneAdd: CutsceneElement {
	GameObject prefab;

	public CutsceneAdd(GameObject pfab) {
		prefab = pfab;
	}

	public void Add() {
		GameManager.Instance.Player.AddItem (GameObject.Instantiate (prefab));
	}
}

public class CutsceneDisable: CutsceneElement {
	GameObject hideObject;
	public CutsceneDisable(GameObject go) {
		hideObject = go;
	}

	public void Disable() {
		hideObject.SetActive (false);
	}
}

public class CutsceneEnable: CutsceneElement {
	GameObject hideObject;
	public CutsceneEnable(GameObject go) {
		hideObject = go;
	}

	public void Enable() {
		hideObject.SetActive (true);
	}
}

public class CutsceneActivate: CutsceneElement {
	bool activate;
	ActivateableObject ao;

	public CutsceneActivate(ActivateableObject aObj, bool activated) {
		ao = aObj;
		activate = activated;
	}

	public void Activate() {
		if (activate) {
			ao.Activate ();
		} else {
			ao.Deactivate ();
		}
	}

	public float RunTime {
		get {
			return ao.ActivationTime;
		}
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
