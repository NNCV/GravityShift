using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterScript : MonoBehaviour
{
    public PlayerManager pm;

    public GameObject blast;
    public Transform blastSpawnZone;
    public float offset = -90f;
    public float pointSpeed;

    public float shakeMovMin = 0f;
    public float shakeMovMax = 0f;
    public float shakeRotMin = 0f;
    public float shakeRotMax = 0f;

    public int blasterEnergyDrain = 0;
    public float blasterFireRate = 0f;
    public float blasterCooldown = 0f;

    Vector3 dif;

    void Start()
    {
        blastSpawnZone = transform.GetChild(1).transform;
    }

    void FixedUpdate()
    {
        if (pm.isFrozen == false)
        {
            dif = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            dif.Normalize();

            blasterCooldown += Time.deltaTime;
        }

        float rotZ = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, rotZ + offset), Time.deltaTime * pointSpeed);
    }

    public void Fire()
    {
        if (blasterCooldown >= blasterFireRate)
        {
            float shakeMovAddX = Random.Range(shakeMovMin, shakeMovMax);
            float shakeMovAddY = Random.Range(shakeMovMin, shakeMovMax);
            float shakemovAddRot = Random.Range(shakeRotMin, shakeRotMax);
            Instantiate(blast, blastSpawnZone.position, blastSpawnZone.rotation);
            Camera.main.GetComponent<CameraMovementManager>().Shake(shakeMovAddX, shakeMovAddY, shakemovAddRot);
            blasterCooldown = 0;
        }
    }
}