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
    private void OnEnable()
    {
        if (controller == null)
        {
            controller = GameManager.Instance.Player.Controller;
        }

        childImages = GetComponentsInChildren<Image>();
        UpdateUI();
        InvokeRepeating("HandleInput", waitTime, waitTime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Update()
    {
        if (controller.ActivateItem)
        {
            leftItemDown = true;
        } else if (controller.DeactivateItem)
        {
            leftItemDown = false;
        }
    }
    private void HandleInput()
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


    }

    public void UpdateUI()
    {
        List<UsableItem> items = GameManager.Instance.Player.Inventory;
        for(int i = 0; i < items.Count; i++)
        {
            childImages[i + 1].sprite = items[i].uiSprite;
        }
    }
}
