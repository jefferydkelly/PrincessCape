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

    GameObject itemManager;

	//public Image itemLeft, itemCenter, itemRight;
    Text areaNameBox;

	bool revealing = false;
	bool done = false;

    ItemBox rightBox;
    ItemBox leftBox;
    InteractionBox interactionBox;

    GameObject inventoryMenu;
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
		if (revealing && done && GameManager.Instance.Player.Controller.Jump)
		{
			StartCoroutine(NextElement());
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

			areaNameBox = GameObject.Find("AreaName").GetComponent<Text>();
			areaNameBox.enabled = false;

            leftBox = new ItemBox("LeftBox");
            rightBox = new ItemBox("RightBox");
            interactionBox = new InteractionBox("InteractBox");
            
          
            inventoryMenu = GameObject.Find("InventoryMenu");
            inventoryMenu.SetActive(false);
            //stealthMeter = new StealthMeter();
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
	public IEnumerator HideDialog()
	{
		yield return new WaitForEndOfFrame();
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

    private IEnumerator RevealMessage(string msg)
    {
        done = false;
        int lettersRevealed = 0;
        interactionBox.Interaction = "";
        while (lettersRevealed < msg.Length)
        {
            yield return new WaitForSeconds(0.02f);
            lettersRevealed++;
            dialogBox.Text = msg.Substring(0, lettersRevealed);
        }
        interactionBox.Interaction = "Next";
        while (!GameManager.Instance.Player.Controller.Interact)
        {
            
            yield return null;
        }
        interactionBox.Interaction = "";
        done = true;
    }

	IEnumerator NextElement()
	{
		yield return new WaitForEndOfFrame();
		revealing = false;
		GameManager.Instance.Cutscene.NextElement();
	}

	void Proceed()
	{
		GameManager.Instance.Cutscene.NextElement();
	}
	public void WaitFor(float time)
	{
		Invoke("Proceed", time);
	}

    public bool ShowMenu
    {
        get
        {
            return inventoryMenu.activeSelf;
        }
        
        set
        {
            //Set the inventory's visibility to the game's pause state
            inventoryMenu.SetActive(value);
        }
        
    }
	/*
	 * Shows the given string as a message in the upper box
	 * 
	 * msg - The message to be displayed
	 */
	public void ShowMessage(string msg)
	{
        hpBar.enabled = false;
        mpBar.enabled = false;
        //stealthMeter.Enabled = false;

        messageBox.Enabled = true;
        messageBox.Text = msg;
        
        ShowInteraction("Close");
	}

    public void ShowDialog(string msg)
    {
        dialogBox.Enabled = true;
        dialogBox.Text = msg;
      
        ShowInteraction("Close");
    }


    /*
	 * Shows the given string as a message in the upper box for a set amount of time.
	 * 
	 * msg - The message to be displayed
	 * time - The amount of time the message will be displayed
	 */
    public void ShowMessage(string msg, float time)
	{   
        GameManager.Instance.IsInCutscene = true;
        //stealthMeter.Enabled = false;
       
        messageBox.Enabled = true;
        messageBox.Text = msg;
        
        
		Invoke("HideMessage", time);
	}

    public IEnumerator ShowFoundItemMessage(string[] msg)
    {
        GameManager.Instance.IsInCutscene = true;
        dialogBox.Enabled = true;
        hpBar.enabled = true;
        mpBar.enabled = true;
        foreach (string s in msg)
        {
            yield return StartCoroutine(RevealMessage(s)); 
        }
        dialogBox.Enabled = false;
        GameManager.Instance.IsInCutscene = false;
    }

	//Hides the message in the message box
	public void HideMessage()
	{
        hpBar.enabled = true;
        mpBar.enabled = true;
        //stealthMeter.Enabled = true;
        messageBox.Enabled = false;
        dialogBox.Enabled = false;
        GameManager.Instance.IsInCutscene = false;
    }

    public void HideDialogBox()
    {
        dialogBox.Enabled = false;
    }

	public bool InCutscene
	{
		set
		{
			bool val = !value;
			hpBar.enabled = val;
			mpBar.enabled = val;
            //stealthMeter.Enabled = val;
		}
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
    /*
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
    }*/

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

    public void UpdateUI()
    {
        Player p = GameManager.Instance.Player;
        
        leftBox.ItemSprite = p.LeftItem;
        rightBox.ItemSprite = p.RightItem;
        inventoryMenu.GetComponent<InventoryMenu>().UpdateUI();
    }

    public void UpdateUI(Controller c)
    {
        Player p = GameManager.Instance.Player;
        if (p.LeftItem != null)
        {
            leftBox.ItemSprite = p.LeftItem;
        }
        leftBox.Key = c.LeftItemKey.ToUpper();
        if (p.RightItem != null)
        {
            rightBox.ItemSprite = p.RightItem;
        }
        rightBox.Key = c.RightItemKey.ToUpper();
        interactionBox.Key = c.InteractKey.ToUpper();
        inventoryMenu.GetComponent<InventoryMenu>().UpdateUI();
    }

    public void ShowInteraction(string s)
    {
        interactionBox.Enabled = true;
        interactionBox.Interaction = s;
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

public struct ItemBox
{
    GameObject background;
    Image itemRenderer;
    Text keyText;

    public ItemBox(string s)
    {
        background = GameObject.Find(s);
        itemRenderer = background.GetComponentsInChildren<Image>()[1];
        keyText = background.GetComponentInChildren<Text>();
    }

    public Sprite ItemSprite
    {
        get
        {
            return itemRenderer.sprite;
        }

        set
        {
            itemRenderer.sprite = value;
        }
    }

    public bool Enabled
    {
        get
        {
            return background.activeSelf;
        }

        set
        {
            background.SetActive(value);
        }
    }

    public string Key
    {
        get
        {
            return keyText.text;
        }

        set
        {
            keyText.text = value;
        }
    }
}

public struct InteractionBox
{
    GameObject background;
    Text interactionText;
    Text keyText;

    public InteractionBox(string s)
    {
        background = GameObject.Find(s);
        Text[] texts = background.GetComponentsInChildren<Text>();
        keyText = texts[0];
        interactionText = texts[1];
    }
    public bool Enabled
    {
        get
        {
            return background.activeSelf;
        }

        set
        {
            //12/31 throwing errors in 5.5. When moving towards the item chest, it doesn't seem to have an object reference.
            background.SetActive(value);
        }
    }

    public string Interaction
    {
        get
        {
            return interactionText.text;
        }

        set
        {
            interactionText.text = value;
        }
    }
    public string Key
    {
        get
        {
            return keyText.text;
        }

        set
        {
            keyText.text = value;
        }
    }
}