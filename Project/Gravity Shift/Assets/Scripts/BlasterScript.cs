using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterScript : MonoBehaviour
{
    public PlayerManager pm;

    public GameObject blast;
    public Transform blastSpawnZone;

    public int blasterEnergyDrain = 0;
    public float blasterFireRate = 0f;

    Vector3 dif;

    void Start()
    {
        blastSpawnZone = transform.GetChild(1).transform;
    }
    

    public void Fire()
    {/*
        float shakeMovAddX = Random.Range(shakeMovMin, shakeMovMax);
        float shakeMovAddY = Random.Range(shakeMovMin, shakeMovMax);
        float shakemovAddRot = Random.Range(shakeRotMin, shakeRotMax);
        Instantiate(blast, blastSpawnZone.position, blastSpawnZone.rotation);
        Camera.main.GetComponent<CameraMovementManager>().Shake(shakeMovAddX, shakeMovAddY, shakemovAddRot);
        */
    }
}