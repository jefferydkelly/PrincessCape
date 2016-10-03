using UnityEngine;
using System.Collections;

public class GateController : MonoBehaviour, ActivateableObject {

	public int openFrames = 60;
	private bool isActive = false;
	protected Vector3 startPosition;
	protected Vector3 endPosition;
	// Use this for initialization
	void Start () {
		
		startPosition = transform.position;
		endPosition = startPosition - new Vector3(0, GetComponent<SpriteRenderer>().bounds.extents.y * 2);
	}

	public void Activate()
	{
		if (!isActive)
		{
			isActive = true;
			StartCoroutine(Open());
		}
	}

	protected IEnumerator Open()
	{
		Vector3 dif = (endPosition - startPosition) / openFrames;

		while (transform.position.y < endPosition.y)
		{
			transform.position += dif;
			yield return null;
		}
		transform.position = endPosition;
		yield return null;
	}

	public void Deactivate()
	{
		if (isActive)
		{
			isActive = false;
			StartCoroutine(Close());
		}
	}

	protected IEnumerator Close()
	{
		Vector3 dif = (startPosition - endPosition) / openFrames;
		while (transform.position.y > startPosition.y)
		{
			transform.position += dif;
			yield return null;
		}
		transform.position = startPosition;
		yield return null;
	}

	public bool IsActive
	{
		get
		{
			return isActive;
		}
	}
}
