using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
	static CameraManager instance = null;
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    Player target;
    int fwd = 1;
    private Camera cam;
	public Canvas canvas;
    public float playerOffsetPercent = 0.08f;
    Vector3 screenSize;
    Vector3 vel = Vector3.zero;

    // Use this for initialization
    void Awake () {
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);

            cam = GetComponent<Camera>();
            canvas.gameObject.SetActive(true);
            

            target = GameManager.Instance.Player;
			Vector3 camPos = target.transform.position;
			camPos.z = -10;
			transform.position = camPos;
			
           
            screenSize = new Vector2(Screen.width, Screen.height);
            Vector3 playerPos = cam.WorldToScreenPoint(target.transform.position);
            cam.transform.position = cam.ScreenToWorldPoint(playerPos + new Vector3(fwd * screenSize.x * playerOffsetPercent, -screenSize.y));

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
	void FixedUpdate () {
		if (!GameManager.Instance.IsPaused)
		{
			if (target)
			{
                Vector3 playerPos = cam.WorldToScreenPoint(target.transform.position);
                float dif = Mathf.Abs(playerPos.x - screenSize.x / 2);
                Vector3 newCamPos = cam.transform.position;
                 if (target.Forward.x != fwd && dif >= screenSize.x * playerOffsetPercent * 2)
                {
                    fwd *= -1;
                    vel = Vector3.zero;
                    
                }
                newCamPos.z = -10;
                Vector3 posTarget = cam.ScreenToWorldPoint(playerPos + new Vector3(fwd * screenSize.x * playerOffsetPercent, screenSize.y / 6));
                posTarget.z = -10;
                newCamPos = Vector3.SmoothDamp(cam.transform.position, posTarget, ref vel, dampTime);
                cam.transform.position = newCamPos;
            }
            else
            {
                target = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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

	public Vector3 Velocity
	{
		get
		{
			return velocity;
		}

		set
		{
			velocity = value;
		}
	}
}
