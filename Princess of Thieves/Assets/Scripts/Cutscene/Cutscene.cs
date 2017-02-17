﻿using UnityEngine;
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
				c = new CutsceneEffect (parts [1], EffectType.Show, float.Parse (parts [2]), float.Parse (parts [3]));
			} else if (p == "hide") {
				c = new CutsceneEffect (parts [1], EffectType.Hide);
			} else if (p == "fade-in") {
				c = new CutsceneEffect (parts [1], EffectType.FadeIn, float.Parse (parts [3]), float.Parse (parts [4]), float.Parse (parts [2]));
			} else if (p == "fade-out") {
				c = new CutsceneEffect (parts [1], EffectType.FadeOut, float.Parse (parts [2]));
			} else if (p == "flip-x") {
				c = new CutsceneEffect (parts [1], EffectType.FlipHorizontal);
			} else if (p == "flip-y") {
				c = new CutsceneEffect (parts [1], EffectType.FlipVertical);
			} else if (p == "scale") {
				c = new CutsceneEffect (parts [1], EffectType.Scale, float.Parse (parts [2]), float.Parse (parts [3])); 
			} else if (p == "scalex") {
				c = new CutsceneEffect (parts [1], EffectType.ScaleX, float.Parse (parts [2]), float.Parse (parts [3])); 
			} else if (p == "scaley") {
				c = new CutsceneEffect (parts [1], EffectType.ScaleY, float.Parse (parts [2]), float.Parse (parts [3])); 
			} else if (p == "scalexy") {
				c = new CutsceneEffect (parts [1], EffectType.Scale, float.Parse (parts [2]), float.Parse (parts [3]), float.Parse(parts[4])); 
			} else if (p == "rotate") {
				c = new CutsceneMovement (parts [1], MoveTypes.Rotate, float.Parse (parts [2]), parts.Length == 4 ? float.Parse (parts [3]) : 0);
			} else if (p == "move") {
				c = new CutsceneMovement (parts [1], MoveTypes.XY, float.Parse (parts [2]), float.Parse (parts [3]), parts.Length == 5 ? float.Parse (parts [4]) : 0);
			} else if (p == "character") {
				if (parts.Length == 2) {
					CreateCharacter (parts [1]);
				} else {
					CreateCharacter (parts [1], parts [2]);
				}
				continue;
			} else if (p == "swap-sprite") {
				c = new CutsceneSpriteChange (parts[1], parts[2]);
			} else if (p == "pan") {
				if (parts [1] == "to") {
					GameObject go = GameObject.Find (parts [2]);
					c = new CameraPan (go.transform.position, float.Parse (parts [3]));
				} else {
					c = new CameraPan (new Vector2 (float.Parse (parts [1]), float.Parse (parts [2])), float.Parse (parts [3]));
				}
			} else if (p == "wait") {
				c = new CutsceneWait (float.Parse (parts [1]));
			} else if (p == "create") {
				c = new CutsceneCreation (parts [1], parts [2], parts [3], parts [4]);
			} else if (p == "destroy") {
				c = new CutsceneCreation (parts [1]);
			} else if (p == "add") {
				c = new CutsceneAdd (Resources.Load<GameObject> (parts[1]));
			} else if (p == "disable") {
				GameObject go;
				CutsceneActor ca = FindActor (parts [1]);
				if (ca == null) {
					go = GameObject.Find (parts [1]);
				} else {
					go = ca.gameObject;
				}
				c = new CutsceneEnable (go, false);
			} else if (p == "enable") {
				GameObject go;
				CutsceneActor ca = FindActor (parts [1]);
				if (ca == null) {
					go = GameObject.Find (parts [1]);
				} else {
					go = ca.gameObject;
				}
				c = new CutsceneEnable (go, true);
			} else if (p == "activate") {
				GameObject go = GameObject.Find (parts [1]);
				if (go != null) {
					ActivateableObject ao = go.GetComponent<ActivateableObject> ();
					if (ao != null) {
						c = new CutsceneActivate (ao, parts [2] == "true");
					}
				}
			} else if (p == "align") {
				c = new CutsceneAlign (parts [1] == "left");
			} else if (p == "play") {
				c = new CutscenePlay (parts [1]);
			}
			else
			{
				parts = line.Split(':');
				if (parts.Length == 2) {
					c = new CutsceneDialog (parts [0], parts [1].Trim ());
				} else {
					c = new CutsceneDialog (parts [0].Trim ());
				}
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
			
		currentNode.Run ();

		if (currentNode.AutoAdvance) {
			NextElement ();
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
		Sprite s = Resources.LoadAll<Sprite>(spriteName)[0];
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

	public CutsceneActor FindActor(string actorName)
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
	public CutsceneCharacter FindCharacter(string characterName)
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

	/// <summary>
	/// Adds the actor to stage.
	/// </summary>
	/// <param name="actor">Actor.</param>
	public void AddActorToStage(CutsceneActor actor) {
		charactersOnStage.Add (actor);
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
