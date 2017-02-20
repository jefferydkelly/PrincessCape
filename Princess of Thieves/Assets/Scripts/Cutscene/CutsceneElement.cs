using UnityEngine;
using System.Collections;
using System.IO;

public class CutsceneElement
{
	public CutsceneElement nextElement = null;
	public CutsceneElement prevElement = null;

	protected bool autoAdvance = false;

	public bool AutoAdvance {
		get {
			return autoAdvance;
		}
	}

	public virtual void Run() {
	}
}

/// <summary>
/// A container for cutscene dialog
/// </summary>
public class CutsceneDialog : CutsceneElement
{
	string speaker = "Character";
	string dialog = "Hi, I'm a character";

	/// <summary>
	/// Initializes a new instance of the <see cref="CutsceneDialog"/> class with a speaker and a line.
	/// </summary>
	/// <param name="spk">Spk.</param>
	/// <param name="dia">Dia.</param>
	public CutsceneDialog(string spk, string dia) {
		speaker = spk;
		dialog = dia;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="CutsceneDialog"/> class for duration.
	/// </summary>
	/// <param name="dia">Dia.</param>
	public CutsceneDialog(string dia) {
		speaker = null;
		dialog = dia;
	}

	public override void Run ()
	{
		if (speaker != null) {
			UIManager.Instance.RevealDialog (dialog, speaker);
		} else {
			UIManager.Instance.RevealDialog (dialog);
		}
	}
}

/// <summary>
/// Camera pan.
/// </summary>
public class CameraPan : CutsceneElement
{
	Vector2 panDistance = Vector2.zero;
	float time;
	Vector3 panEnding;
	bool panTo;

	/// <summary>
	/// Initializes a new <see cref="CameraPan"/>.
	/// </summary>
	/// <param name="pd">The distance which the Camera will be panned.</param>
	/// <param name="t">The duration of the pan.</param>
	public CameraPan(Vector2 pd, float t)
	{
		panDistance = pd;
		time = t;
		panTo = false;
	}

	/// <summary>
	/// Initializes a new <see cref="CameraPan"/> to the given location.
	/// </summary>
	/// <param name="pd">The ending of the pan</param>
	/// <param name="t">The duration of the pan</param>
	public CameraPan(Vector3 pd, float t)
	{
		panEnding = pd;
		time = t;
		panTo = true;
	}

	public override void Run() {
		if (panTo)
		{
			panDistance = panEnding - Camera.main.transform.position;
		}

		CameraManager.Instance.Pan (panDistance, time);
	}
}

/// <summary>
/// Cutscene wait.
/// </summary>
public class CutsceneWait : CutsceneElement
{
	float time;

	/// <summary>
	/// Initializes a new instance of the <see cref="CutsceneWait"/> class.
	/// </summary>
	/// <param name="dt">The duration of the wait.</param>
	public CutsceneWait(float dt) {
		time = dt;
	}
	public override void Run ()
	{
		UIManager.Instance.WaitFor (time);
	}
}

public enum MoveTypes
{
	XY, Rotate
}

public class CutsceneMovement : CutsceneElement
{
	string mover = "Character";
	MoveTypes moveType = MoveTypes.XY;
	float x = 0;
	float y = 0;
	float ang = 0;
	float time = 0;

	public CutsceneMovement(string target, MoveTypes mt, float dx, float dy, float dt) {
		mover = target;
		moveType = mt;
		x = dx;
		y = dy;
		time = dt;
	}

	public CutsceneMovement(string target, MoveTypes mt, float angle, float dt) {
		mover = target;
		moveType = mt;
		ang = angle;
		time = dt;
	}

	public override void Run ()
	{
		CutsceneActor myActor = GameManager.Instance.Cutscene.FindActor (mover);

		if (myActor != null) {
			if (moveType == MoveTypes.XY) {
				myActor.MoveTo (new Vector2 (x, y), time);
			} else if (moveType == MoveTypes.Rotate) {
				myActor.StartRotation (ang, time);
			}
		}
	}
}

public class CutsceneEffect : CutsceneElement
{
	EffectType type = EffectType.Show;
	string affected = "Character";
	float time = 0.0f;
	float x = 0.0f;
	float y = 0.0f;

	public CutsceneEffect(string target, EffectType et) {
		affected = target;
		type = et;
		autoAdvance = true;
	}

	public CutsceneEffect(string target, EffectType et, float dt) {
		affected = target;
		type = et;
		time = dt;
	}

	public CutsceneEffect(string target, EffectType et, float dx, float dy) {
		affected = target;
		type = et;
		x = dx;
		if (et == EffectType.Scale || et == EffectType.ScaleX || et == EffectType.ScaleY) {
			time = dy;
		} else {
			y = dy;
			autoAdvance = true;
		}
	}

	public CutsceneEffect(string target, EffectType et, float dx, float dy, float dt) {
		affected = target;
		type = et;
		x = dx;
		y = dy;
		time = dt;
	}
	public override void Run ()
	{
		Cutscene cutscene = GameManager.Instance.Cutscene;
		CutsceneActor myActor = cutscene.FindActor (affected);
        
        if (type == EffectType.Show || type == EffectType.FadeIn) {

            if (myActor == null)
            {
                CutsceneCharacter myCharacter = cutscene.FindCharacter(affected);
                if (myCharacter.characterName == affected)
                {
                    GameObject go = new GameObject(myCharacter.characterName);
                    go.AddComponent<SpriteRenderer>();
                    myActor = go.AddComponent<CutsceneActor>();
                    myActor.MyInfo = myCharacter;
                    myActor.parentCutscene = cutscene;
                    cutscene.AddActorToStage(myActor);
                    //myActor.transform.parent;// = UIManager.Instance.transform;
                }
            }
            if (myActor && myActor.IsHidden) {

				Vector3 aPosition = new Vector3 (x, y);

				myActor.Position = aPosition;

				if (type == EffectType.Show) {
					myActor.IsHidden = false;
					//auto advance
				} else {
					myActor.StartFadeIn (time);
				}
			}
		} else if (type == EffectType.Hide) {
			myActor = cutscene.FindActor (affected);

			if (myActor && !myActor.IsHidden) {
				myActor.IsHidden = true;
			}
			//auto advance
		} else if (type == EffectType.FadeOut) {
			myActor = cutscene.FindActor (affected);

			if (myActor && !myActor.IsHidden) {
				myActor.StartFadeOut (time);
			}
		} else if (type == EffectType.FlipHorizontal) {
			myActor.FlipX ();
			//NextElement ();
		} else if (type == EffectType.FlipVertical) {
			myActor.FlipY ();
			//NextElement ();
		} else if (type == EffectType.Scale) {
			myActor.StartScale (x, time);
		} else if (type == EffectType.ScaleX) {
			myActor.StartScaleX (x, time);
		} else if (type == EffectType.ScaleY) {
			myActor.StartScaleY (x, time);
		} else if (type == EffectType.ScaleXYInd) {
			myActor.StartScaleXY (new Vector3 (x, y, 1), time);
		}
	}
}

public class CutsceneCreation : CutsceneElement {
	GameObject prefab;
	Vector3 position;
	string objectName;
	bool destroy = false;
	public CutsceneCreation(string name, string dx, string dy, string dz) {
		prefab = Resources.Load<GameObject> (name);
		position = new Vector3 (float.Parse (dx), float.Parse (dy), float.Parse (dz));
		autoAdvance = true;
	}

	public CutsceneCreation(string name) {
		objectName = name;
		destroy = true;
		autoAdvance = true;
	}

	public override void Run() {
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
		autoAdvance = true;
	}

	public override void Run() {
		GameManager.Instance.Player.AddItem (GameObject.Instantiate (prefab));
	}
}

public class CutsceneEnable: CutsceneElement {
	GameObject hideObject;
	bool enable = true;
	public CutsceneEnable(GameObject go, bool en) {
		hideObject = go;
		enable = en;
		autoAdvance = true;
	}

	public override void Run() {
		if (hideObject) {
			hideObject.SetActive (enable);
		}
	}
}

public class CutsceneActivate: CutsceneElement {
	bool activate;
	ActivateableObject ao;

	public CutsceneActivate(ActivateableObject aObj, bool activated) {
		ao = aObj;
		activate = activated;
		autoAdvance = (RunTime == 0);
	}

	public override void Run() {
		if (activate) {
			ao.Activate ();
		} else {
			ao.Deactivate ();
		}

		if (RunTime > 0) {
			UIManager.Instance.WaitFor (RunTime);
		}
	}

	public float RunTime {
		get {
			return ao.ActivationTime;
		}
	}
}

public class CutsceneAlign: CutsceneElement {
	bool left;

	public CutsceneAlign(bool l) {
		left = l;
		autoAdvance = true;
	}

	public override void Run() {
		UIManager.Instance.LeftAligned = left;
	}
}

public class CutscenePlay: CutsceneElement {
	AudioClip soundEffect;
	public CutscenePlay(string s) {
		soundEffect = Resources.Load<AudioClip> ("Sounds/" + s);
		autoAdvance = true;
	}

	public override void Run() {
		AudioManager.Instance.PlaySound (soundEffect);
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
	string affected = "Character";
	string newSprite = "Character";

	public CutsceneSpriteChange(string target, string spriteName) {
		affected = target;
        newSprite = spriteName.Trim();
		autoAdvance = true;
	}

	public override void Run ()
	{
		CutsceneActor ca = GameManager.Instance.Cutscene.FindActor (affected);

		if (ca) {
			ca.MySprite = Resources.Load<Sprite> (newSprite);
		}
	}
}
