using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovementManager : MonoBehaviour
{
    public LevelManager lm;
    public Transform mapCenter;
    public PlayerMapTargetManager pmtm;
    public PlayerManager pm;

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
    public Camera mapCam;

    public Vector3 finalMov;
    public Quaternion finalRot;

    public float shakeX = 0f;
    public float shakeY = 0f;
    public float shakeRot = 0f;

    public float shakeThreshold = 0.05f;

    public bool isInMapScreen = false;
    public float timeCurrent = 0;
    public float timeRate = 0;
    public bool toResetIMS = false;

    public float mapZoom, mapZoomMin, mapZoomMax, mapZoomSpeed, mapZoomDif;
    public Vector3 mapPos;
    public Quaternion mapRot;
    public GameObject mapTarget;
    public GameObject mapSelectedSystem;
    public GameObject mapSelectedSector;
    public GameObject mapSelector;
    public GameObject mapJumpRangeDisplay;
    public float jumpRange;
    public bool selected = false;
    public bool isSystemView = true;
    public float selectorScalar = 10f;
    public float systemPlanetsScalar = 40f;
    public float systemSectorScalar = 50f;
    public int currentSector = 0;
    public float mapTravelSpeed, mapTravelMult;
    public float mapScalableSpeed;
    public float mapTargetScalableScale, mapTargetScale;
    public Button systemGalaxyViewButton;
    public Button jumpButton;

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

    public void switchMapScreen()
    {
        isInMapScreen = !isInMapScreen;
    }

    public void switchSectorGalaxy()
    {
        if(selected == true)
        {
            currentSector = 0;
            isSystemView = !isSystemView;
            mapSelectedSector.gameObject.SetActive(isSystemView);
            if(isSystemView == false)
            {
                restartMap();
                selected = false;
            }
        }
    }

    public void restartMap()
    {
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("MapPart"))
        {
            Destroy(go);
        }
        lm.GenerateGalaxyMap(mapCenter);
    }

    void FixedUpdate()
    {
        if(selected == false)
        {
            systemGalaxyViewButton.interactable = false;
        }
        else
        {
            systemGalaxyViewButton.interactable = true;
        }

        if(isSystemView == false)
        {
            jumpButton.interactable = false;
        }
        else
        {
            jumpButton.interactable = true;
        }

        if (isInMapScreen == false)
        {
            zoom += Input.GetAxisRaw("Mouse ScrollWheel") / zoomDif;
            zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);

            finalMov = target.transform.position + new Vector3(shakeX, shakeY, -150f);
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
        else
        {
            mapJumpRangeDisplay.transform.localScale = new Vector3(jumpRange, jumpRange, jumpRange);

            mapJumpRangeDisplay.transform.position = pm.currentPositionMap.position;

              //      mapSelectedSector.transform.position = mapSelectedSystem.GetComponent<MapDataContainerScript>().returnPlanetPosition(currentSector, systemPlanetsScalar, mapSelectedSystem.transform);
            //pos.x = origin.position.x + a * scalar * currentGalaxy.systems[a].systemCentre.rot1;

            mapScalableSpeed = mapZoom / mapTravelMult;
            mapTargetScalableScale = (mapZoom / mapTravelMult) * mapTargetScale;

            if (mapSelectedSystem != null)
            {
                mapSelector.SetActive(true);
                mapSelector.transform.position = mapSelectedSystem.transform.position;
                mapSelector.transform.localScale = new Vector3(mapSelectedSystem.transform.localScale.x * selectorScalar, mapSelectedSystem.transform.localScale.y * selectorScalar, mapSelectedSystem.transform.localScale.z * selectorScalar);
            }
            else
            {
                mapSelector.SetActive(false);
            }

            if (isSystemView == false)
            {
                jumpButton.enabled = false;
                pmtm.startSpriteRenderer();
               // mapSelector.GetComponent<Renderer>().sharedMaterial.color = new Color(1f, 1f, 1f, 0.8f);
                //mapSelector.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.8f);
                mapTarget.transform.position = Vector3.Lerp(mapTarget.transform.position, mapTarget.transform.position + new Vector3(Input.GetAxisRaw("Horizontal") * mapScalableSpeed, -Input.GetAxisRaw("Vertical") * mapScalableSpeed, 0f), Time.deltaTime * mapTravelSpeed);
                mapTarget.transform.localScale = Vector3.Lerp(mapTarget.transform.localScale, new Vector3(mapTargetScalableScale, mapTargetScalableScale, mapTargetScalableScale), Time.deltaTime * mapTravelSpeed);

                mapZoom += Input.GetAxisRaw("Mouse ScrollWheel") / mapZoomDif;
                mapZoom = Mathf.Clamp(mapZoom, mapZoomMin, mapZoomMax);

                mapPos = mapTarget.transform.position + new Vector3(0f, 0f, -50f);

                mapCam.transform.position = Vector3.Lerp(mapCam.transform.position, mapPos, Time.deltaTime * mapTravelSpeed);
                mapCam.transform.rotation = mapRot;

                cam.orthographicSize = 72;
                mapCam.orthographicSize = Mathf.Lerp(mapCam.orthographicSize, mapZoom, Time.deltaTime * mapZoomSpeed);
            }
            else
            {
                if (Vector3.Distance(pm.currentPositionMap.position, mapSelectedSector.transform.position) <= jumpRange)
                {
                    jumpButton.enabled = true;
                }
                pmtm.stopSpriteRenderer();
               // mapSelector.GetComponent<Renderer>().sharedMaterial.color = new Color(mapSelector.GetComponent<Renderer>().sharedMaterial.color.r, mapSelector.GetComponent<Renderer>().sharedMaterial.color.g, mapSelector.GetComponent<Renderer>().sharedMaterial.color.b, Mathf.Lerp(mapSelector.GetComponent<Renderer>().sharedMaterial.color.a, 0.2f, Time.deltaTime * mapZoomSpeed));
                //mapSelector.GetComponent<SpriteRenderer>().color = new Color(mapSelectedSystem.GetComponent<SpriteRenderer>().color.r, mapSelectedSystem.GetComponent<SpriteRenderer>().color.g, mapSelectedSystem.GetComponent<SpriteRenderer>().color.b, Mathf.Lerp(mapSelectedSystem.GetComponent<SpriteRenderer>().color.a, 0.1f, Time.deltaTime * mapZoomSpeed));
                if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    currentSector++;
                    if(currentSector >= mapSelectedSystem.GetComponent<MapDataContainerScript>().slo.systemPlanets.Length)
                    {
                        currentSector = 0;
                    }
                }
                else if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    currentSector--;
                    if(currentSector < 0)
                    {
                        currentSector = mapSelectedSystem.GetComponent<MapDataContainerScript>().slo.systemPlanets.Length - 1;
                    }
                }
                mapSelectedSector.transform.localScale = new Vector3(systemSectorScalar + mapSelectedSystem.GetComponent<MapDataContainerScript>().slo.systemPlanets[currentSector].planetRadius, systemSectorScalar + mapSelectedSystem.GetComponent<MapDataContainerScript>().slo.systemPlanets[currentSector].planetRadius, systemSectorScalar + mapSelectedSystem.GetComponent<MapDataContainerScript>().slo.systemPlanets[currentSector].planetRadius);
                
                

                //Debug.Log(mapSelectedSystem.GetComponent<MapDataContainerScript>().slo.systemPlanets[currentSector].sectorName);
                mapSelectedSystem.GetComponent<MapDataContainerScript>().showPlanets(systemPlanetsScalar, mapSelectedSystem.transform, 1);
                if (currentSector == 0)
                {
                    mapSelectedSector.transform.position = mapSelectedSystem.GetComponent<MapDataContainerScript>().returnPlanetPosition(currentSector, 0f, mapSelectedSystem.transform);
                }
                else
                {
                    mapSelectedSector.transform.position = mapSelectedSystem.GetComponent<MapDataContainerScript>().returnPlanetPosition(currentSector, systemPlanetsScalar, mapSelectedSystem.transform);
                }
                mapSelector.transform.localScale = Vector3.Lerp(mapSelector.transform.localScale, new Vector3((mapSelectedSystem.GetComponent<MapDataContainerScript>().slo.systemPlanetCount + 50) * systemSectorScalar, (mapSelectedSystem.GetComponent<MapDataContainerScript>().slo.systemPlanetCount + 50) * systemSectorScalar, (mapSelectedSystem.GetComponent<MapDataContainerScript>().slo.systemPlanetCount + 50) * systemSectorScalar), Time.deltaTime * mapZoomSpeed);
                mapCam.transform.position = Vector3.Lerp(mapCam.transform.position, new Vector3(mapSelectedSystem.transform.position.x, mapSelectedSystem.transform.position.y, mapCam.transform.position.z), Time.deltaTime * mapZoomSpeed);
                mapCam.orthographicSize = Mathf.Lerp(mapCam.orthographicSize, 350f, Time.deltaTime * mapZoomSpeed);
            }
        }
    }

    public void JumpToSector()
    {
        if(mapSelectedSector.activeSelf == true)
            pm.JumpTo(mapSelectedSector, currentSector);
    }

    public void Shake(float shakeXIN, float shakeYIN, float shakeRotIN)
    {
        shakeX += shakeXIN;
        shakeY += shakeYIN;
        shakeRot += shakeRotIN;
    }
}