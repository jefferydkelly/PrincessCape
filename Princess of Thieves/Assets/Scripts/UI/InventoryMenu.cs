using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
public class InventoryMenu : MonoBehaviour {

    
    int curSelected = 0;
    [SerializeField]
    float waitTime = 0.25f;
	List<InventoryItem> childImages;

    bool leftItemDown = false;
    bool rightItemDown = false;
    bool interactDown = false;
    bool showingInfo = false;
    private void OnEnable()
    {
		childImages = GetComponentsInChildren<InventoryItem>().ToList();
        UpdateUI();
        if (UIManager.Instance) { 
            UIManager.Instance.ShowInteraction("Info");
			/*
            InvokeRepeating("HandleInput", waitTime, waitTime);

            leftItemDown = false;
            rightItemDown = false;
            interactDown = false;
            showingInfo = false;
            */
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
        UIManager.Instance.HideMessage();
    }

    private void Update()
    {
        if (GameManager.Instance.IsPaused)
        {
			/*
            if (controller.ActivateLeftItem)
            {
                leftItemDown = true;
                rightItemDown = false;
                interactDown = false;
            }
            else if (controller.DeactivateLeftItem)
            {
                leftItemDown = false;
            }

            if (controller.ActivateRightItem)
            {
                rightItemDown = true;
                leftItemDown = false;
                interactDown = false;
            }
            else if (controller.DeactivateRightItem)
            {
                rightItemDown = false;
            }

            if (controller.Interact)
            {
                interactDown = true;
                leftItemDown = false;
                rightItemDown = false;
            }*/
        }
    }
    private void HandleInput()
    {
        Image curImg = GetComponentsInChildren<Image>()[curSelected + 1];
        curImg.color = Color.white;
        curSelected += controller.Horizontal + controller.Vertical * 2;
        curSelected += 4;
        curSelected %= 4;
        UsableItem item = null;
        if (curSelected < GameManager.Instance.Player.Inventory.Count)
        {
                item = GameManager.Instance.Player.Inventory[curSelected];
            if (item)
            {
                UIManager.Instance.ShowMessage(item.itemName);
            }
        }

		curImg = childImages[curSelected + 1].Image;
        curImg.color = Color.blue;

        if (leftItemDown)
        {
            GameManager.Instance.Player.EquipItem(curSelected, true);
            leftItemDown = false;
        }
        else if (rightItemDown)
        {
            GameManager.Instance.Player.EquipItem(curSelected, false);
            rightItemDown = false;
        } else if (interactDown)
        {
            if (!showingInfo)
            {
                    
                if (item)
                {
                    showingInfo = true;
                    UIManager.Instance.ShowDialog(item.info);
                }
            } else
            {
                showingInfo = false;
                UIManager.Instance.HideDialogBox();
            }
            interactDown = false;
        }

    }

    public void UpdateUI()
    {
        List<UsableItem> items = GameManager.Instance.Player.Inventory;
        if (items != null)
        {
            for (int i = 0; i < items.Count; i++)
            {
                UsableItem curItem = items[i];
                childImages[i].Sprite = curItem ? curItem.uiSprite : null;
            }
        }
    }

	public void EquipItem(InventoryItem i, bool left) {
		GameManager.Instance.Player.EquipItem(childImages.IndexOf (i), left);
	}

	private Controller controller {
		get {
			return GameManager.Instance.Player.Controller;
		}
	}
}
