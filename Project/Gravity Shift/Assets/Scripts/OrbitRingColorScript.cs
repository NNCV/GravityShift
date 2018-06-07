using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitRingColorScript : MonoBehaviour {

    public Color pColor;

	void Start () {
        pColor.a = 0.25f;
        GetComponent<SpriteRenderer>().color = pColor;
	}
	
}
