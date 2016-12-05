using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class InventoryMenu : MonoBehaviour {

    Controller controller;
    int curSelected = 0;
    [SerializeField]
    float waitTime = 0.25f;
    Image[] childImages;

    bool leftItemDown = false;
    bool rightItemDown = false;
    private void OnEnable()
    {
        if (controller == null)
        {
            controller = GameManager.Instance.Player.Controller;
        }

        childImages = GetComponentsInChildren<Image>();
        UpdateUI();
        InvokeRepeating("HandleInput", waitTime, waitTime);

        leftItemDown = false;
        rightItemDown = false;
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Update()
    {
        if (GameManager.Instance.IsPaused)
        {
            if (controller.ActivateLeftItem)
            {
                leftItemDown = true;
                rightItemDown = false;
            }
            else if (controller.DeactivateLeftItem)
            {
                leftItemDown = false;
            }

            if (controller.ActivateRightItem)
            {
                rightItemDown = true;
                leftItemDown = false;
            }
            else if (controller.DeactivateRightItem)
            {
                rightItemDown = false;
            }
        }
    }
    private void HandleInput()
    {
        if (GameManager.Instance.IsPaused)
        {
            Image curImg = GetComponentsInChildren<Image>()[curSelected + 1];
            curImg.color = Color.white;
            curSelected += controller.Horizontal + controller.Vertical * 2;
            curSelected += 4;
            curSelected %= 4;

            curImg = childImages[curSelected + 1];
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
            }
        }

    }

    public void UpdateUI()
    {
        if (GameManager.Instance.IsPaused)
        {
            List<UsableItem> items = GameManager.Instance.Player.Inventory;

            for (int i = 0; i < items.Count; i++)
            {
                UsableItem curItem = items[i];
                childImages[i + 1].sprite = curItem ? curItem.uiSprite : null;
                
            }
        }
    }
}
