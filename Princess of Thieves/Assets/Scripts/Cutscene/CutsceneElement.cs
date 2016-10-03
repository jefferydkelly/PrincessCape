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
