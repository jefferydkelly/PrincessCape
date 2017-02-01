using UnityEngine;
using System.Collections;

public class GateController : JDMappableObject, ActivateableObject {

	public int openFrames = 60;
	private bool isActive = false;
	protected Vector3 startPosition;
	protected Vector3 endPosition;
    public bool startOpen = false;
    SpriteRenderer myRenderer;
    BoxCollider2D myCollider;

    [SerializeField]
    Sprite[] sprites;
	[SerializeField]
	AudioClip closeSound;
	// Use this for initialization
	void Start () {
		
		startPosition = transform.position;
        myRenderer = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<BoxCollider2D>();
		//endPosition = startPosition + new Vector3(0, GetComponent<SpriteRenderer>().bounds.extents.y * 2);
        if (startOpen)
        {
            isActive = true;
            // transform.position = endPosition;
            OnOpen();
        }
    }

	public void Activate()
	{
		if (!isActive)
		{
			StopAllCoroutines();
			isActive = true;
            OnOpen();
			//StartCoroutine(Open());
		}
	}

	protected IEnumerator Open()
	{
		//Vector3 dif = (endPosition - startPosition) / openFrames;

		//while (transform.position.y < endPosition.y)
		//{
		//	if (!GameManager.Instance.IsPaused)
		//	{
		//		transform.position += dif;
		//	}
		//	yield return null;
		//}
		//transform.position = endPosition;
        //OnOpen();
		yield return null;
	}

    protected virtual void OnOpen()
    {
        myRenderer.sprite = sprites[1];
        myCollider.isTrigger = true;
    }

	public void Deactivate()
	{
		if (isActive)
		{
			isActive = false;
			StopAllCoroutines();
            //StartCoroutine(Close());
            OnClose();
		}
	}

	protected IEnumerator Close()
	{
		//Vector3 dif = (startPosition - endPosition) / openFrames;
		//while (transform.position.y > startPosition.y)
		//{
		//	if (!GameManager.Instance.IsPaused)
		//	{
		//		transform.position += dif;
		//	}
		//	yield return null;
		//}
		//transform.position = startPosition;

       
		yield return null;
	}

    protected virtual void OnClose()
    {
		AudioManager.Instance.PlaySound (closeSound);
        myRenderer.sprite = sprites[0];
        myCollider.isTrigger = false;
    }

	public bool IsActive
	{
		get
		{
			return isActive;
		}
	}
}
