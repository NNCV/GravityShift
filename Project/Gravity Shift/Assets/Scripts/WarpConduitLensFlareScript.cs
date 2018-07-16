using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpConduitLensFlareScript : MonoBehaviour {

    public float offsetX, offsetY, multiplierAfter, multiplierBefore;

	void FixedUpdate () {
        transform.localScale = new Vector3(1f + Mathf.PerlinNoise((Time.time + offsetX) * multiplierBefore, (Time.time + offsetX) * multiplierBefore) * multiplierAfter, 1f + Mathf.PerlinNoise((Time.time + offsetY) * multiplierBefore, (Time.time + offsetY) * multiplierBefore) * multiplierAfter, 1f);
	}
}
