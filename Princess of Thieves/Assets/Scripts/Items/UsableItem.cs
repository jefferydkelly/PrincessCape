using UnityEngine;
using System.Collections;

public abstract class UsableItem : MonoBehaviour {

    public Sprite uiSprite;
    public string itemName;
    protected bool onCooldown = false;
    public float cooldownTime = 0.0f;
    public abstract void Use();
}
