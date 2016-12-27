using UnityEngine;
using System.Collections;

public abstract class UsableItem : MonoBehaviour {

    
    public Sprite uiSprite;
    public string itemName;
    //public int manaCast = 5;
    public string[] description;
    public string info;
    protected bool onCooldown = false;
    public float cooldownTime = 0.0f;
    public abstract void Activate();
    public abstract void Deactivate();

    public abstract void UseMana();
}
