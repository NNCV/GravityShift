using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySettingValueScript : MonoBehaviour {

    public int mode;

    public Slider from;
    public Text to;

	void Start () {
	}
	
	void Update () {
        to.text = from.value.ToString();
	}
}
