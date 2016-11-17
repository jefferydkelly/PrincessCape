using UnityEngine;
using System.Collections;

public class CoverController : JDMappableObject, InteractiveObject {

	/// <summary>
	/// Hides the player behind the cover of the object or causes the Player to leave cover.
	/// </summary>
	public void Interact()
	{
		Player player = GameManager.Instance.Player;
		player.Hidden = true;
		player.transform.position = transform.position - new Vector3(0, gameObject.HalfHeight() - player.HalfHeight, 0);

	}
}
