using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour {
    [SerializeField]
    Texture2D normalTexture;
    [SerializeField]
    Texture2D gloveTexture;
    [SerializeField]
    Texture2D magGlassTexture;
    [SerializeField]
    Texture2D pushGloveTexture;
    [SerializeField]
    Texture2D pullGloveTexture;
    [SerializeField]
    Texture2D halfGloveTexture;

    private static CursorManager instance = null;
    CursorState state = CursorState.Normal;
    // Use this for initialization
    void Start () {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this);
        }
        Cursor.SetCursor(normalTexture, Vector2.zero, CursorMode.ForceSoftware);
	}

    public CursorState State
    {
        get
        {
            return state;
        }

        set
        {
            state = value;

            switch(state)
            {
                case CursorState.Normal:
                    Cursor.SetCursor(normalTexture, Vector2.zero, CursorMode.ForceSoftware);
                    break;
                case CursorState.Sign:
                    Cursor.SetCursor(magGlassTexture, Vector2.zero, CursorMode.ForceSoftware);
                    break;
                case CursorState.Block:
                    Cursor.SetCursor(gloveTexture, Vector2.zero, CursorMode.ForceSoftware);
                    break;
                case CursorState.Metal:
                    if (GameManager.Instance.Player.HasPullGloveEquipped)
                    {
                        if (GameManager.Instance.Player.HasPushGloveEquipped)
                        {
                            Cursor.SetCursor(halfGloveTexture, Vector2.zero, CursorMode.ForceSoftware);
                        } else
                        {
                            Cursor.SetCursor(pullGloveTexture, Vector2.zero, CursorMode.ForceSoftware);
                        }
                    }
                    else if (GameManager.Instance.Player.HasPushGloveEquipped)
                    {
                        Cursor.SetCursor(pushGloveTexture, Vector2.zero, CursorMode.ForceSoftware);
                    }
                    break;
            }
        }
    }

    public static CursorManager Instance
    {
        get
        { 
            return instance;
        }
    }
}

public enum CursorState
{
    Normal,
    Metal,
    Block,
    Sign
}
