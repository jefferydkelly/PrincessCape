using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
	static CameraManager instance = null;
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public GameObject target;
    private Camera cam;
	public Canvas canvas;

    // Use this for initialization
    void Awake () {
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
			target = GameObject.FindGameObjectWithTag("Player");
			Vector3 camPos = target.transform.position;
			camPos.z = -10;
			transform.position = camPos;
			cam = Camera.main;
			canvas.gameObject.SetActive(true);
		}
		else {
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>The instance.</value>
	public static CameraManager Instance
	{
		get
		{
			return instance;
		}
	}
	// Update is called once per frame
	void Update () {
		if (!GameManager.Instance.IsPaused)
		{
			if (target)
			{
				Vector3 point = cam.WorldToViewportPoint(target.transform.position);
				Vector3 delta = target.transform.position - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
				Vector3 destination = transform.position + delta;
				transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
			}
		}
    }

	/// <summary>
	/// Pan the specified panDistance and time.
	/// </summary>
	/// <param name="panDistance">Pan distance.</param>
	/// <param name="time">Time.</param>
	public void Pan(Vector3 panDistance, float time)
	{
		if (GameManager.Instance.IsInCutscene)
		{
			StartCoroutine(IPan(panDistance, time));	
		}
	}

	/// <summary>
	/// Pans the given distance over the given time.
	/// </summary>
	/// <returns>An ienumerator</returns>
	/// <param name="pan">Pan distance.</param>
	/// <param name="time">Time.</param>
	IEnumerator IPan(Vector3 pan, float time)
	{
		float timePanning = 0;
		Vector3 startPos = transform.position;
		while (timePanning < time)
		{
			timePanning += Time.deltaTime;
			timePanning = Mathf.Min(timePanning, time);
			transform.position = startPos + pan * timePanning / time;
			yield return null;
		}

		GameManager.Instance.Cutscene.NextElement();
	}

	public void Zoom(float zoomLevel, float time)
	{
		StartCoroutine(IZoom(zoomLevel, time));
	}

	IEnumerator IZoom(float zoomLevel, float time)
	{
		yield return null;
	}
}
