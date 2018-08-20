using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRotatorScript : MonoBehaviour {

    public Vector3 rotateBy;

	void Update () {
        transform.Rotate(rotateBy);
	}
}
