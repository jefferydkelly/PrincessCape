using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IndicatorDirection : MonoBehaviour {

    public Sprite[] sprites;
    Image mySprite;
	// Use this for initialization
	void Start () {
        mySprite = GetComponent<Image>();
	}

    // Update is called once per frame
    void Update() {
        Vector2 temp = GameManager.Instance.Player.Aiming;

        if (temp == new Vector2(0, 1))
        {
            mySprite.sprite = sprites[0];
        }
        if (temp == new Vector2(1, 1))
        {
            mySprite.sprite = sprites[1];
        }
        if (temp == new Vector2(1, 0))
        {
            mySprite.sprite = sprites[2];
        }
        if (temp == new Vector2(1, -1))
        {
            mySprite.sprite = sprites[3];
        }
        if (temp == new Vector2(0, -1))
        {
            mySprite.sprite = sprites[4];
        }

        if (temp == new Vector2(-1, -1))
        {
            mySprite.sprite = sprites[5];
        }
        if (temp == new Vector2(-1, -0))
        {
            mySprite.sprite = sprites[6];
        }

        if (temp == new Vector2(-1, 1))
        {
            mySprite.sprite = sprites[7];
        }
    }
}
