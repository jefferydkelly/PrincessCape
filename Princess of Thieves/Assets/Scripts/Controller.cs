using UnityEngine;
using System.Collections;


public class Controller
{
    ControllerType controllerType;
    //The basic movement keys 
   	string leftKey;
    string rightKey;
    string upKey;
    string downKey;
    string jumpKey;

    //The action keys
    string attackKey;
    string itemKeyOne;
    string interactKey;
    string sneakKey;

	//Menu Keys
	string pauseKey;

    //Axis Check Bools
    float resetTime = 0.5f;
    bool spellAxisFrozen = false;

    public Controller()
    {
        if (Input.GetJoystickNames().Length == 0)
        {
            controllerType = ControllerType.Keyboard;
        }
        else
        {
            string os = SystemInfo.operatingSystem;
            if (os.Contains("Mac"))
            {
                controllerType = ControllerType.GamepadMac;
            }
            else
            {
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
            itemKeyOne = "x";
            sneakKey = "left shift";
			pauseKey = "p";


        }
        else
        {
            Controller360 c = new Controller360(controllerType);
            leftKey = c.horizontalAxis;
            upKey = c.verticalAxis;
            interactKey = c.interactKey;
            sneakKey = c.sneakKey;
            jumpKey = c.jumpKey;

        }
    }
    public int Horizontal
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

    public int Vertical
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

    public Vector2 InputDirection
    {
        get
        {
            return new Vector2(Horizontal, Vertical);
        }
    }

    public bool Attack
    {
        get
        {
            return Input.GetKeyDown(attackKey);
        }
    }

    public bool ActivateItem
    {
        get
        {
            return Input.GetKeyDown(itemKeyOne);
        }
    }

    public bool DeactivateItem
    {
        get
        {
            return Input.GetKeyUp(itemKeyOne);
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

    public bool Sneak
    {
        get
        {
            return Input.GetKey(sneakKey);
        }
    }

    public string LeftKey
    {
        get
        {
            return leftKey;
        }
    }

    public string RightKey
    {
        get
        {
            return rightKey;
        }
    }

    public string UpKey
    {
        get
        {
            return upKey;
        }
    }

    public string DownKey
    {
        get
        {
            return downKey;
        }
    }

    public string JumpKey
    {
        get
        {
            return jumpKey;
        }
    }

    public string InteractKey
    {
        get
        {
            return interactKey;
        }
    }

    public string AttackKey
    {
        get
        {
            return attackKey;
        }
    }

	public bool Pause
	{
		get
		{
            return Input.GetKeyDown(pauseKey);
		}
	}
    bool IsKeyboard
    {
        get
        {
            return controllerType == ControllerType.Keyboard;
        }
    }

    public string MovementInfo
    {
        get
        {
            string controls = "";
            if (controllerType == ControllerType.Keyboard)
            {
                controls += "Left: " + leftKey + "\n";
                controls += "Right: " + rightKey + "\n";
                controls += "Up: " + upKey + "\n";
                controls += "Down: " + downKey + "\n";
                controls += "Sneak: " + sneakKey + "\n";
                controls += "Jump: " + jumpKey + "\n";
            }
            return controls;
        }
    }

    public string ActionInfo
    {
        get
        {
            string controls = "";
            if (controllerType == ControllerType.Keyboard)
            {
                controls += "Proceed: " + jumpKey + "\n";
                controls += "Interact: " + interactKey + "\n";
                controls += "use Item: " + itemKeyOne + "\n";
            }
            return controls;
        }
    }
}

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
