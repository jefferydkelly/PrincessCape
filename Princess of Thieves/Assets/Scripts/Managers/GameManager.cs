using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager {
	static GameManager instance = null;
	GameState state;
	Player player;
	Cutscene cutscene;

	private GameManager()
	{
		state = GameState.Play;
        GameObject pObj = GameObject.FindWithTag("Player");
        if (pObj) {
            player = pObj.GetComponent<Player>();
        }
		SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
	/// Starts the cutscene.
	/// </summary>
	/// <param name="cutsceneName">Cutscene name.</param>
	public void StartCutscene(TextAsset cutsceneFile)
    {
        IsInCutscene = true;
        cutscene = new Cutscene(cutsceneFile);
        cutscene.StartCutscene();
    }

    /// <summary>
    /// Gets the instance.
    /// </summary>
    /// <value>The instance.</value>
    public static GameManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new GameManager();
			}

			return instance;
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="T:GameManager"/> is paused.
	/// </summary>
	/// <value><c>true</c> if is paused; otherwise, <c>false</c>.</value>
	public bool IsPaused
	{
		get
		{
			return (state & (GameState.Any & ~GameState.Play)) > 0;
		}
	}

	/// <summary>
	/// Gets the cutscene.
	/// </summary>
	/// <value>The cutscene.</value>
	public Cutscene Cutscene
	{
		get
		{
			return cutscene;
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="T:GameManager"/> is in cutscene.
	/// </summary>
	/// <value><c>true</c> if is in cutscene; otherwise, <c>false</c>.</value>
	public bool IsInCutscene
	{
		get
		{
			return (state & GameState.Cutscene) > 0;
		}

		set
		{
			UIManager.Instance.InCutscene = value;

			if (value)
			{
				state |= GameState.Cutscene;
			}
			else {
				state &= ~GameState.Cutscene;
			}
			AudioManager.Instance.Paused = IsPaused;
		}
	}

    public bool IsInMenu
    {
        get
        {
            return (state & GameState.Menu) > 0;
        }

        set
        {
            
            if (value)
            {
                state |= GameState.Menu;
            }
            else
            {
                state &= ~GameState.Menu;
				UIManager.Instance.HideInteraction ();
            }
			UIManager.Instance.ShowMenu = value;
			AudioManager.Instance.Paused = IsPaused;
        }
    }

    public GameState State
    {
        get
        {
            return state;
        }
    }

	/// <summary>
	/// Gets the player.
	/// </summary>
	/// <value>The player.</value>
	public Player Player
	{
		get
        {
            if (player == null)
            {
                GameObject pObj = GameObject.FindWithTag("Player");
                if (pObj)
                {
                    player = pObj.GetComponent<Player>();
                }
            }
        
            return player;
		}
	}

	/// <summary>
	/// Loads the scene.
	/// </summary>
	/// <param name="sceneName">Scene name.</param>
	public void LoadScene(string sceneName)
	{
		SceneManager.LoadSceneAsync(sceneName);

	}

	void OnSceneLoaded(Scene scene, LoadSceneMode ls)
	{
		GameObject[] gos = GameObject.FindGameObjectsWithTag ("Player");
		if (gos.Length > 1) {
			if (player == null) {
				player = gos [0].GetComponent<Player>();
			}

			foreach (GameObject go in gos) {
				if (go != player.gameObject) {
                    go.GetComponent<Player>().Remove();
				}
			}
		}
		GameObject[] checkpoints = GameObject.FindGameObjectsWithTag ("Checkpoint");

		if (checkpoints.Length > 0) {
			foreach (GameObject go in checkpoints) {
				if (go.GetComponent<Checkpoint> ().IsFirst) {
					Vector3 pos = go.transform.position;
					pos.z = 0;
					Player.transform.position = pos;
					break;
				}
			}
		
		}

	}

    public void Reset()
    {
        foreach(ResettableObject ro in GameObject.FindObjectsOfType<ResettableObject>())
        {
            ro.Reset();
        }
    }

	public void EndGame() {
		instance = null;
	}

	public float DistanceToPlayer(GameObject go) {
		return Vector3.Distance (go.transform.position, player.transform.position);
	}

	public bool InPlayerInteractRange(GameObject go) {
		Vector3 difference = (go.transform.position - player.Position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, difference, player.InteractDistance, 1 << LayerMask.NameToLayer("Interactive") | 1 << LayerMask.NameToLayer("Platforms"));
        /*
		foreach(RaycastHit2D hit in Physics2D.RaycastAll(player.Position, difference, player.InteractDistance, 1 << (LayerMask.NameToLayer("Interactive") | LayerMask.NameToLayer("Platforms")))) {
			if (hit.collider.gameObject == go) {
				return true;
			}
		}*/
        if (hit)
        {
            Debug.Log(hit.collider.gameObject.name);
        } else
        {
            Debug.Log("No Hit");
        }
        return hit && hit.collider.gameObject == go;//false;
	}

	
}

[System.Flags]
public enum GameState {
	None = 0,
	Play = 1,
	Menu = 2,
	Cutscene = 4,
	GameOver = 8,
	Any = int.MaxValue
}
