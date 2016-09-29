using UnityEngine;
using System.Collections;

public class GameManager {
	private static GameManager instance = null;
	private GameState state;

	private GameManager()
	{
		state = GameState.Play;
		//Set up the UI Manager
		//Set up the controller info and pass that to the player
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
