using UnityEngine;
using System.Collections;

public class GateController : JDMappableObject, ActivateableObject {

	public int openFrames = 60;
	private bool isActive = false;
	protected Vector3 startPosition;
	protected Vector3 endPosition;
    public bool startOpen = false;

    [SerializeField]
    Sprite[] sprites;
	// Use this for initialization
	void Start () {
		
		startPosition = transform.position;
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
			StartCoroutine(Open());
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
        OnOpen();
		yield return null;
	}

    protected virtual void OnOpen()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[1];
        Destroy(GetComponent<BoxCollider2D>());
    }

	public void Deactivate()
	{
		if (isActive)
		{
			isActive = false;
			StopAllCoroutines();
			StartCoroutine(Close());
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
        OnClose();
		yield return null;
	}

    protected virtual void OnClose()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[0];
        gameObject.AddComponent<BoxCollider2D>();
    }

	public bool IsActive
	{
		get
		{
			return isActive;
		}
	}
}
