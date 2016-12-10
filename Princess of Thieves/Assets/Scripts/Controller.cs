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
    string leftItemKey;
    string rightItemKey;
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
            interactKey = "z";
            rightItemKey = "c";
            leftItemKey = "x";
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

    public bool ActivateLeftItem
    {
        get
        {
            return Input.GetKeyDown(leftItemKey);
        }
    }

    public bool LeftItemDown
    {
        get
        {
            return Input.GetKey(leftItemKey);
        }
    }
    public bool DeactivateLeftItem
    {
        get
        {
            return Input.GetKeyUp(leftItemKey);
        }
    }

    public bool ActivateRightItem
    {
        get
        {
            return Input.GetKeyDown(rightItemKey);
        }
    }

    public bool RightItemDown
    {
        get
        {
            return Input.GetKey(rightItemKey);
        }
    }

    public bool DeactivateRightItem
    {
        get
        {
            return Input.GetKeyUp(rightItemKey);
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

    public string LeftItemKey
    {
        get
        {
            return leftItemKey;
        }
    }

    public string RightItemKey
    {
        get
        {
            return rightItemKey;
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
            string controls = "Movement\n";
            if (controllerType == ControllerType.Keyboard)
            {
                controls += "Left: " + leftKey + "\n";
                controls += "Right: " + rightKey + "\n";
                controls += "Up: " + upKey + "\n";
                controls += "Down: " + downKey + "\n";
                
            }
            return controls;
        }
    }

    public string ActionInfo
    {
        get
        {
            string controls = "Actions\n";
            if (controllerType == ControllerType.Keyboard)
            {
                controls += "Jump: " + jumpKey + "\n";
                controls += "Interact: " + interactKey + "\n";
                controls += "Left Item: " + leftItemKey + "\n";
                controls += "Right Item: " + rightItemKey + "\n";
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
