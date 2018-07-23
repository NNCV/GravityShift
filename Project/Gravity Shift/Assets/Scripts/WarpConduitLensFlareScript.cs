using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpConduitLensFlareScript : MonoBehaviour {
    
    public float scaleOrigin, offsetX, offsetY, multiplierAfter, multiplierBefore;

	void FixedUpdate () {
        transform.localScale = new Vector3(scaleOrigin + Mathf.PerlinNoise((Time.time + offsetX) * multiplierBefore, (Time.time + offsetX) * multiplierBefore) * multiplierAfter, scaleOrigin + Mathf.PerlinNoise((Time.time + offsetY) * multiplierBefore, (Time.time + offsetY) * multiplierBefore) * multiplierAfter, 1f);
	}
}
