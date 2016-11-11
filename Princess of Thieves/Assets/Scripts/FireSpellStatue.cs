using UnityEngine;
using System.Collections;

public class FireSpellStatue : SpellStatue {
	public TextAsset cutscene;
	// Use this for initialization
	void Start () {
		spellType = SpellType.Fire;
	}
	
	public override void Interact()
	{
		if (!activated)
		{
			activated = true;
			Spell s = new FireSpell();

			UIManager.Instance.ShowMessage("You learned " + s.SpellName, 3);
			GameManager.Instance.Player.AddSpell(s);
			StartCoroutine(gameObject.RunAfter(StartCutscene, 3));

		}
	}

	void StartCutscene()
	{
		GameManager.Instance.StartCutscene(cutscene);
	}
}
