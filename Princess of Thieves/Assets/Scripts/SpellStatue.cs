using UnityEngine;
using System.Collections;

public class SpellStatue : MonoBehaviour, InteractiveObject {

	bool activated = false;
	public SpellType spellType;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Interact()
	{
		if (!activated)
		{
			activated = true;
			Player p = GameManager.Instance.Player;
			switch (spellType)
			{
				case SpellType.Earth:
					p.AddSpell(new EarthSpell());
					break;
				case SpellType.Fire:
					p.AddSpell(new FireSpell());
					break;
				case SpellType.Wind:
					p.AddSpell(new WindSpell());
					break;
				case SpellType.Water:
					p.AddSpell(new WaterSpell());
					break;		
				case SpellType.Light:
					p.AddSpell(new LightSpell());
					break;
				case SpellType.Dark:
					p.AddSpell(new DarkSpell());
					break;
			}

		}
	}
}

public enum SpellType
{
	Earth,
	Fire,
	Wind,
	Water,
	Light,
	Dark
}
