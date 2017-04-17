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
    string leftItemKey;
    string rightItemKey;
    string interactKey;

	//Menu Keys
	string pauseKey;
	string submitKey;

    string restartKey;
    string resetKey;

	Controller360 gamepad;

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
            leftKey = "a";
            rightKey = "d";
            upKey = "w";
            downKey = "s";
            jumpKey = "space";
            interactKey = "f";
            rightItemKey = "mouse 1";
			leftItemKey = "mouse 0";
			pauseKey = "p";
			submitKey = "enter";
            resetKey = "escape";
            restartKey = "1";

        }
        else
        {
			gamepad = new Controller360(controllerType);
			leftKey = gamepad.horizontalAxis;
			rightKey = gamepad.horizontalAxis;
			upKey = gamepad.verticalAxis;
			downKey = gamepad.verticalAxis;
			interactKey = gamepad.interactKey;
			jumpKey = gamepad.jumpKey;
			submitKey = gamepad.jumpKey;
			leftItemKey = gamepad.leftItemKey;
			rightItemKey = gamepad.rightItemKey;
			pauseKey = gamepad.pauseKey;
			resetKey = gamepad.resetKey;

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
    public bool Reset
    {
        get
        {
            return Input.GetKeyDown(resetKey);
        }
    }
    public bool Restart
    {
        get
        {
            return Input.GetKeyDown(restartKey);
        }
    }
    public bool Jump
    {
        get
        {
			return Input.GetKeyDown(jumpKey) || Input.GetKeyDown(upKey);
        }
    }

	public bool AltJump {
		get {
			return Input.GetKeyDown (jumpKey);
		}
	}

	public bool Submit {
		get {


			return Input.GetKey (submitKey);
		}
	}

	string Translate(string key, bool leftOrUp = false) {
		if (IsKeyboard) {
			if (key.StartsWith ("mouse")) {
				string mouseKey = "";
				if (key.EndsWith ("0")) {
					mouseKey = "LMB";
				} else if (key.EndsWith ("1")) {
					mouseKey = "RMB";
				} else if (key.EndsWith ("2")) {
					mouseKey = "MMB";
				}
				return mouseKey;
			} else if (key.Length > 1) {
				return key.Substring (0, 1).ToUpper () + key.Substring (1);
			} else {
				return key.ToUpper ();
			}
		} else {
			return gamepad.Translate (key, leftOrUp);
		}
	}

    public string LeftKey
    {
        get
        {
			return Translate(leftKey, true);
        }
    }

    public string RightKey
    {
        get
        {
			return Translate(rightKey, false);
        }
    }

    public string UpKey
    {
        get
        {
			return Translate(upKey, true);
        }
    }

    public string DownKey
    {
        get
        {
			return Translate(downKey, false);
        }
    }

    public string JumpKey
    {
        get
        {
			return Translate(jumpKey);
        }
    }

    public string InteractKey
    {
        get
        {
			return Translate(interactKey, false);
        }
    }

    public string LeftItemKey
    {
        get
        {
			return Translate(leftItemKey, false);
        }
    }

    public string RightItemKey
    {
        get
        {
			return Translate(rightItemKey);
        }
    }

	public string PauseKey {
		get {
			return Translate (pauseKey);
		}
	}

	public string ResetKey {
		get {
			return Translate (restartKey);
		}
	}

	public string AimKeys {
		get {
			return IsKeyboard ? "WASD Keys" : "Left Stick";
		}
	}
	public bool Pause
	{
		get
		{
			return Input.GetKeyDown (pauseKey);
		}
	}

    public bool IsKeyboard
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
            //if (controllerType == ControllerType.Keyboard)
            //{
                controls += "Left: " + LeftKey + "\n";
                controls += "Right: " + RightKey + "\n";
                controls += "Up: " + UpKey + "\n";
                controls += "Down: " + DownKey + "\n";
            //}
            return controls;
        }
    }

    public string ActionInfo
    {
        get
        {
            string controls = "Actions\n";
            //if (controllerType == ControllerType.Keyboard)
           // {
                controls += "Jump: " + JumpKey + "\n";
                controls += "Interact: " + InteractKey + "\n";
				controls += "Left Item: " + Translate(leftItemKey, true) + "\n";
				controls += "Right Item: " + Translate(rightItemKey, true) + "\n";
                controls += "Pause/Inventory: " + PauseKey + "\n";
                controls += "Reset: " + ResetKey + "\n";
            //}
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
	ControllerType type;
	public string horizontalAxis;
	public string verticalAxis;
	public string jumpKey;
	public string interactKey;
	public string leftItemKey;
	public string rightItemKey;
	public string pauseKey;
	public string resetKey;

	public Controller360(ControllerType c)
	{
		
		horizontalAxis = "Horizontal";
		verticalAxis = "Vertical";
		type = c;
		if (c == ControllerType.GamepadMac)
		{
			jumpKey = "joystick button 16";
			interactKey = "joystick button 18";
			leftItemKey = "joystick button 19";
			rightItemKey = "joystick button 17";
			pauseKey = "joystick button 9";
			resetKey = "joystick button 10";
		}
		else
		{
			jumpKey = "joystick button 0";
			interactKey = "joystick button 2";
			leftItemKey = "joystick button 3";
			rightItemKey = "joystick button 1";
			pauseKey = "joystick button 7";
			resetKey = "joystick button 6";
		}
	}

	public string Translate(string s, bool upLeft = false) {
		if (s == "Horizontal") {
			return "L. Joystick " + (upLeft ? "Left" : "Right");
		} else if (s == "Vertical") {
			return "L. Joystick " + (upLeft ? "Up" : "Down");
		} else if (s == "DPadYWindows") {
			return upLeft ? "D-Pad Up" : "D-Pad Down";
		} else if (s == "DPadXWindows") { 
			return upLeft ? "D-Pad Left" : "D-Pad Right";
		} else {
			string[] subs = s.Split (new char[]{' '});
			int i = int.Parse(subs [subs.Length - 1]);

			switch (i) {
			case 0:
				return "A";
				
			case 1:
				return "B";
				
			case 2:
				return "X";
				
			case 3:
				return "Y";
				
			case 4:
				return "LB";
				
			case 5:
				return type == ControllerType.GamepadMac ? "D-Pad Up" : "RB";
				
			case 6:
				return type == ControllerType.GamepadMac ? "D-Pad Down" :"Back";
				
			case 7:
				return type == ControllerType.GamepadMac ? "D-Pad Left" :"Start";
				
			case 8:
				return type == ControllerType.GamepadMac ? "D-Pad Right" :"Left Click";
				
			case 9:
				return type == ControllerType.GamepadMac ? "Start" :"Right Click";
				
			case 10:
				return "Back";
				
			case 11:
				return "Left Click";
				
			case 12:
				return "Right Click";
				
			case 13:
				return "LB";
				
			case 14:
				return "RB";
				
			case 15:
				return "Xbox";
				
			case 16:
				return "A";
				
			case 17:
				return "B";
				
			case 18:
				return "X";
				
			case 19:
				return "Y";
				
			}
		}
		return "";
	}
}
