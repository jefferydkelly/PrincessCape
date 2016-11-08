﻿using UnityEngine;
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

            Spell s = new WaterSpell() ;
			switch (spellType)
			{
				case SpellType.Earth:
					s = new EarthSpell();
					break;
				case SpellType.Fire:
                    s = new FireSpell();
					break;
				case SpellType.Wind:
                    s = new WindSpell();
					break;
				case SpellType.Water:
                    s = new WaterSpell();
					break;		
				case SpellType.Light:
                    s = new LightSpell();
					break;
				case SpellType.Dark:
                    s = new DarkSpell();
					break;
			}

            UIManager.Instance.ShowMessage("You learned " + s.SpellName, 3);
            GameManager.Instance.Player.AddSpell(s);

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
