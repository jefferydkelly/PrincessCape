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
	private string interactKey;
	private string nextItemKey;
	private string prevItemKey;
	private string useItemKey;
	private string nextSpellKey;
	private string prevSpellKey;

	public Controller()
	{
		controllerType = (Input.GetJoystickNames().Length > 0) ? ControllerType.Gamepad : ControllerType.Keyboard;
	}

	void LoadController()
	{
		if (IsKeyboard)
		{
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

			return (int)Mathf.Sign(Input.GetAxis(leftKey));

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

			return (int)Mathf.Sign(Input.GetAxis(upKey));

		}
	}

	public bool Attack
	{
		get
		{
			return Input.GetKeyDown(attackKey);
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
			return ((Input.GetKeyDown(nextSpellKey)) ? 1 : 0) - ((Input.GetKeyDown(prevSpellKey)) ? 1 : 0);
		}
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
			return (controllerType & ControllerType.Keyboard) > 0;
		}
	}
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
	}
}

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
}

[System.Flags]
public enum ControllerType
{
	None,
	Keyboard,
	Gamepad
}
