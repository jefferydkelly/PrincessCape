using UnityEngine;
using System.Collections;

public class GameManager {
	private static GameManager instance = null;

	private GameManager()
	{
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
}

public enum GameState {
	None,
	Play,
	Menu,
	Cutscene,
	Pause
}
