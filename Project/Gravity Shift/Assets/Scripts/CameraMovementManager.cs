﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovementManager : MonoBehaviour
{
    //Variabile legate de jucator
    public LevelManager lm;
    public Transform mapCenter;
    public PlayerMapTargetManager pmtm;
    public PlayerManager pm;

    //Variabile legate de camera propriu-zisa
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

    //Miscarile finale ale camerei (Dupa ce sunt calculate)
    public Vector3 finalMov;
    public Quaternion finalRot;

    //Perturbarile in miscare cauzate de factori externi
    public float shakeX = 0f;
    public float shakeY = 0f;
    public float shakeRot = 0f;
    public float shakeThreshold = 0.05f;

    //Variabile pentru tranzitia dintre harta si joc
    public bool isInMapScreen = false;
    public float timeCurrent = 0;
    public float timeRate = 0;
    public bool toResetIMS = false;

    //Variabile pentru harta
    public float mapPosXMin, mapPosXMax, mapPosYMin, mapPosYMax;
    public float mapZoom, mapZoomMin, mapZoomMax, mapZoomSpeed, mapZoomDif;
    public Vector3 mapPos;
    public Quaternion mapRot;
    public GameObject mapTarget;
    public GameObject mapSelectedSystem;
    public GameObject mapSelectedSector;
    public GameObject mapSelector;
    public GameObject mapJumpRangeDisplay;
    public GameObject selectedSystemInMap;
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
        //Daca exista jucator, se centreaza pe el
        target = FindObjectOfType<PlayerEquipmentManager>().transform.gameObject;
        cam = GetComponent<Camera>();
        foreach(MapDataContainerScript mds in FindObjectsOfType<MapDataContainerScript>())
        {
            if(mds.slo.systemOrbitStage == pm.currentSystem)
            {
                mapJumpRangeDisplay.transform.position = mds.transform.position;
            }
        }
        if (pm.warping == false || pm.warmingUp == true || pm.tutorialIntroAnimation == true)
        {
            if (target != null)
            {
                transform.position = target.transform.position + new Vector3(0f, 0f, -50f);
            }
            else
            {
                transform.position = initialPosition;
            }
        }
    }

    //Functie pentru butoanele de accesare si iesire din harta
    public void switchMapScreen()
    {
        isInMapScreen = !isInMapScreen;
    }

    //Functie pentru butonul de schimbare a perspectivei in harta
    public void switchSectorGalaxy()
    {
        if(selected == true)
        {
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

    //Pentru a actualiza harta
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
        //Pregatire pentru salt
        if(pm.isDead == false)
        {
            if (pm.warmingUp == true || pm.warping == true || pm.tutorialIntroAnimation)
            {
                cam.transform.position = cam.transform.parent.transform.position + new Vector3(0f, 1f, -50f);
                cam.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
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

                if (isInMapScreen == false)
                {
                    zoom += Input.GetAxisRaw("Mouse ScrollWheel") / zoomDif;
                    zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);

                    if (pm.warping == false)
                    {
                        finalMov = target.transform.position + new Vector3(shakeX, shakeY, -150f);
                        finalRot = Quaternion.Euler(0f, 0f, target.transform.eulerAngles.z + shakeRot);
                    }
                    else
                    {
                        finalMov = new Vector3(0f, 1f, 4873f);
                        finalRot = Quaternion.Euler(0f, 0f, 0f);
                    }
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
                    foreach (MapDataContainerScript mds in FindObjectsOfType<MapDataContainerScript>())
                    {
                        if (mds.slo.systemOrbitStage == pm.currentSystem)
                        {
                            mapJumpRangeDisplay.transform.position = mds.transform.position;
                            mapJumpRangeDisplay.transform.GetChild(0).transform.localPosition = new Vector3(0f, 0f, 0f);
                        }
                    }

                    cam.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

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
                        jumpButton.enabled = true;
                        pmtm.startSpriteRenderer();
                        mapTarget.transform.position = Vector3.Lerp(mapTarget.transform.position, mapTarget.transform.position + new Vector3(Input.GetAxisRaw("Horizontal") * mapScalableSpeed, -Input.GetAxisRaw("Vertical") * mapScalableSpeed, 0f), Time.deltaTime * mapTravelSpeed);
                        mapTarget.transform.localScale = Vector3.Lerp(mapTarget.transform.localScale, new Vector3(mapTargetScalableScale, mapTargetScalableScale, mapTargetScalableScale), Time.deltaTime * mapTravelSpeed);

                        if (mapTarget.transform.position.x >= mapPosXMax)
                        {
                            mapTarget.transform.position = Vector3.Lerp(mapTarget.transform.position, new Vector3(mapPosXMax, mapTarget.transform.position.y, mapTarget.transform.position.z), Time.deltaTime * mapTravelSpeed);
                        }
                        if (mapTarget.transform.position.x <= mapPosXMin)
                        {
                            mapTarget.transform.position = Vector3.Lerp(mapTarget.transform.position, new Vector3(mapPosXMin, mapTarget.transform.position.y, mapTarget.transform.position.z), Time.deltaTime * mapTravelSpeed);
                        }
                        if (mapTarget.transform.position.y >= mapPosYMax)
                        {
                            mapTarget.transform.position = Vector3.Lerp(mapTarget.transform.position, new Vector3(mapTarget.transform.position.x, mapPosYMax, mapTarget.transform.position.z), Time.deltaTime * mapTravelSpeed);
                        }
                        if (mapTarget.transform.position.y <= mapPosYMin)
                        {
                            mapTarget.transform.position = Vector3.Lerp(mapTarget.transform.position, new Vector3(mapTarget.transform.position.x, mapPosYMin, mapTarget.transform.position.z), Time.deltaTime * mapTravelSpeed);
                        }

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

                        /*
                        bool goodSystem = false;

                        for(int a = 0; a < pm.completedSystems.Length; a++)
                        {
                            if (mapSelectedSystem.GetComponent<MapDataContainerScript>().slo.systemOrbitStage == pm.completedSystems[a])
                            {
                                goodSystem = true;
                            }
                        }
                        */

                        Vector2 pCurrent = new Vector2(mapJumpRangeDisplay.transform.position.x, mapJumpRangeDisplay.transform.position.y);
                        Vector2 pThen = new Vector2(mapSelectedSector.transform.position.x, mapSelectedSector.transform.position.y);

                        if (Vector2.Distance(pCurrent, pThen) <= pm.jumpRange * pm.jumpRangeMultiplier) //&& canJump && goodsystem == true
                        {
                            jumpButton.enabled = true;
                            jumpButton.interactable = true;
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
                        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 1100f, Time.deltaTime * mapZoomSpeed);
                    }
                }
            }
        }
    }

    public void JumpToSector()
    {
        if(mapSelectedSector.activeSelf == true)
        {
            pm.currentSystem = mapSelectedSystem.GetComponent<MapDataContainerScript>().slo.systemOrbitStage;
            pm.currentSector = currentSector;
            pm.warmingUp = true;
            switchMapScreen();
        }
    }

    public void Shake(float shakeXIN, float shakeYIN, float shakeRotIN)
    {
        shakeX += shakeXIN;
        shakeY += shakeYIN;
        shakeRot += shakeRotIN;
    }
}