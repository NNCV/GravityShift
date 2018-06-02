using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementManager : MonoBehaviour
{

    public float focusSpeed = 1f;
    public float recoverSpeed = 1f;
    public GameObject target;
    public float zoom = 6f;
    public float zoomMin = 4f;
    public float zoomMax = 16f;
    public float zoomSpeed = 1f;
    public float zoomDif = 4f;

    public Vector3 initialPosition;

    public Camera cam;

    public Vector3 finalMov;
    public Quaternion finalRot;

    public float shakeX = 0f;
    public float shakeY = 0f;
    public float shakeRot = 0f;

    public float shakeThreshold = 0.05f;

    void Start()
    {
        target = FindObjectOfType<PlayerEquipmentManager>().transform.gameObject;
        cam = GetComponent<Camera>();
        if (target != null)
        {
            transform.position = target.transform.position + new Vector3(0f, 0f, -10f);
        }
        else
        {
            transform.position = initialPosition;
        }
    }

    void FixedUpdate()
    {

        zoom += Input.GetAxisRaw("Mouse ScrollWheel") / zoomDif;
        zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);

        finalMov = target.transform.position + new Vector3(shakeX, shakeY, -10f);
        finalRot = target.transform.rotation * Quaternion.Euler(0f, 0f, shakeRot);

        if (shakeX <= shakeThreshold)
        {
            shakeX = 0f;
        }
        else shakeX = shakeX / recoverSpeed;
        if (shakeY <= shakeThreshold)
        {
            shakeY = 0f;
        }
        else shakeY = shakeY / recoverSpeed;
        if (shakeRot <= shakeThreshold)
        {
            shakeRot = 0f;
        }
        else shakeRot = shakeRot / recoverSpeed;

        transform.position = Vector3.Lerp(transform.position, finalMov, focusSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, finalRot, focusSpeed * Time.deltaTime);

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoom, Time.deltaTime * zoomSpeed);
    }

    public void Shake(float shakeXIN, float shakeYIN, float shakeRotIN)
    {
        shakeX += shakeXIN;
        shakeY += shakeYIN;
        shakeRot += shakeRotIN;
    }
}