using UnityEngine;
using System.Collections;

public class TresureChestController : JDMappableObject, InteractiveObject {

    public Sprite openedSprite;
    public Sprite closedSprite;
    public Sprite highlightedSprite;
    bool opened = false;
    [SerializeField]
    GameObject contents;
    SpriteRenderer myRenderer;

    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
    }

    public void Interact()
    {
        if (!opened)
        {
            UIManager.Instance.ShowInteraction("");
            GameManager.Instance.Player.AddItem(contents);
            myRenderer.sprite = openedSprite;
            opened = true;
        }
    }

    public void Highlight()
    {
        if (!opened)
        {
            UIManager.Instance.ShowInteraction("Open");
            myRenderer.sprite = highlightedSprite;
        }
    }

    public void Dehighlight()
    {
        if (!opened)
        {
            UIManager.Instance.ShowInteraction("");
            myRenderer.sprite = closedSprite;
        }
    }
}
