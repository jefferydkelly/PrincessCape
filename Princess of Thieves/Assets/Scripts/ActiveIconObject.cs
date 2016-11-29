using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ActiveIconObject: MonoBehaviour
{

    [SerializeField]
    GameObject icon;

    [SerializeField]
    string displayText = "This is an object you have inspected";

    bool displayIcon = false;

    // Use this for initialization
    void Start()
    {
        icon.SetActive(false);// = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            ShowIcon();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            HideIcon();
        }
    }
    public void ShowIcon()
    {
        icon.SetActive(true);
    }
    public void HideIcon()
    {
        icon.SetActive(false);
    }

    public void DestroyIcon()
    {
        Destroy(gameObject);
    }
    public void Interact()
    {

    }
}
