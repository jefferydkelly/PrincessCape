using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
/*
 * Singleton manager for the User Interface
 */
public class UIManager : MonoBehaviour
{
	public float fadeOutDelay = 1.0f;
	public float fadeOutTime = 0.5f;

	//The one static instance of UIManager
	private static UIManager instance = null;

	//The text box that shows messages at the top of the screen
	ImageTextCombo messageBox, dialogBox, nameBox, spellBox;

    //The Stealth Meter that displays how much light is currently shining on the player
    StealthMeter stealthMeter;
	//The HP and MP bars
	//public PlayerBarController hpBar, mpBar;
	HPBar hpBar;
	MPBar mpBar;

	//public Image itemLeft, itemCenter, itemRight;
    Text areaNameBox;

	bool revealing = false;
	bool done = false;
	/*
	 * If there isn't an instance of UIManager, set it to this and Reload everything.
	 */
	void Awake()
	{
		if (!instance)
		{
			DontDestroyOnLoad(gameObject);
			instance = this;
			Reload();
		}
		else {
			Destroy(gameObject);
		}
	}

	void Update() {
		if (revealing && done && Input.GetMouseButtonDown(0))
		{
			if (!GameManager.Instance.IsInCutscene)
			{
				HideDialog();
			}
			else {
				NextElement();
			}
		}
	}
	//Resets the objects when the scene changes
	public void Reload()
	{
		if (FindObjectOfType<Canvas>())
		{
			CancelInvoke();
			messageBox = new ImageTextCombo("MessageBox");
			messageBox.Enabled = false;

			nameBox = new ImageTextCombo("NameBox");
			nameBox.Enabled = false;

			hpBar = GameObject.Find("HPBar").GetComponent<HPBar>();

			mpBar = GameObject.Find("MPBar").GetComponent<MPBar>();

			dialogBox = new ImageTextCombo("DialogBox");
			dialogBox.Enabled = false;

			spellBox = new ImageTextCombo("SpellNameBox");
			spellBox.Enabled = false;


			//itemLeft = GameObject.Find("ItemLeft").GetComponent<Image>();
			//itemLeft.enabled = false;
			//itemCenter = GameObject.Find("ItemCenter").GetComponent<Image>();
			//itemCenter.enabled = false;
			//itemRight = GameObject.Find("ItemRight").GetComponent<Image>();
			//itemRight.enabled = false;

			areaNameBox = GameObject.Find("AreaName").GetComponent<Text>();
			areaNameBox.enabled = false;

            stealthMeter = new StealthMeter();
		}
	}

	//Getter for the static instance of UIManager
	public static UIManager Instance
	{
		get
		{
			return instance;
		}
	}

	/*
	 * Reveals the passed in message as dialog
	 * 
	 * msg - The message to be displayed in the dialog box
	 */
	public void RevealDialog(string msg)
	{
		revealing = true;
		dialogBox.Enabled = true;
		StartCoroutine(RevealLetters(msg));

	}

	public void RevealDialog(string msg, string spker)
	{
		revealing = true;
		nameBox.Enabled = true;
		nameBox.Text = spker;
		dialogBox.Enabled = true;
		StartCoroutine(RevealLetters(msg));
	}

	//Hides the dialog box
	public void HideDialog()
	{
		dialogBox.Enabled = false;
		nameBox.Enabled = false;
		revealing = false;
	}

	/*
	 * Reveals the message one letter at a time.
	 * 
	 * msg - The dialog to be revealed
	 */
	private IEnumerator RevealLetters(string msg)
	{
		done = false;
		int lettersRevealed = 0;
		while (lettersRevealed < msg.Length)
		{
			yield return new WaitForSeconds(0.05f);
			lettersRevealed++;
			dialogBox.Text = msg.Substring(0, lettersRevealed);
		}
		done = true;
	}

	void NextElement()
	{
		revealing = false;
		GameManager.Instance.Cutscene.NextElement();
	}
	/*
	 * Shows the given string as a message in the upper box
	 * 
	 * msg - The message to be displayed
	 */
	public void ShowMessage(string msg)
	{
		messageBox.Enabled = true;
		messageBox.Text = msg;
	}

	/*
	 * Shows the given string as a message in the upper box for a set amount of time.
	 * 
	 * msg - The message to be displayed
	 * time - The amount of time the message will be displayed
	 */
	public void ShowMessage(string msg, float time)
	{
		messageBox.Enabled = true;
		messageBox.Text = msg;
		Invoke("HideMessage", time);
	}

	//Hides the message in the message box
	public void HideMessage()
	{
		messageBox.Enabled = false;
	}

	public bool InCutscene
	{
		set
		{
			bool val = !value;
			ShowSpell = val;
			hpBar.enabled = val;
			mpBar.enabled = val;
            stealthMeter.Enabled = val;
		}
	}
	//Shows/Hides the equipment box 
	public bool ShowSpell
	{
		get
		{
			return spellBox.Enabled;
		}

		set
		{
			spellBox.Enabled = value;
			UpdateSpellInfo();
		}
	}

	public void UpdateSpellInfo()
	{
		spellBox.Text = GameManager.Instance.Player.SpellName;
	}

	public void UpdateSpellName(string spellName)
	{
		spellBox.Text = spellName;
	}

	public void EnterArea(string s)
	{
		if (s != areaNameBox.text)
		{
			areaNameBox.enabled = true;
			areaNameBox.text = s;
			Invoke("StartFadeOut", fadeOutDelay);
		}
	}

    public float LightLevel
    {
        get
        {
            return stealthMeter.LightLevel;
        }

        set
        {
            stealthMeter.LightLevel = value;
        }
    }

	void StartFadeOut()
	{
		areaNameBox.CrossFadeAlpha(0, fadeOutTime, false);
		Invoke("HideAreaName", fadeOutTime);
	}
	void HideAreaName()
	{
		areaNameBox.enabled = false;
		Color ac = areaNameBox.color;
		ac.a = 1;
		areaNameBox.color = ac;
	}
}

/*
 *  A struct built to hold a combination of a background image and a text box in front of it
 */
public struct ImageTextCombo
{
	//The background image
	public Image bg;
	public Image img;
	//The text box
	public Text txt;
	//Whether or not the textbox is enabled
	private bool enabled;

	public ImageTextCombo(string s)
	{
        bg = GameObject.Find(s).GetComponent<Image>();

		img = bg.GetComponentsInChildren<Image>()[1];
		txt = img.GetComponentInChildren<Text>();
		txt.text = "";
		enabled = true;
	}
	//A getter and setter for (dis)abling the textbox and checking that status
	public bool Enabled
	{
		set
		{
			enabled = value;
			img.enabled = value;
			bg.enabled = value;
			txt.enabled = value;
		}

		get
		{
			return enabled;
		}
	}

	//Getter and setter for the text in the checkbox
	public string Text
	{
		set
		{
			txt.text = value;
		}

		get
		{
			return txt.text;
		}
	}
}
