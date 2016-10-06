using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 * Singleton manager for the User Interface
 */
public class UIManager : MonoBehaviour
{
	public float fadeOutDelay = 1.0f;
	public float fadeOutTime = 0.5f;
	private string areaName = "";
	//The one static instance of UIManager
	private static UIManager instance = null;

	//The text box that shows messages at the top of the screen
	ImageTextCombo messageBox, dialogBox, nameBox, spellBox;

	//The HP and MP bars
	//public PlayerBarController hpBar, mpBar;
	HPBar hpBar;
	MPBar mpBar;

	//public Image itemLeft, itemCenter, itemRight;

	//public Text areaNameBox;
	/*
	 * If there isn't an instance of UIManager, set it to this and Reload everything.
	 */
	void Awake()
	{
		if (!instance)
		{
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
		else {
			Destroy(gameObject);
		}

		if (GameManager.Instance != null)
		{
			Reload();
		}
	}

	//Resets the objects when the scene changes
	public void Reload()
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

		//areaNameBox = GameObject.Find("AreaName").GetComponent<Text>();
		//areaNameBox.enabled = false;
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
		dialogBox.Enabled = true;
		StartCoroutine(RevealLetters(msg));

	}

	public void RevealDialog(string msg, string spker)
	{
		nameBox.Enabled = true;
		nameBox.Text = spker;
		dialogBox.Enabled = true;
		StartCoroutine(RevealLetters(msg));
	}

	//Hides the dialog box
	public void HideDialog()
	{
		dialogBox.Enabled = false;
		//nameBox.Enabled = false;
	}

	/*
	 * Reveals the message one letter at a time.
	 * 
	 * msg - The dialog to be revealed
	 */
	private IEnumerator RevealLetters(string msg)
	{
		int lettersRevealed = 0;
		while (lettersRevealed < msg.Length)
		{
			yield return new WaitForSeconds(0.1f);
			lettersRevealed++;
			dialogBox.Text = msg.Substring(0, lettersRevealed);
		}

		if (!GameManager.Instance.IsInCutscene)
		{
			Invoke("HideDialog", 1.0f);
		}
		else {
			Invoke("NextElement", 1.0f);
		}
	}

	void NextElement()
	{
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

	/*
	public void SetVisibleItems(Inventory playerInv)
	{
		if (playerInv.curItemSelected.itemIcon)
		{
			itemCenter.enabled = true;
			itemCenter.sprite = playerInv.curItemSelected.itemIcon;
		}
		else
		{
			itemCenter.GetComponent<Image>().enabled = false;
		}
		if (playerInv.LeftItem)
		{
			itemLeft.enabled = true;
			itemLeft.sprite = playerInv.LeftItem;
		}
		else
		{
			itemLeft.GetComponent<Image>().enabled = false;
		}

		if (playerInv.RightItem)
		{
			itemRight.enabled = true;
			itemRight.sprite = playerInv.RightItem;
		}
		else
		{
			itemRight.GetComponent<Image>().enabled = false;
		}
	}*/

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
			spellBox.Text = GameManager.Instance.Player.SpellName;
		}
	}

	public void UpdateSpellName(string spellName)
	{
		spellBox.Text = spellName;
	}

	/*
	public void EnableItemBox(ItemBox ib, bool enabled)
	{
		Image img = itemCenter;

		if (ib == ItemBox.Left)
		{
			img = itemLeft;
		}
		else if (ib == ItemBox.Right)
		{
			img = itemRight;
		}

		img.enabled = enabled;
	}

	public void SetItemBoxSprite(ItemBox ib, Sprite sp)
	{
		Image img = itemCenter;

		if (ib == ItemBox.Left)
		{
			img = itemLeft;
		}
		else if (ib == ItemBox.Right)
		{
			img = itemRight;
		}

		img.sprite = sp;
	}

	public void EnterArea(string s)
	{
		if (s != areaName)
		{
			areaName = s;
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
	}*/
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

public enum ItemBox
{
	Left,
	Center,
	Right
}
