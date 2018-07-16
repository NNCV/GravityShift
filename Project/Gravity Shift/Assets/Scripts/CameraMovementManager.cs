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
    public GameObject selectedSystemInMap;
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
        if (pm.warping == false)
        {
            if (target != null)
            {
                transform.position = target.transform.position + new Vector3(0f, 0f, -10f);
            }
            else
            {
                transform.position = initialPosition;
            }
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
            //removeMapPartsWhenSwitching();
             
            Transform trAct = mapSelectedSystem.transform.parent.transform.parent.Find(mapSelectedSystem.transform.parent.name);

            GameObject[] goAll = GameObject.FindGameObjectsWithTag("MapPart");
            Transform[] trAll = new Transform[goAll.Length];

            Debug.Log(trAct.name);
            
            /*
            for(int i = 5; i < trAll.Length; i++)
            {
                trAll[i] = goAll[i].transform.parent.transform;
                if(trAll[i].GetComponentInChildren<MapDataContainerScript>() != null)
                {
                    //if(trAll[i].GetComponentInChildren<MapDataContainerScript>().slo.systemName == trAct.GetChild(0))
                    if (trAll[i].GetChild(0).GetComponent<MapDataContainerScript>().slo.systemName == trAct.GetChild(0).GetComponent<MapDataContainerScript>().slo.systemName)
                    {
                        Debug.Log(trAll[i].name);
                        Debug.Log(trAll[i].GetChild(0).GetComponent<MapDataContainerScript>().slo.systemName);
                        Debug.Log(i);
                    }
                }
            }
            */




            mapSelectedSystem.GetComponent<MapDataContainerScript>().showPlanets(systemPlanetsScalar, mapSelectedSystem.transform, 1);

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

    //modify this when implementing the jump system
    public void removeMapPartsWhenSwitching()
    {
        //this either deletes all or none
        //right now it deletes none
        //please modify
        foreach (GameObject inGO in GameObject.FindGameObjectsWithTag("MapPart"))
        {
            if (inGO.transform.GetChild(0).GetComponent<MapDataContainerScript>().slo.systemName != mapSelectedSystem.GetComponent<MapDataContainerScript>().slo.systemName)
            {
                Destroy(inGO);
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
        if (pm.warping == false)
        {
            if (selected == false)
            {
                systemGalaxyViewButton.interactable = false;
            }
            else
            {
                systemGalaxyViewButton.interactable = true;
            }

            if (isSystemView == false)
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
                cam.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                mapJumpRangeDisplay.transform.localScale = new Vector3(jumpRange, jumpRange, jumpRange);
                mapJumpRangeDisplay.transform.position = pm.currentPositionMap.position;

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
                    mapTarget.transform.position = Vector3.Lerp(mapTarget.transform.position, mapTarget.transform.position + new Vector3(Input.GetAxisRaw("Horizontal") * mapScalableSpeed, -Input.GetAxisRaw("Vertical") * mapScalableSpeed, 0f), Time.deltaTime * mapTravelSpeed);
                    mapTarget.transform.localScale = Vector3.Lerp(mapTarget.transform.localScale, new Vector3(mapTargetScalableScale, mapTargetScalableScale, mapTargetScalableScale), Time.deltaTime * mapTravelSpeed);

                    mapZoom += Input.GetAxisRaw("Mouse ScrollWheel") / mapZoomDif;
                    mapZoom = Mathf.Clamp(mapZoom, mapZoomMin, mapZoomMax);

                    mapPos = mapTarget.transform.position + new Vector3(0f, 0f, -50f);

                    cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, mapZoom, Time.deltaTime * mapZoomSpeed);
                    cam.transform.position = Vector3.Lerp(cam.transform.position, mapPos, Time.deltaTime * mapTravelSpeed);
                    cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, mapRot, Time.deltaTime * mapTravelSpeed);
                }
                else
                {
                    pmtm.stopSpriteRenderer();


                    if (Vector3.Distance(pm.currentPositionMap.position, mapSelectedSector.transform.position) <= jumpRange)
                    {
                        jumpButton.enabled = true;
                    }

                    if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        currentSector++;
                        if (currentSector >= mapSelectedSystem.GetComponent<MapDataContainerScript>().slo.systemPlanets.Length)
                        {
                            currentSector = 0;
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        currentSector--;
                        if (currentSector < 0)
                        {
                            currentSector = mapSelectedSystem.GetComponent<MapDataContainerScript>().slo.systemPlanets.Length - 1;
                        }
                    }

                    if (currentSector == 0)
                    {
                        mapSelectedSector.transform.position = Vector3.Lerp(mapSelectedSector.transform.position, mapSelectedSystem.transform.position, Time.deltaTime * 25f);

                        mapSelectedSector.transform.localScale = Vector3.Lerp(mapSelectedSector.transform.localScale, new Vector3((systemSectorScalar + mapSelectedSystem.GetComponent<MapDataContainerScript>().slo.systemCentre.sunRadius) * 2.25f, (systemSectorScalar + mapSelectedSystem.GetComponent<MapDataContainerScript>().slo.systemCentre.sunRadius) * 2.25f, (systemSectorScalar + mapSelectedSystem.GetComponent<MapDataContainerScript>().slo.systemCentre.sunRadius) * 2.25f), Time.deltaTime * 25f);
                    }
                    else
                    {
                        mapSelectedSector.transform.position = Vector3.Lerp(mapSelectedSector.transform.position, mapSelectedSystem.transform.GetChild(currentSector - 1).transform.GetChild(0).transform.position, Time.deltaTime * 25f);
                        mapSelectedSector.transform.localScale = Vector3.Lerp(mapSelectedSector.transform.localScale, new Vector3((systemSectorScalar + mapSelectedSystem.GetComponent<MapDataContainerScript>().slo.systemPlanets[currentSector].planetRadius) * 1.25f, (systemSectorScalar + mapSelectedSystem.GetComponent<MapDataContainerScript>().slo.systemPlanets[currentSector].planetRadius) * 1.25f, (systemSectorScalar + mapSelectedSystem.GetComponent<MapDataContainerScript>().slo.systemPlanets[currentSector].planetRadius) * 1.25f), Time.deltaTime * 25f);

                    }

                    mapSelector.transform.localScale = Vector3.Lerp(mapSelector.transform.localScale, new Vector3((mapSelectedSystem.GetComponent<MapDataContainerScript>().slo.systemPlanetCount + 100f) * systemSectorScalar, (mapSelectedSystem.GetComponent<MapDataContainerScript>().slo.systemPlanetCount + 100f) * systemSectorScalar, (mapSelectedSystem.GetComponent<MapDataContainerScript>().slo.systemPlanetCount + 100f) * systemSectorScalar), Time.deltaTime * mapZoomSpeed);


                    cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(mapSelectedSystem.transform.position.x, mapSelectedSystem.transform.position.y, -50f), Time.deltaTime * mapZoomSpeed);
                    cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 700f, Time.deltaTime * mapZoomSpeed);

                }
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