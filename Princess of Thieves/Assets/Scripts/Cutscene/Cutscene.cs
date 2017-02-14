using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class Cutscene
{
	List<CutsceneElement> elements = new List<CutsceneElement>();
	public List<CutsceneCharacter> characters = new List<CutsceneCharacter>();
	private List<CutsceneActor> charactersOnStage;
	public GameObject characterPrefab;
	CutsceneElement head = null;
	CutsceneElement currentNode = null;
   
	// Use this for initialization
    public Cutscene(TextAsset text)
    {
        charactersOnStage = new List<CutsceneActor>();
        elements = new List<CutsceneElement>();
        foreach (string line in text.text.Split('\n'))
        {
            string[] parts = line.Split(' ');
            CutsceneElement c = null;
            string p = parts[0].ToLower();
			if (p == "show") {
				CutsceneEffect ce = new CutsceneEffect ();
				ce.type = EffectType.Show;
				ce.affected = parts [1];
				if (parts.Length > 2) {
					ce.x = float.Parse (parts [2]);
					ce.y = float.Parse (parts [3]);
				}
				c = ce;
			} else if (p == "hide") {
				CutsceneEffect ce = new CutsceneEffect ();
				ce.type = EffectType.Hide;
				ce.affected = parts [1];
				c = ce;
			} else if (p == "fade-in") {
				CutsceneEffect ce = new CutsceneEffect ();
				ce.type = EffectType.FadeIn;
				ce.affected = parts [1];
				ce.time = float.Parse (parts [2]);
				if (parts.Length == 5) {

					ce.x = float.Parse (parts [3]);
					ce.y = float.Parse (parts [4]);

				}

				c = ce;
			} else if (p == "fade-out") {
				CutsceneEffect ce = new CutsceneEffect ();
				ce.type = EffectType.FadeOut;
				ce.affected = parts [1];
				ce.time = float.Parse (parts [2]);

				c = ce;
			} else if (p == "flip-x") {
				CutsceneEffect ce = new CutsceneEffect ();
				ce.type = EffectType.FlipHorizontal;
				ce.affected = parts [1];
				c = ce;
			} else if (p == "flip-y") {
				CutsceneEffect ce = new CutsceneEffect ();
				ce.type = EffectType.FlipVertical;
				ce.affected = parts [1];
				c = ce;
			} else if (p == "scale") {
				CutsceneEffect ce = new CutsceneEffect ();
				ce.type = EffectType.Scale;
				ce.affected = parts [1];
				ce.x = float.Parse (parts [2]);
				ce.time = float.Parse (parts [3]);
				c = ce;
			} else if (p == "scalex") {
				CutsceneEffect ce = new CutsceneEffect ();
				ce.type = EffectType.ScaleX;
				ce.affected = parts [1];
				ce.x = float.Parse (parts [2]);
				ce.time = float.Parse (parts [3]);
				c = ce;
			} else if (p == "scaley") {
				CutsceneEffect ce = new CutsceneEffect ();
				ce.type = EffectType.ScaleY;
				ce.affected = parts [1];
				ce.y = float.Parse (parts [2]);
				ce.time = float.Parse (parts [3]);
				c = ce;
			} else if (p == "scalexy") {
				CutsceneEffect ce = new CutsceneEffect ();
				ce.type = EffectType.ScaleXYInd;
				ce.affected = parts [1];
				ce.x = float.Parse (parts [2]);
				ce.y = float.Parse (parts [3]);
				ce.time = float.Parse (parts [4]);
				c = ce;
			} else if (p == "rotate") {
				CutsceneMovement cm = new CutsceneMovement ();
				cm.moveType = MoveTypes.Rotate;
				cm.mover = parts [1];
				cm.ang = float.Parse (parts [2]);
				cm.time = parts.Length == 4 ? float.Parse (parts [3]) : 0;
				c = cm;
			} else if (p == "move") {
				CutsceneMovement cm = new CutsceneMovement ();
				cm.moveType = MoveTypes.XY;
				cm.mover = parts [1];
				cm.x = float.Parse (parts [2]);
				cm.y = float.Parse (parts [3]);
				cm.time = parts.Length == 5 ? float.Parse (parts [4]) : 0;
				c = cm;
			} else if (p == "character") {
				if (parts.Length == 2) {
					CreateCharacter (parts [1]);
				} else {
					CreateCharacter (parts [1], parts [2]);
				}
				continue;
			} else if (p == "swap-sprite") {
				CutsceneSpriteChange csc = new CutsceneSpriteChange ();
				csc.affected = parts [1];
				csc.newSprite = parts [2];
				c = csc;
			} else if (p == "pan") {
				if (parts [1] == "to") {
					GameObject go = GameObject.Find (parts [2]);
					c = new CameraPan (go.transform.position, float.Parse (parts [3]));
				} else {
					c = new CameraPan (new Vector2 (float.Parse (parts [1]), float.Parse (parts [2])), float.Parse (parts [3]));
				}
			} else if (p == "wait") {
				CutsceneWait cw = new CutsceneWait ();
				cw.time = float.Parse (parts [1]);
				c = cw;
			} else if (p == "create") {
				c = new CutsceneCreation (parts [1], parts [2], parts [3], parts [4]);
			} else if (p == "add") {
				c = new CutsceneAdd (Resources.Load<GameObject> (parts [1]));
			} else if (p == "disable") {
				GameObject go;
				CutsceneActor ca = FindActor (parts [1]);
				if (ca == null) {
					go = GameObject.Find (parts [1]);
				} else {
					go = ca.gameObject;
				}
				c = new CutsceneDisable (go);
			} else if (p == "enable") {
				GameObject go;
				CutsceneActor ca = FindActor (parts [1]);
				if (ca == null) {
					go = GameObject.Find (parts [1]);
				} else {
					go = ca.gameObject;
				}
				c = new CutsceneEnable (go);
			} else if (p == "activate") {
				GameObject go = GameObject.Find (parts [1]);
				if (go != null) {
					ActivateableObject ao = go.GetComponent<ActivateableObject> ();
					if (ao != null) {
						c = new CutsceneActivate (ao, parts [2] == "true");
					}
				}
			}
			else
			{
				CutsceneDialog cd = new CutsceneDialog();
				parts = line.Split(':');
				cd.speaker = parts[0];
				cd.dialog = parts[1];
				c = cd;
			}

            if (elements.Count == 0)
            {
                head = c;
            }
            else
            {
                CutsceneElement d = elements[elements.Count - 1];
                d.nextElement = c;
                c.prevElement = d;
            }
            elements.Add(c);
        }
    }

	public void StartCutscene()
	{
		GameManager.Instance.IsInCutscene = true;
		UIManager.Instance.ShowBoxes = false;
		NextElement();
	}

	/// <summary>
	/// Advances to the next element if it exists.
	/// Otherwise, it ends the cutscene and removes everything from the screen.
	/// </summary>
	public void NextElement()
	{
		if (currentNode == null)
		{
			currentNode = head;
		}
		else if (currentNode.nextElement != null)
		{
			currentNode = currentNode.nextElement;
		}
		else {
			//End the cutscene
			UIManager.Instance.ShowBoxes = true;
			GameManager.Instance.IsInCutscene = false;
            UIManager.Instance.StartCoroutine("HideDialog");
    
			foreach (CutsceneActor ca in charactersOnStage)
			{
				ca.DestroySelf();
			}
				
			return;
		}

		if (currentNode is CutsceneDialog) {
			CutsceneDialog d = currentNode as CutsceneDialog;

			UIManager.Instance.RevealDialog (d.dialog, d.speaker);
		} else if (currentNode is CutsceneMovement) {
			CutsceneMovement m = currentNode as CutsceneMovement;
			//Get the appropriate sprite and move it
			CutsceneActor myActor = FindActor (m.mover);

			if (myActor != null) {
				if (m.moveType == MoveTypes.XY) {
					myActor.MoveTo (new Vector2 (m.x, m.y), m.time);
				} else if (m.moveType == MoveTypes.Rotate) {
					myActor.StartRotation (m.ang, m.time);
				}
			}
		} else if (currentNode is CutsceneEffect) {
			CutsceneEffect e = currentNode as CutsceneEffect;
			//Get the appropriate sprite and do the effect
			CutsceneActor myActor = FindActor (e.affected);
			if (e.type == EffectType.Show || e.type == EffectType.FadeIn) {
				if (myActor == null) {
					CutsceneCharacter myCharacter = FindCharacter (e.affected);
					if (myCharacter.characterName == e.affected) {
						GameObject go = new GameObject (myCharacter.characterName);
						go.AddComponent<SpriteRenderer> ();
						myActor = go.AddComponent<CutsceneActor> ();
						myActor.MyInfo = myCharacter;
						myActor.parentCutscene = this;
						charactersOnStage.Add (myActor);
						//myActor.transform.parent;// = UIManager.Instance.transform;
					}
				}

				if (myActor && myActor.IsHidden) {

					Vector3 aPosition = new Vector3 (e.x, e.y);

					myActor.Position = aPosition;

					if (e.type == EffectType.Show) {
						myActor.IsHidden = false;
						NextElement ();
					} else {
						myActor.StartFadeIn (e.time);
					}
				}
			} else if (e.type == EffectType.Hide) {
				myActor = FindActor (e.affected);

				if (myActor && !myActor.IsHidden) {
					myActor.IsHidden = true;
					NextElement ();
				}
			} else if (e.type == EffectType.FadeOut) {
				myActor = FindActor (e.affected);

				if (myActor && !myActor.IsHidden) {
					myActor.StartFadeOut (e.time);
				}
			} else if (e.type == EffectType.FlipHorizontal) {
				myActor.FlipX ();
				NextElement ();
			} else if (e.type == EffectType.FlipVertical) {
				myActor.FlipY ();
				NextElement ();
			} else if (e.type == EffectType.Scale) {
				myActor.StartScale (e.x, e.time);
			} else if (e.type == EffectType.ScaleX) {
				myActor.StartScaleX (e.x, e.time);
			} else if (e.type == EffectType.ScaleY) {
				myActor.StartScaleY (e.x, e.time);
			} else if (e.type == EffectType.ScaleXYInd) {
				myActor.StartScaleXY (new Vector3 (e.x, e.y, 1), e.time);
			}
		} else if (currentNode is CutsceneSpriteChange) {
			CutsceneSpriteChange csc = currentNode as CutsceneSpriteChange;
			CutsceneActor ca = FindActor (csc.affected);

			if (ca) {
				ca.MySprite = Resources.Load<Sprite> (csc.newSprite);
			}
			NextElement ();

		} else if (currentNode is CameraPan) { 
			CameraPan cp = currentNode as CameraPan;
			cp.Start ();
			CameraManager.Instance.Pan (cp.panDistance, cp.time);
		} else if (currentNode is CutsceneWait) {
			CutsceneWait cw = (currentNode as CutsceneWait);
			UIManager.Instance.WaitFor (cw.time);
		} else if (currentNode is CutsceneCreation) {
			(currentNode as CutsceneCreation).Create ();
			NextElement ();
		} else if (currentNode is CutsceneAdd) {
			(currentNode as CutsceneAdd).Add ();
			NextElement ();
		} else if (currentNode is CutsceneDisable) {
			(currentNode as CutsceneDisable).Disable ();
			NextElement ();
		} else if (currentNode is CutsceneEnable) {
			(currentNode as CutsceneEnable).Enable ();
			NextElement ();
		} else if (currentNode is CutsceneActivate) {
			CutsceneActivate ca = (currentNode as CutsceneActivate);
			ca.Activate ();
			UIManager.Instance.WaitFor (ca.RunTime);
		}
	}

	/// <summary>
	/// Creates a character dynamically from the sprite in Resources with the same name.
	/// </summary>
	/// <param name="charName">The name of the character and the sprite.</param>
	public void CreateCharacter(string charName)
	{
		Sprite s = Resources.Load<Sprite>(charName);
		if (s)
		{
			CutsceneCharacter cc = new CutsceneCharacter(charName, s);
			characters.Add(cc);
		}
	}

	public void CreateCharacter(string charName, string spriteName)
	{
		Sprite s = Resources.Load<Sprite>(spriteName);
		if (s)
		{
			CutsceneCharacter cc = new CutsceneCharacter(charName, s);
			characters.Add(cc);
		}
	}

	/// <summary>
	/// Finds the actor with the given name.
	/// </summary>
	/// <returns>The actor.</returns>
	/// <param name="actorName">The Actor's name.</param>

	CutsceneActor FindActor(string actorName)
	{
		foreach (CutsceneActor ca in charactersOnStage)
		{
			if (ca.CharacterName == actorName)
			{
				return ca;
			}
		}
		return null;
	}

	/// <summary>
	/// Finds the character with the given name.
	/// </summary>
	/// <returns>The character.</returns>
	/// <param name="characterName">The Character's name.</param>
	CutsceneCharacter FindCharacter(string characterName)
	{
		foreach (CutsceneCharacter cc in characters)
		{
			if (cc.characterName == characterName)
			{
				return cc;
			}
		}
		return new CutsceneCharacter();
	}
}

/// <summary>
/// A struct for holding the information for CutsceneCharacters
/// </summary>
[System.Serializable]
public struct CutsceneCharacter
{
	public string characterName;
	public Sprite sprite;

	/// <summary>
	/// Initializes a new instance of the <see cref="T:CutsceneCharacter"/> struct.
	/// </summary>
	/// <param name="n">The character's name.</param>
	/// <param name="s">The sprite that represents the character.</param>
	public CutsceneCharacter(string n, Sprite s)
	{
		characterName = n;
		sprite = s;
	}
}
