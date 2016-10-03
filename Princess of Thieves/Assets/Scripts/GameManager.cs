using UnityEngine;
using System.Collections;

public class GameManager {
	private static GameManager instance = null;
	private GameState state;
	private Player player;
	private Cutscene cutscene;
	private GameManager()
	{
		state = GameState.Play;
		player = GameObject.FindWithTag("Player").GetComponent<Player>();
		//Set up the UI Manager
		//Set up the controller info and pass that to the player
	}

	public void StartCutscene(string cutsceneName)
	{
		IsInCutscene = true;
		cutscene = new Cutscene(cutsceneName);
		cutscene.StartCutscene();
	}
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

	public bool IsPaused
	{
		get
		{
			return (state & GameState.Paused) > 0;
		}

		set
		{
			if (value)
			{
				state |= GameState.Paused;
			}
			else {
				state &= ~GameState.Paused;
			}
		}
	}

	public Cutscene Cutscene
	{
		get
		{
			return cutscene;
		}
	}

	public bool IsInCutscene
	{
		get
		{
			return (state & GameState.Cutscene) > 0;
		}

		set
		{
			if (value)
			{
				state |= GameState.Cutscene | GameState.Paused;
			}
			else {
				state &= ~GameState.Cutscene;
			}
		}
	}

	public Player Player
	{
		get
		{
			return player;
		}
	}
}

[System.Flags]
public enum GameState {
	None = 0,
	Play = 1,
	Menu = 2,
	Cutscene = 4,
	GameOver = 8,
	Paused = 16,
	Any = int.MaxValue
}
