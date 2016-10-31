using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class StealthMeter {

    Image meter;
    List<Sprite> sprites;
    float lightLevel = 0.0f;

    public StealthMeter()
    {
        meter = GameObject.Find("StealthMeter").GetComponent<Image>();
        sprites = Resources.LoadAll<Sprite>("Sprites/StealthMeter").ToList();
        meter.fillAmount = 1;
    }

    public float LightLevel
    {
        get
        {
            return lightLevel;
        }

        set
        {
            lightLevel = value;
            meter.sprite = sprites[Mathf.FloorToInt(value * 4)];
            
        }
    }

    public bool Enabled
    {
        get
        {
            return meter.enabled;
        }

        set
        {
            meter.enabled = value;
        }
    }
}
