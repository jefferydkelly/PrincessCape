using UnityEngine;
using System.Collections;


public class Controller
{
	private ControllerType controllerType;
	//The basic movement keys 
	private string leftKey;
	private string rightKey;
	private string upKey;
	private string downKey;
	private string jumpKey;

	//The action keys
	private string attackKey;
	private string spellKey;
	private string interactKey;
	private string sneakKey;
	private string nextItemKey;
	private string prevItemKey;
	private string useItemKey;
	private string nextSpellKey;
	private string prevSpellKey;

    //Axis Check Bools
    float resetTime = 0.5f;
    bool spellAxisFrozen = false;

	public Controller()
	{
		if (Input.GetJoystickNames().Length == 0)
		{
			controllerType = ControllerType.Keyboard;
		}
		else {
			string os = SystemInfo.operatingSystem;
			if (os.Contains("Mac"))
			{
				controllerType = ControllerType.GamepadMac;
			}
			else {
				controllerType = ControllerType.GamepadWindows;
			}
		}

        LoadController();

    }

	void LoadController()
	{
		if (IsKeyboard)
		{
			leftKey = "left";
			rightKey = "right";
			upKey = "up";
			downKey = "down";
			jumpKey = "space";
			interactKey = "f";
			attackKey = "c";
			spellKey = "x";
			useItemKey = "z";
			nextSpellKey = "v";
			prevSpellKey = "d";
			sneakKey = "left shift";


		}
		else {
			Controller360 c = new Controller360(controllerType);
			leftKey = c.horizontalAxis;
			upKey = c.verticalAxis;
			interactKey = c.interactKey;
			spellKey = c.spellKey;
			sneakKey = c.sneakKey;
			jumpKey = c.jumpKey;
			prevSpellKey = c.prevSpellKey;
			nextSpellKey = c.nextSpellKey;

		}
	}
	public virtual int Horizontal
	{
		get
		{
			if (IsKeyboard)
			{
				return (Input.GetKey(rightKey) ? 1 : 0) - (Input.GetKey(leftKey) ? 1 : 0);
			}
			float h = Input.GetAxis(leftKey);
			return Mathf.Abs(h) > 0.9 ? (int)Mathf.Sign(h) : 0;

		}
	}

	public virtual int Vertical
	{
		get
		{
			if (IsKeyboard)
			{
				return (Input.GetKey(upKey) ? 1 : 0) - (Input.GetKey(downKey) ? 1 : 0);
			}

			float v = Input.GetAxis(upKey);
			return Mathf.Abs(v) > 0.9 ? (int)Mathf.Sign(v) : 0;

		}
	}

	public bool Attack
	{
		get
		{
			return Input.GetKeyDown(attackKey);
		}
	}

	public bool UseSpell
	{
		get
		{
			return Input.GetKeyDown(spellKey);
		}
	}

	public bool Interact
	{
		get
		{
			return Input.GetKeyDown(interactKey);
		}
	}

	public bool Jump
	{
		get
		{
			return Input.GetKeyDown(jumpKey);
		}
	}

	public bool UseItem
	{
		get
		{
			return Input.GetKeyDown(useItemKey);
		}
	}

	public bool Sneak
	{
		get
		{
			return Input.GetKey(sneakKey);
		}
	}
	public virtual int ItemChange
	{
		get
		{
			return ((Input.GetKeyDown(nextItemKey)) ? 1 : 0) - ((Input.GetKeyDown(prevItemKey)) ? 1 : 0);
		}
	}

	public virtual int SpellChange
	{
		get
		{
			if (controllerType == ControllerType.GamepadWindows)
			{
                float val = Input.GetAxis(nextSpellKey);
                if (Mathf.Abs(val) > 0.5 && !spellAxisFrozen)
                {
                    spellAxisFrozen = true;
                    GameManager.Instance.Player.HandleSpellAxisCooldownForController(resetTime);
                    return (int)Mathf.Sign(val);
                }
                return 0;

			}
			else {
				return ((Input.GetKeyDown(nextSpellKey)) ? 1 : 0) - ((Input.GetKeyDown(prevSpellKey)) ? 1 : 0);
			}
		}
	}

    public void UnfreezeSpellAxis()
    {
        spellAxisFrozen = false;
    }

	public virtual string LeftKey
	{
		get
		{
			return leftKey;
		}
	}

	public virtual string RightKey
	{
		get
		{
			return rightKey;
		}
	}

	public virtual string UpKey
	{
		get
		{
			return upKey;
		}
	}

	public virtual string DownKey
	{
		get
		{
			return downKey;
		}
	}

	public virtual string JumpKey
	{
		get
		{
			return jumpKey;
		}
	}

	public virtual string InteractKey
	{
		get
		{
			return interactKey;
		}
	}

	public virtual string AttackKey
	{
		get
		{
			return attackKey;
		}
	}

	public virtual string UseItemKey
	{
		get
		{
			return useItemKey;
		}
	}

	public virtual string NextItemKey
	{
		get
		{
			return nextItemKey;
		}
	}

	public virtual string PrevItemKey
	{
		get
		{
			return prevItemKey;
		}
	}

	public virtual string NextSpellKey
	{
		get
		{
			return nextSpellKey;
		}
	}

	public virtual string PrevSpellKey
	{
		get
		{
			return prevSpellKey;
		}
	}

	bool IsKeyboard
	{
		get
		{
			return controllerType == ControllerType.Keyboard;
		}
	}

	/*
	public ControllerSetup Setup
	{
		get
		{
			ControllerSetup cs = new ControllerSetup();
			cs.leftKey = leftKey;
			cs.rightKey = rightKey;
			cs.upKey = upKey;
			cs.downKey = downKey;
			cs.jumpKey = jumpKey;
			cs.attackKey = attackKey;
			cs.interactKey = interactKey;
			cs.useItemKey = useItemKey;
			cs.prevItemKey = prevItemKey;
			cs.nextItemKey = nextItemKey;
			cs.nextSpellKey = nextSpellKey;
			cs.prevSpellKey = prevSpellKey;

			return cs;
		}

		set
		{
			leftKey = value.leftKey;
			rightKey = value.rightKey;
			upKey = value.upKey;
			downKey = value.downKey;
			jumpKey = value.jumpKey;
			attackKey = value.attackKey;
			interactKey = value.interactKey;
			useItemKey = value.useItemKey;
			nextItemKey = value.nextItemKey;
			prevItemKey = value.prevItemKey;
			nextSpellKey = value.nextSpellKey;
			prevSpellKey = value.prevSpellKey;

		}
	}*/
}

/*
[System.Serializable]
public class ControllerSetup
{
	public string leftKey;
	public string rightKey;
	public string upKey;
	public string downKey;
	public string jumpKey;

	//The action keys
	public string attackKey;
	public string interactKey;
	public string useItemKey;
	public string nextItemKey;
	public string prevItemKey;
	public string nextSpellKey;
	public string prevSpellKey;
}*/

[System.Flags]
public enum ControllerType
{
	None,
	Keyboard,
	GamepadMac,
	GamepadWindows
}

public struct Controller360
{
	public string horizontalAxis;
	public string verticalAxis;
	public string jumpKey;
	public string sneakKey;
	public string interactKey;
	public string spellKey;
	public string nextSpellKey;
	public string prevSpellKey;

	public Controller360(ControllerType c)
	{
		
		horizontalAxis = "Horizontal";
		verticalAxis = "Vertical";

		if (c == ControllerType.GamepadMac)
		{
			jumpKey = "joystick button 16";
			sneakKey = "joystick button 17";
			interactKey = "joystick button 18";
			spellKey = "joystick button 19";
			nextSpellKey = "joystick button 8";
			prevSpellKey = "joystick button 7";
		}
		else
		{
			jumpKey = "joystick button 0";
			sneakKey = "joystick button 1";
			interactKey = "joystick button 2";
			spellKey = "joystick button 3";
			nextSpellKey = "DPadXWindows";
			prevSpellKey = "DPadXWindows";
		}
	}
}
