using UnityEngine;
using System.Collections;

public class SimpleSpellPlaceholder : MonoBehaviour {


    public Vector3 fwd = new Vector3(1, 0, 0);
    public float moveSpeed = 10;
    float oldXVel = 0;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        oldXVel = GetComponent<Rigidbody2D>().velocity.x;

        if (Mathf.Abs(oldXVel) < Mathf.Epsilon)
        {
            Destroy(gameObject);
        }
    }
    public void Cast(GameObject playerInfo)
    {
        transform.position = playerInfo.transform.position;
        FWD = GameManager.Instance.Player.Forward;
        Init();
    }

    public void Init()
    {
        GetComponent<Rigidbody2D>().AddForce(FWD * 5f, ForceMode2D.Impulse);
    }

    public  Vector3 FWD
    {
        get
        {
            return fwd;
        }

        set
        {
            fwd = value;
            if (fwd.y == 0)
            {
                GetComponent<SpriteRenderer>().flipX = (fwd.x == -1);
            }
            else
            {
                transform.Rotate(Vector3.forward, 90 * fwd.y);
            }
        }
    }

}
