using UnityEngine;
using System.Collections;

public abstract class UsableItem : MonoBehaviour {

    /// <summary>
    /// The user interface sprite.
    /// </summary>
    public Sprite uiSprite;

	/// <summary>
	/// The name of the item.
	/// </summary>
    public string itemName;
    
	public int activationManaCost;
	public int manaPerSecondCost;

	/// <summary>
	/// The description.
	/// </summary>
    public string[] description;

	/// <summary>
	/// The info of the item to be displayed in the inventory menu
	/// </summary>
    public string info;

	/// <summary>
	/// Whether or not the item is on cooldown
	/// </summary>
    protected bool onCooldown = false;

	[SerializeField]
	bool continuous = false;

	protected bool itemActive = false;
	/// <summary>
	/// The cooldown time.
	/// </summary>
    public float cooldownTime = 0.0f;
	protected Timer cooldownTimer;
	protected WaitDelegate cooldownDelegate;

	/// <summary>
	/// Activate this instance.
	/// </summary>
    public abstract void Activate();

	/// <summary>
	/// Deactivate this instance.
	/// </summary>
    public abstract void Deactivate();

	public abstract void Use();

	/// <summary>
	/// Creates the cooldown timer.
	/// </summary>
	protected void CreateCooldownTimer() {
		cooldownDelegate = () => {
			onCooldown = false;
		};

		cooldownTimer = new Timer (cooldownDelegate, cooldownTime);
	}

	/// <summary>
	/// Starts the cooldown.
	/// </summary>
	protected void StartCooldown() {
		onCooldown = true;
		cooldownTimer.Reset ();
		TimerManager.Instance.AddTimer (cooldownTimer);
	}

	public bool Continuous {
		get {
			return continuous;
		}
	}

	public bool IsActive {
		get {
			return itemActive;
		}
	}
}
