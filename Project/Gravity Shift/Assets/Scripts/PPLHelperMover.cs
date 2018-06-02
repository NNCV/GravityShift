using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPLHelperMover : MonoBehaviour {

    public Transform player;
    public bool isInCutscene = false;

    public Transform posToGo;

    void FixedUpdate()
    {
        if (isInCutscene == false)
        {
            transform.position = player.position;
            transform.rotation = player.rotation;
        }
        else
        {
            transform.position = player.position + posToGo.position;
            transform.rotation = player.rotation * posToGo.rotation;
        }
    }
}
