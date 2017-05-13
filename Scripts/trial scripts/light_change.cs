using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//adapted by Camille Baumann-Jaeger from Unity Manual code found on https://docs.unity3d.com/ScriptReference/Light-color.html

public class light_change : MonoBehaviour {
    public float duration = 1.0F;
    public Color color0 = Color.red;
    public Color color1 = Color.blue;
    public Light lt;

    void Start() {
        lt = GetComponent<Light>();
    }
    void Update() {
        float t = Mathf.PingPong(Time.time, duration) / duration;
        if (Input.GetKeyDown(KeyCode.R))
        {
        	lt.color = Color.Lerp(color0, color1, t);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
        	lt.color = Color.Lerp(color1, color0,t);
        }
    }
}