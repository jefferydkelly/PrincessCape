using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour {
	static CameraManager instance = null;
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    GameObject target;
    int fwd = 1;
    private Camera cam;
	public Canvas canvas;
    public float playerOffsetPercent = 0.25f;
    Vector3 screenSize;
    Vector3 vel = Vector3.zero;
	Vector3 playerPos = new Vector3 ();
	Vector3 newCamPos = new Vector3();
	Vector3 posTarget = new Vector3();
    // Use this for initialization
	static bool isClosing = false;
	static bool addedFunction = false;
	GameManager manager;
    [SerializeField]
    Texture2D fadeTexture;
    bool fading = false;
    int fadeDir = 1;
    float fadeTime = 4.0f;
    float alpha = 1.0f;
    string sceneToLoad = "";

	void Awake() {
		if (!addedFunction) {
			SceneManager.sceneLoaded += DetermineCameraInstance;
		}
	}

    void DetermineCameraInstance(Scene scene, LoadSceneMode lsm) {
        if (instance != this) {
            if (instance == null)
            {
                if (scene.name.StartsWith("JD") || (scene.name.StartsWith("Rose")))
                {
                    isClosing = false;
                    instance = this;
                    manager = GameManager.Instance;
                    DontDestroyOnLoad(gameObject);

                    cam = GetComponent<Camera>();
                    canvas.gameObject.SetActive(true);

                    DontDestroyOnLoad(AudioManager.Instance.AttachedObject);

                    CenterCamera();
                }
			} else {
                Destroy(gameObject);
			}
		} else {
            if (scene.name.StartsWith("JD") || (scene.name.StartsWith("Rose")))
            {
                target = GameManager.Instance.Player.gameObject;
                fwd = (int)target.GetComponent<Player>().Forward.x;
                CenterCamera();
            } else
            {
                fading = false;
                
                isClosing = true;
                instance = null;
                Destroy(gameObject);
            }
            
		}
	}

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= DetermineCameraInstance;
    }
    void CenterCamera() {
		target = GameManager.Instance.Player.gameObject;
		Vector3 camPos = target.transform.position;
		camPos.z = -10;
		transform.position = camPos;


		screenSize = new Vector2 (Screen.width, Screen.height);
		Vector3 playerPos = cam.WorldToScreenPoint (target.transform.position);


		camPos = cam.ScreenToWorldPoint (playerPos + new Vector3 (fwd * screenSize.x * playerOffsetPercent, -screenSize.y));
		camPos.z = -10;
		cam.transform.position = camPos;
	}	
	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>The instance.</value>
	public static CameraManager Instance
	{
		get
		{
            
			if (!isClosing) {
				return instance;
			}

			return null;
		}
	}
		
	// Update is called once per frame
	void LateUpdate () {

		if (manager != null) {
            if (target && target.activeSelf)
            {
                playerPos = cam.WorldToScreenPoint(target.transform.position);
                newCamPos = cam.transform.position;

                newCamPos.z = -10;
                Player player = target.GetComponent<Player>();
                if (player != null)
                {
                    if (player.Forward.x == fwd)
                    {

                        posTarget = cam.ScreenToWorldPoint(playerPos + new Vector3(fwd * screenSize.x * playerOffsetPercent, screenSize.y / 6));

                    }
                    else if (Mathf.Abs(playerPos.x - screenSize.x / 2) >= screenSize.x * playerOffsetPercent)
                    {
                        fwd *= -1;

                        posTarget = cam.ScreenToWorldPoint(playerPos + new Vector3(fwd * screenSize.x * playerOffsetPercent, screenSize.y / 6));
                    }
                    else
                    {
                        posTarget = cam.ScreenToWorldPoint(new Vector3(screenSize.x / 2, playerPos.y + screenSize.y / 6));
                    }
                } else
                {
                    posTarget = cam.ScreenToWorldPoint(playerPos + new Vector3(fwd * screenSize.x * playerOffsetPercent, screenSize.y / 6));
                }

				posTarget.z = -10;
				newCamPos = Vector3.SmoothDamp (cam.transform.position, posTarget, ref vel, dampTime);
				cam.transform.position = newCamPos;
			} else {
				GameObject go = GameObject.FindGameObjectWithTag ("Player");
				if (go) {
                    target = go;
				}
			}
		} else if (manager == null) {
			manager = GameManager.Instance;
			target = manager.Player.gameObject;
			cam = GetComponent<Camera> ();
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
        
		transform.position = startPos + pan;
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

	public Vector3 MouseWorldPos {
		get {
			Vector3 mousePos = Input.mousePosition;
			mousePos.z = 10;
			return Camera.main.ScreenToWorldPoint (mousePos);
		}
	}

    private void OnGUI()
    {
        if (fading)
        {
            Color guiColor = GUI.color;
            alpha -= fadeDir * Time.deltaTime / fadeTime;
            alpha = Mathf.Clamp01(alpha);
            guiColor.a -= alpha;

            GUI.color = guiColor;
            GUI.depth = -1000;
           
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);

            if (alpha == 1)
            {
                GameManager.Instance.Player.ResetStatus();
                fading = false;
            } else if (alpha == 0)
            {
                SceneManager.LoadScene(sceneToLoad);
                fadeDir = 0;
                SceneManager.sceneLoaded += FadeIntoScene;
            }
        }
    }

    private void FadeIntoScene(Scene arg0, LoadSceneMode arg1)
    {
        fadeDir = -1;
        SceneManager.sceneLoaded -= FadeIntoScene;
    }

    public void FadeOutToNewScene(string sceneName)
    {
        if (!fading || fadeDir == 1)
        {
            GameManager.Instance.Player.IsFrozen = true;
            sceneToLoad = sceneName;
            fading = true;
            fadeDir = 1;
        }
    }

    public GameObject Target
    {
        get
        {
            return target;
        }

        set
        {
            target = value;
        }
    }

    public void SetToPlayerFwd ()
    {
        fwd = (int)GameManager.Instance.Player.Forward.x;
    }
}
