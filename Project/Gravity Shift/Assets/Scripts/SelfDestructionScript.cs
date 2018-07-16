using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructionScript : MonoBehaviour {

    public float timeC, timeM;
    
	void FixedUpdate () {
        timeC += Time.deltaTime;
        if(timeC >= timeM)
        {
            Destroy(gameObject);
        }
	}
}
