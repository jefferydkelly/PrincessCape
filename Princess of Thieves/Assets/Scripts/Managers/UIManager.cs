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
	ImageTextCombo messageBox, dialogBox, nameBox;

    //The Stealth Meter that displays how much light is currently shining on the player
    StealthMeter stealthMeter;
	//The HP and MP bars
	//public PlayerBarController hpBar, mpBar;

    GameObject itemManager;

	//public Image itemLeft, itemCenter, itemRight;
    Text areaNameBox;

	bool revealing = false;
	bool done = false;

	ItemBox rightBox;
    ComboBox leftBox;
    InteractionBox interactionBox;

    GameObject inventoryMenu;

	bool leftAligned = true;
	int lettersPerLine = 69;

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
		if (revealing && done && Input.anyKeyDown)
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

			dialogBox = new ImageTextCombo("DialogBox");
			dialogBox.Enabled = false;

			areaNameBox = GameObject.Find("AreaName").GetComponent<Text>();
			areaNameBox.enabled = false;

            leftBox = new ComboBox("LeftBox");
			leftBox.Enabled = true;
            rightBox = new ItemBox("RightBox");
			rightBox.Enabled = true;
            interactionBox = new InteractionBox("InteractBox");
			interactionBox.Enabled = false;
            
          
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

	public bool IsShowingInteraction {
		get {
			return leftBox.IsShowingInteraction;
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
		nameBox.Enabled = false;
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
		if (!leftAligned) {
			string opening = "";
			for (int i = msg.Length; i < lettersPerLine; i++) {
				opening += " ";
			}
			lettersRevealed += opening.Length;
			msg = opening + msg;
		}
		//interactionBox.Interaction = "Skip";
		while (lettersRevealed < msg.Length)
		{
			if (Input.anyKeyDown) {
				lettersRevealed = msg.Length;
				dialogBox.Text = msg.Substring (0, lettersRevealed);
			} else {
				yield return null;
				lettersRevealed++;
				dialogBox.Text = msg.Substring (0, lettersRevealed);
			}
		}
		done = true;
		//interactionBox.Interaction = "Next";

	}

    private IEnumerator RevealMessage(string msg)
    {
		yield return null;
        done = false;
        int lettersRevealed = 0;
        //interactionBox.Interaction = "";
        while (lettersRevealed < msg.Length)
        {
			if (GameManager.Instance.Player.Controller.Interact) {
				dialogBox.Text = msg;
				break;
			} else {
				yield return new WaitForSeconds (0.01f);
				lettersRevealed++;
				dialogBox.Text = msg.Substring (0, lettersRevealed);
			}
        }
		yield return null;
        //interactionBox.Interaction = "Next";
        while (!GameManager.Instance.Player.Controller.Interact)
        {
            
            yield return null;
        }
        //interactionBox.Interaction = "";
        done = true;
    }

	IEnumerator NextElement()
	{
		//interactionBox.Interaction = "";
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
        //bars.enabled = false;
        //stealthMeter.Enabled = false;

        messageBox.Enabled = true;
		messageBox.Text = msg;
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
		for (int i = 0; i < msg.Length; i++) {
			msg [i] = msg [i].Replace ("\\n", "\n");
		}
        GameManager.Instance.IsInCutscene = true;

        dialogBox.Enabled = true;
		//bars.enabled = true;
        foreach (string s in msg)
        {
            yield return StartCoroutine(RevealMessage(s)); 
        }
        dialogBox.Enabled = false;
        HideInteraction();
        GameManager.Instance.IsInCutscene = false;
    }

	//Hides the message in the message box
	public void HideMessage()
	{
		//bars.enabled = true;
        //stealthMeter.Enabled = true;
        messageBox.Enabled = false;
        dialogBox.Enabled = false;
        HideInteraction();
        GameManager.Instance.IsInCutscene = false;
    }

    public void HideDialogBox()
    {
        dialogBox.Enabled = false;
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
        leftBox.Key = c.LeftItemKey;
        if (p.RightItem != null)
        {
            rightBox.ItemSprite = p.RightItem;
        }
        rightBox.Key = c.RightItemKey;
        //interactionBox.Key = c.InteractKey.ToUpper();
        inventoryMenu.GetComponent<InventoryMenu>().UpdateUI();
    }

    public void ShowInteraction(string s)
    {
		if (s.Length > 0) {
			leftBox.ShowInteraction (s);
		} else {
			leftBox.HideInteraction ();
		}
		/*
        interactionBox.Enabled = true;
        interactionBox.Interaction = s;
        */
    }

	public void HideInteraction() {
		leftBox.HideInteraction ();
	}

	public ItemBox LeftItemBox {
		get {
			return leftBox;
		}
	}

	public ItemBox RightItemBox {
		get {
			return rightBox;
		}
	}

	public bool ShowBoxes {
		get {
			return LeftItemBox.Enabled;
		}

		set {
			rightBox.Enabled = value;
			leftBox.Enabled = value;
			//interactionBox.Enabled = value;
		}
	}

	public bool LeftAligned {
		set {
			leftAligned = value;
			if (value) {
				//dialogBox.txt.alignment = TextAnchor.UpperLeft;
				nameBox.bg.rectTransform.anchoredPosition = new Vector2 (-1190, -600);
			} else {
				nameBox.bg.rectTransform.anchoredPosition= new Vector2(1190, -600);
				//dialogBox.txt.alignment = TextAnchor.UpperRight;
			}
		}
	}

	public bool Italicized {
		get {
			return (dialogBox.txt.fontStyle & FontStyle.Italic) > 0;
		}

		set {
			if (value) {
				dialogBox.txt.fontStyle |= FontStyle.Italic;
			} else {
				dialogBox.txt.fontStyle &= ~FontStyle.Italic;
			}
		}
	}

	public bool Bolded {
		get {
			return (dialogBox.txt.fontStyle & FontStyle.Bold) > 0;
		}

		set {
			if (value) {
				dialogBox.txt.fontStyle |= FontStyle.Bold;
			} else {
				dialogBox.txt.fontStyle &= ~FontStyle.Bold;
			}
		}
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