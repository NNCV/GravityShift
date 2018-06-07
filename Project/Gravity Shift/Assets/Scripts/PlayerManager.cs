using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {

    public GameObject testEnemy;
    public float eMinX, eMinY;
    public float eMaxX, eMaxY;
    public float eRotMin, eRotMax;

    public GeneralItem[] allItems;
    public HullItem[] hulls;
    public BlasterItem[] blasters;
    public ReactorItem[] reactors;
    public ShieldItem[] shields;
    
    public PlayerEquipmentManager pem;
    public PlayerInventoryManager pim;
    public PlayerMovementManager pmm;
    public LevelManager lm;
    public GameObject starsystem;

    public Image slotSelector;
    public SlotScript selectedSlot;
    public Vector2 slotSelectorOffset;

    public GalaxyObject currentGalaxy = new GalaxyObject();
    public Transform currentPositionMap;
    public SystemLevelObject currentSystem = new SystemLevelObject();
    public int currentSector = new int();
    public int currentSystemLevel = new int();
    public string playerName;

    public int playerLevel = 1;
    public int currentXP = 0;
    public int[] xpLevels = new int[51];

    public bool isInCutscene = false;
    public bool isFrozen = false;

    public float timeMult = 1f;
    public float timeSubMult = 2f;
    public float timeWait = 1f;
    public float timeCurrent = 0f;
    public bool timeStopped = false;
    
    public Animator uiAnim;
    public int uiAnimState;
    public Animator playerAnim;
    public int playerAnimState;
    public GameObject warpBridge;
    public SectorObject warpDestination;

    public float warpTimeCurrent, warpTimeMax;
    public bool warping = false;

    public void JumpTo(GameObject dest, int pl)
    {
        Transform tr = dest.transform;
        currentPositionMap = tr;
        //playerAnimState = 1;
        //Instantiate(warpBridge, transform.position, transform.rotation);
        //warpDestination = dest.GetComponent<MapDataContainerScript>().slo.systemPlanets[pl];
        FreezePlayerFunctions();
        //uiAnim.SetInteger("State", -10);
        warping = true;
        jumpRange = 5000;
    }



    public float jumpRange;

    public int mode = 0;
    
    private void Update()
    {
        if (mode != 0)
        {
            Camera.main.GetComponent<CameraMovementManager>().jumpRange = jumpRange;
            if (timeStopped == true)
            {
                if (timeCurrent >= timeWait)
                {
                    Time.timeScale = Time.timeScale / timeSubMult;
                    if (Time.timeScale <= 0.05f)
                    {
                        Time.timeScale = 0f;
                    }
                }
                else
                {
                    timeCurrent += Time.deltaTime;
                }
            }

            //playerAnim.SetInteger("State", playerAnimState);

            if (warping)
            {
                warpTimeCurrent += Time.deltaTime;
                if (warpTimeCurrent >= warpTimeMax)
                {
                    //uiAnim.SetInteger("State", 0);
                    warpTimeCurrent = 0;
                    warping = false;
                }
            }
        }
    }

    public void Start()
    {
        if (mode != 0)
        {
            Load();
            pem.UpdateShipEquipmentStats();
            lm.loaded = loadSectorAndSystem();
            currentGalaxy = lm.GenerateRandomGalaxy();
            lm.currentGalaxy = currentGalaxy;
        }
    }
    
    public SystemLevelObject loadSectorAndSystem()
    {
        SystemLevelObject slo = new SystemLevelObject("null");

        if (PlayerPrefs.GetString("PlayerSystem") != "")
        {
            slo.systemName = PlayerPrefs.GetString("PlayerSystem");
            slo.systemOrbitStage = PlayerPrefs.GetInt("PlayerSystemOrbitStage");
            slo.systemPlanetCount = PlayerPrefs.GetInt("PlayerSystemPlanetCount");
            slo.systemType = PlayerPrefs.GetString("PlayerSystemType");

            slo.systemCentre = new SunSector();
            slo.systemPlanets = new PlanetObject[slo.systemPlanetCount];

            for (int a = 0; a < slo.systemPlanetCount; a++)
            {
                if (a == 0)
                {
                    slo.systemCentre = lm.GenerateRandomSun();
                    slo.systemCentre.sectorName = slo.systemName;
                    slo.systemCentre.mapGO = lm.sunMapGO;
                    slo.systemCentre.sunRadius = PlayerPrefs.GetFloat("PlayerSystemCenterRadius");
                    slo.systemCentre.sunColor = new Color(PlayerPrefs.GetFloat("PlayerSystemCenterColorR"), PlayerPrefs.GetFloat("PlayerSystemCenterColorG"), PlayerPrefs.GetFloat("PlayerSystemCenterColorB"));
                }
                if (slo.systemPlanets[a] != null)
                {
                    slo.systemPlanets[a] = lm.GenerateRandomPlanet(slo.systemCentre, a);
                    slo.systemPlanets[a].rot1 = PlayerPrefs.GetFloat("PlayerSystemPlanet" + a + "Rot1");
                    slo.systemPlanets[a].rot2 = PlayerPrefs.GetFloat("PlayerSystemPlanet" + a + "Rot1");
                    slo.systemPlanets[a].sectorName = PlayerPrefs.GetString("PlayerSystemPlanet" + a + "Name");
                    slo.systemPlanets[a].sectorType = PlayerPrefs.GetString("PlayerSystemPlanet" + a + "Type");
                    slo.systemPlanets[a].planetType = PlayerPrefs.GetString("PlayerSystemPlanet" + a + "PlanetType");
                    slo.systemPlanets[a].planetMainColor = new Color(PlayerPrefs.GetFloat("PlayerSystemPlanet" + a + "ColorR"), PlayerPrefs.GetFloat("PlayerSystemPlanet" + a + "ColorG"), PlayerPrefs.GetFloat("PlayerSystemPlanet" + a + "ColorB"));

                    int caught = 0;

                    /*
                    for (int c = 0; c < lm.planetTypes.Length; c++)
                    {
                        if (lm.planetTypes[c] == slo.systemPlanets[a].planetType)
                        {
                            caught = c;
                        }
                    }
                    */
                    slo.systemPlanets[a].mapGO = lm.planetsGO[0];
                    slo.systemPlanets[a].planetRadius = PlayerPrefs.GetFloat("PlayerSystemPlanet" + a + "PlanetRadius");
                }
            }
        }

        return slo;
    }

    /*
    public void saveSectorAndSystem()
    {
        PlayerPrefs.SetString("PlayerSystem", currentGalaxy.systems[currentSystem].systemName);
        PlayerPrefs.SetInt("PlayerSystemOrbitStage", currentGalaxy.systems[currentSystem].systemOrbitStage);
        PlayerPrefs.SetInt("PlayerSystemPlanetCount", currentGalaxy.systems[currentSystem].systemPlanetCount);
        PlayerPrefs.SetString("PlayerSystemSystemType", currentGalaxy.systems[currentSystem].systemType);

        for (int b = 0; b < currentGalaxy.systems[currentSystem].systemPlanetCount; b++)
        {
            if (b == 0)
            {
                PlayerPrefs.SetFloat("PlayerSystemCenterRadius", currentGalaxy.systems[currentSystem].systemCentre.sunRadius);
                PlayerPrefs.SetFloat("PlayerSystemCenterColorR", currentGalaxy.systems[currentSystem].systemCentre.sunColor.r);
                PlayerPrefs.SetFloat("PlayerSystemCenterColorG", currentGalaxy.systems[currentSystem].systemCentre.sunColor.g);
                PlayerPrefs.SetFloat("PlayerSystemCenterColorB", currentGalaxy.systems[currentSystem].systemCentre.sunColor.b);
            }
            if (currentGalaxy.systems[currentSystem].systemPlanets[b] != null)
            {
                PlayerPrefs.SetFloat("PlayerSystemPlanet" + b + "Rot1", currentGalaxy.systems[currentSystem].systemPlanets[b].rot1);
                PlayerPrefs.SetFloat("PlayerSystemPlanet" + b + "Rot2", currentGalaxy.systems[currentSystem].systemPlanets[b].rot2);
                PlayerPrefs.SetString("PlayerSystemPlanet" + b + "Name", currentGalaxy.systems[currentSystem].systemPlanets[b].sectorName);
                PlayerPrefs.SetString("PlayerSystemPlanet" + b + "Type", currentGalaxy.systems[currentSystem].systemPlanets[b].sectorType);
                PlayerPrefs.SetString("PlayerSystemPlanet" + b + "PlanetType", currentGalaxy.systems[currentSystem].systemPlanets[b].planetType);
                PlayerPrefs.SetFloat("PlayerSystemPlanet" + b + "ColorR", currentGalaxy.systems[currentSystem].systemPlanets[b].planetMainColor.r);
                PlayerPrefs.SetFloat("PlayerSystemPlanet" + b + "ColorG", currentGalaxy.systems[currentSystem].systemPlanets[b].planetMainColor.g);
                PlayerPrefs.SetFloat("PlayerSystemPlanet" + b + "ColorB", currentGalaxy.systems[currentSystem].systemPlanets[b].planetMainColor.b);
                PlayerPrefs.SetFloat("PlayerSystemPlanet" + b + "PlanetRadius", currentGalaxy.systems[currentSystem].systemPlanets[b].planetRadius);

            }
        }
    }
    */

    public void FreezePlayerFunctions()
    {
        isFrozen = true;
    }

    public void RestorePlayerFunctions()
    {
        isFrozen = false;
    }

    public void stopTime()
    {
        timeStopped = true;
        timeCurrent = 0f;
    }

    public void returnTime()
    {
        Time.timeScale = 1f;
        timeStopped = false;
    }

    public PlayerManager(PlayerEquipmentManager pem, PlayerMovementManager pmm, string name)
    {
        this.pem = pem;
        this.pmm = pmm;
        playerName = name;
    }

    public void SpawnEnemy()
    {
        Vector3 v3 = new Vector3(Random.Range(eMinX, eMaxX), Random.Range(eMinY, eMaxY), 0f);
        Quaternion q = Quaternion.Euler(0f, 0f, Random.Range(eRotMin, eRotMax));
        Instantiate(testEnemy, v3, q);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void passGalaxyToLM()
    {
        lm.currentGalaxy = currentGalaxy;
    }

    public void Load()
    {

        if (PlayerPrefs.GetString("PlayerName") == "")
            return;
        else
        {
            playerName = PlayerPrefs.GetString("PlayerName");

            jumpRange = PlayerPrefs.GetFloat("JumpRange");
            currentSystemLevel = PlayerPrefs.GetInt("PlayerCurrentSystemLevel");

            foreach (HullItem hull in hulls)
            {
                if (hull.itemName == PlayerPrefs.GetString("ShipHull"))
                {
                    pem.currentHull = hull;
                }
            }

            BlasterItem[] blastersa = new BlasterItem[pem.currentHull.maxWeaponNumber];

            for (int i = 0; i < pem.currentHull.maxWeaponNumber; i++)
            {
                foreach (BlasterItem blaster in blasters)
                {
                    if (blaster.itemName == PlayerPrefs.GetString("Blaster" + i))
                    {
                        blastersa[i] = blaster;
                    }
                }
            }

            pem.currentBlasters = blastersa;

            ReactorItem[] reactorsa = new ReactorItem[pem.currentHull.maxNeocortexNumber];
            int energyTotal = 0;
            int energyRechargeTotal = 0;
            int energyRechargeTimeTotal = 0;
            int reactorNo = 0;

            for (int j = 0; j < pem.currentHull.maxNeocortexNumber; j++)
            {
                foreach(ReactorItem reactor in reactors)
                {
                    if (reactor.itemName == PlayerPrefs.GetString("Reactor" + j))
                    {
                        reactorsa[j] = reactor;
                        reactorNo++;
                        energyRechargeTotal += reactor.rechargeRate;
                        energyRechargeTimeTotal += reactor.timeRecharge;
                        energyTotal += reactor.maxEnergy;
                    }
                }
            }

            pem.currentReactors = reactorsa;

            ShieldItem[] shieldsa = new ShieldItem[pem.currentHull.maxShieldsNumber];
            int shieldTotal = 0;
            int shieldRechargeTotal = 0;
            int shieldRechargeTimeTotal = 0;
            int shieldNo = 0;

            for (int z = 0; z < pem.currentHull.maxShieldsNumber; z++)
            {
                foreach(ShieldItem shield in shields)
                {
                    if(shield.itemName == PlayerPrefs.GetString("Shield" + z))
                    {
                        shieldsa[z] = shield;
                        shieldNo++;
                        shieldRechargeTotal += shield.shieldRecharge;
                        shieldRechargeTimeTotal += shield.shieldRechargeRate;
                        shieldTotal += shield.shieldMax;
                    }
                }
            }

            pem.currentShields = shieldsa;


            pem.hullMax = pem.currentHull.maxHullHP;
            
            shieldRechargeTotal = Mathf.RoundToInt(shieldRechargeTotal / shieldNo);
            shieldRechargeTimeTotal = Mathf.RoundToInt(shieldRechargeTimeTotal / shieldNo);
            pem.shieldRecharge = shieldRechargeTotal;
            pem.shieldRechargeRate = shieldRechargeTimeTotal;
            pem.shieldMax = shieldTotal;

            energyRechargeTotal = Mathf.RoundToInt(energyRechargeTotal / reactorNo);
            energyRechargeTimeTotal = Mathf.RoundToInt(energyRechargeTimeTotal / reactorNo);
            pem.energyRecharge = energyRechargeTotal;
            pem.energyRechargeRate = energyRechargeTimeTotal;
            pem.energyMax = energyTotal;

            pmm.fSpeed = pem.currentHull.forwardSpeed;
            pmm.rSpeed = pem.currentHull.rotationSpeed;
            
            currentXP = PlayerPrefs.GetInt("CurrentXP");
            playerLevel = PlayerPrefs.GetInt("CurrentLevel");

            pem.hullCurrent = PlayerPrefs.GetInt("HullCurrent");
            pem.shieldCurrent = PlayerPrefs.GetInt("ShieldCurrent");
            pem.energyCurrent = PlayerPrefs.GetInt("EnergyCurrent");

            pem.DisplayEquipment();
            
            for (int a = 0; a < 6; a++)
            {
                for (int b = 0; b < 8; b++)
                {
                    if (PlayerPrefs.GetString("InventorySlot" + b + "" + a) == "")
                    {
                        pim.slots[b].line[a].itemInSlot = null;
                        pim.slots[b].line[a].DisplayItem();
                    }
                    else
                    {
                        foreach (GeneralItem gi in allItems)
                        {
                            if (gi.itemName == PlayerPrefs.GetString("InventorySlot" + b + "" + a))
                            {
                                pim.slots[b].line[a].itemInSlot = gi;
                                pim.slots[b].line[a].DisplayItem();
                            }
                        }
                    }
                }
            }

           // loadSectorAndSystem();
            
            /*
            currentGalaxy = new GalaxyObject();
            currentGalaxy.galaxyName = PlayerPrefs.GetString("GalaxyName");
            currentGalaxy.galaxySystemCount = PlayerPrefs.GetInt("GalaxySystemsCount");
            currentGalaxy.systems = new SystemLevelObject[500];

            for(int a = 0; a < 500; a++)
            {
                currentGalaxy.systems[a] = new SystemLevelObject();
                currentGalaxy.systems[a].systemCentre = new SunSector();
                currentGalaxy.systems[a].systemPlanets = new PlanetObject[lm.sysPMax];

                if (PlayerPrefs.GetString("SystemName" + a) != "")
                {
                    SystemLevelObject slo = new SystemLevelObject();
                    slo.systemName = PlayerPrefs.GetString("SystemName" + a);
                    slo.systemType = PlayerPrefs.GetString("SystemType" + a);
                    slo.systemPlanetCount = PlayerPrefs.GetInt("SystemPlanetCount" + a);
                    slo.systemPlanets = new PlanetObject[lm.sysPMax];

                    for(int b = 0; b < lm.sysPMax; b++)
                    {
                        slo.systemPlanets = new PlanetObject[lm.sysPMax];
                        if(b == 0)
                        {
                            SunSector ss = new SunSector();
                            ss.sectorName = PlayerPrefs.GetString("SystemCentreSectorName" + a);
                            ss.rot1 = PlayerPrefs.GetFloat("SystemCenterSectorR1" + a);
                            ss.rot2 = PlayerPrefs.GetFloat("SystemCenterSectorR2" + a);
                            ss.sunColor = new Color(PlayerPrefs.GetFloat("SystemCenterSectorColorR" + a), PlayerPrefs.GetFloat("SystemCenterSectorColorG" + a), PlayerPrefs.GetFloat("SystemCenterSectorColorB" + a));
                            ss.mapGO = lm.sunMapGO;
                            ss.sunRadius = PlayerPrefs.GetFloat("SytemCenterSectorMapRadius" + a);
                            slo.systemCentre = ss;
                            slo.systemCentre.mapGO = ss.mapGO;
                            slo.systemCentre.sunRadius = ss.sunRadius;
                            
                        }
                        if(PlayerPrefs.GetString("System" + a + "Sector" + b + "Name") != "")
                        {
                            PlanetObject po = new PlanetObject();
                            po.sectorName = PlayerPrefs.GetString("System" + a + "Sector" + b + "Name");
                            po.rot1 = PlayerPrefs.GetFloat("System" + a + "Sector" + b + "R1");
                            po.rot2 = PlayerPrefs.GetFloat("System" + a + "Sector" + b + "R2");
                            po.planetMainColor = new Color(PlayerPrefs.GetFloat("System" + a + "Sector" + b + "ColorR"), PlayerPrefs.GetFloat("System" + a + "Sector" + b + "ColorG"), PlayerPrefs.GetFloat("System" + a + "Sector" + b + "ColorB"));
                            po.planetType = PlayerPrefs.GetString("System" + a + "Sector" + b + "PlanetType");
                            po.orbitNumber = PlayerPrefs.GetInt("System" + a + "Sector" + b + "OrbitNumber");

                            for(int z = 0; z < lm.planetsGO.Length; z++)
                            {
                                if(po.planetType == lm.planetTypes[z])
                                {
                                    po.mapGO = lm.planetsGO[z];
                                }
                            }

                            po.planetRadius = PlayerPrefs.GetFloat("System" + a + "Sector" + b + "MapRadius");
                            slo.systemPlanets[b] = po;
                        }
                        else
                        {
                            slo.systemPlanets[b] = null;
                        }
                    }

                    currentGalaxy.systems[a] = slo;
                }
                else
                {
                    currentGalaxy.systems[a] = null;
                }
            }
            

            passGalaxyToLM();
            */
        }
    }

    public void Save()
    {
        PlayerPrefs.SetInt("CurrentLevel", playerLevel);
        PlayerPrefs.SetInt("CurrentXP", currentXP);
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.SetString("ShipHull", pem.currentHull.itemName);
        PlayerPrefs.SetFloat("JumpRange", jumpRange);


        PlayerPrefs.SetInt("PlayerCurrentSystemLevel", currentSystemLevel);

        if (pem.currentBlasters != null)
        {
            for (int i = 0; i < pem.currentHull.maxWeaponNumber; i++)
            {
                if(pem.currentBlasters[i] == null)
                {
                    PlayerPrefs.SetString("Blaster" + i, "");
                }
                else
                    PlayerPrefs.SetString("Blaster" + i, pem.currentBlasters[i].itemName);
            }
        }
        if (pem.currentReactors != null)
        {
            for (int j = 0; j < pem.currentHull.maxNeocortexNumber; j++)
            {
                if (pem.currentReactors[j] == null)
                {
                    PlayerPrefs.SetString("Reactor" + j, "");
                }
                else
                    PlayerPrefs.SetString("Reactor" + j, pem.currentReactors[j].itemName);
            }
        }
        if(pem.currentShields != null)
        {
            for(int z = 0; z < pem.currentHull.maxShieldsNumber; z++)
            {
                if (pem.currentShields[z] == null)
                {
                    PlayerPrefs.SetString("Shield" + z, "");
                }
                else
                    PlayerPrefs.SetString("Shield" + z, pem.currentShields[z].itemName);
            }
        }
        
        PlayerPrefs.SetInt("HullCurrent", pem.hullCurrent);
        PlayerPrefs.SetInt("ShieldCurrent", pem.shieldCurrent);
        PlayerPrefs.SetInt("EnergyCurrent", pem.energyCurrent);

        for (int a = 0; a < 6; a++)
        {
            for (int b = 0; b < 8; b++)
            {
                if (pim.slots[b].line[a].itemInSlot == null)
                {
                    PlayerPrefs.SetString("InventorySlot" + b + "" + a, "");
                }
                else
                    PlayerPrefs.SetString("InventorySlot" + b + "" + a, pim.slots[b].line[a].itemInSlot.itemName);
            }
        }

        //SaveGalaxy(currentGalaxy);

    }


    /*for(int b = 0; b < lm.sysPMax; b++)
                    {
                        slo.systemPlanets = new PlanetObject[lm.sysPMax];
                        if(b == 0)
                        {
                            SunSector ss = new SunSector();
                            ss.sectorName = PlayerPrefs.GetString("SystemCentreSectorName" + a);
                            ss.rot1 = PlayerPrefs.GetFloat("SystemCenterSectorR1" + a);
                            ss.rot2 = PlayerPrefs.GetFloat("SystemCenterSectorR2" + a);
                            ss.sunColor.r = PlayerPrefs.GetFloat("SystemCenterSectorColorR" + a);
                            ss.sunColor.g = PlayerPrefs.GetFloat("SystemCenterSectorColorG" + a);
                            ss.sunColor.b = PlayerPrefs.GetFloat("SystemCenterSectorColorB" + a);
                            ss.mapGO = lm.sunMapGO;
                            ss.sunRadius = PlayerPrefs.GetFloat("SytemCenterSectorMapRadius" + a);
                            currentGalaxy.systems[a].systemCentre = ss;
                        }
                        if(PlayerPrefs.GetString("System" + a + "Sector" + b + "Name") != "")
                        {
                            PlanetObject po = new PlanetObject();
                            po.sectorName = PlayerPrefs.GetString("System" + a + "Sector" + b + "Name");
                            po.rot1 = PlayerPrefs.GetFloat("System" + a + "Sector" + b + "R1");
                            po.rot2 = PlayerPrefs.GetFloat("System" + a + "Sector" + b + "R2");
                            po.planetMainColor.r = PlayerPrefs.GetFloat("System" + a + "Sector" + b + "ColorR");
                            po.planetMainColor.g = PlayerPrefs.GetFloat("System" + a + "Sector" + b + "ColorG");
                            po.planetMainColor.b = PlayerPrefs.GetFloat("System" + a + "Sector" + b + "ColorB");
                            po.mapGO = lm.planetsGO[po.planetType.IndexOf(po.planetType)];
                            po.planetRadius = PlayerPrefs.GetFloat("System" + a + "Sector" + b + "MapRadius");
                            slo.systemPlanets[b] = po;
                        }
                        else
                        {
                            slo.systemPlanets[b] = null;
                        }
                    } 
    */
    public void SaveGalaxy(GalaxyObject currentGalaxy)
    {
        //Galaxy Saving

        PlayerPrefs.SetString("GalaxyName", currentGalaxy.galaxyName);
        PlayerPrefs.SetInt("GalaxySystemsCount", currentGalaxy.galaxySystemCount);

        for (int a = lm.galaxySystemMinLY; a < 500; a++)
        {
            if (currentGalaxy.systems[a] != null)
            {
                PlayerPrefs.SetString("SystemName" + a, currentGalaxy.systems[a].systemName);
                PlayerPrefs.SetString("SystemType" + a, currentGalaxy.systems[a].systemType);
                PlayerPrefs.SetInt("SystemPlanetCount" + a, currentGalaxy.systems[a].systemPlanetCount);

                for (int b = 0; b < currentGalaxy.systems[a].systemPlanetCount; b++)
                {
                    if (b == 0)
                    {
                        PlayerPrefs.SetString("SystemCenterSectorName" + a, currentGalaxy.systems[a].systemCentre.sectorName);
                        PlayerPrefs.SetFloat("SystemCenterSectorR1" + a, currentGalaxy.systems[a].systemCentre.rot1);
                        PlayerPrefs.SetFloat("SystemCenterSectorR2" + a, currentGalaxy.systems[a].systemCentre.rot2);
                        PlayerPrefs.SetFloat("SystemCenterSectorColorR" + a, currentGalaxy.systems[a].systemCentre.sunColor.r);
                        PlayerPrefs.SetFloat("SystemCenterSectorColorG" + a, currentGalaxy.systems[a].systemCentre.sunColor.g);
                        PlayerPrefs.SetFloat("SystemCenterSectorColorB" + a, currentGalaxy.systems[a].systemCentre.sunColor.b);
                        PlayerPrefs.SetFloat("SytemCenterSectorMapRadius" + a, currentGalaxy.systems[a].systemCentre.sunRadius);
                    }
                    if (currentGalaxy.systems[a].systemPlanets[b] != null)
                    {
                        PlayerPrefs.SetString("System" + a + "Sector" + b + "Name", currentGalaxy.systems[a].systemPlanets[b].sectorName);
                        PlayerPrefs.SetFloat("System" + a + "Sector" + b + "R1", currentGalaxy.systems[a].systemPlanets[b].rot1);
                        PlayerPrefs.SetFloat("System" + a + "Sector" + b + "R2", currentGalaxy.systems[a].systemPlanets[b].rot2);
                        PlayerPrefs.SetFloat("System" + a + "Sector" + b + "ColorR", currentGalaxy.systems[a].systemPlanets[b].planetMainColor.r);
                        PlayerPrefs.SetFloat("System" + a + "Sector" + b + "ColorG", currentGalaxy.systems[a].systemPlanets[b].planetMainColor.g);
                        PlayerPrefs.SetFloat("System" + a + "Sector" + b + "ColorB", currentGalaxy.systems[a].systemPlanets[b].planetMainColor.b);
                        PlayerPrefs.SetFloat("System" + a + "Sector" + b + "MapRadius", currentGalaxy.systems[a].systemPlanets[b].planetRadius);
                        PlayerPrefs.SetString("System" + a + "Sector" + b + "PlanetType", currentGalaxy.systems[a].systemPlanets[b].planetType);
                        PlayerPrefs.SetInt("System" + a + "Sector" + b + "OrbitNumber",currentGalaxy.systems[a].systemPlanets[b].orbitNumber);
                    }
                    else
                    {
                        PlayerPrefs.SetString("System" + a + "Sector" + b + "Name", "");
                    }
                }
            }
        }
    }

    public void SelectSlot(SlotScript ss)
    {
        if (selectedSlot != null)
        {
            MoveItem(ss);
        }
        else if(ss.itemInSlot == null)
        {
            return;
        }
      //  else if(ss.slotType.Contains("Equippable"))
      //  {
      //      slotSelector.enabled = false;
      //      return;
      //  }
        else
        {
            slotSelector.enabled = true;
            slotSelector.rectTransform.anchoredPosition = ss.GetComponent<RectTransform>().anchoredPosition - slotSelectorOffset;
            selectedSlot = ss;
        }
    }

    public void DeselectSlot()
    {
        selectedSlot = null;
        slotSelector.enabled = false;
    }

    public void DestroyItem(SlotScript ss)
    {
        ss.itemInSlot = null;
        ss.DisplayItem();
    }

    public void MoveItem(SlotScript ss)
    {
        GeneralItem from = selectedSlot.itemInSlot;
        GeneralItem to = ss.itemInSlot;


        string toSlotType = ss.slotType;
        string fromSlotType = selectedSlot.slotType;

        if(fromSlotType == "Inventory")
        {
            if(toSlotType == "Inventory")
            {
                ss.itemInSlot = from;
                selectedSlot.itemInSlot = to;

                selectedSlot.DisplayItem();
                ss.DisplayItem();

                DeselectSlot();
            }
            else if(toSlotType.Contains("Equippable"))
            {
                string fromItemType = selectedSlot.itemInSlot.itemType.Substring(9);

                if (ss.itemInSlot == null)
                {
                    if (from.itemType == ss.slotType)
                    {
                        ss.itemInSlot = from;
                        selectedSlot.itemInSlot = to;

                        selectedSlot.DisplayItem();
                        ss.DisplayItem();

                        DeselectSlot();

                        pem.SaveEquipment();
                        pem.UpdateShipEquipmentStats();
                    }
                }
                else
                {
                    string toItemType = ss.itemInSlot.itemType.Substring(8);

                    if (fromItemType == toItemType)
                    {
                        ss.itemInSlot = from;
                        selectedSlot.itemInSlot = to;

                        selectedSlot.DisplayItem();
                        ss.DisplayItem();

                        DeselectSlot();

                        pem.SaveEquipment();
                        pem.UpdateShipEquipmentStats();
                    }
                    else
                    {
                        DeselectSlot();
                    }
                }
            }
        }
        else if(fromSlotType.Contains("Equippable"))
        {
            if(toSlotType == "Inventory")
            {
                if (ss.itemInSlot == null)
                {
                    ss.itemInSlot = from;
                    selectedSlot.itemInSlot = to;

                    selectedSlot.DisplayItem();
                    ss.DisplayItem();

                    DeselectSlot();
                    
                    pem.SaveEquipment();
                    pem.UpdateShipEquipmentStats();
                }
                else
                {
                    string toItemType = ss.itemInSlot.itemType;
                    string fromItemType = selectedSlot.itemInSlot.itemType;

                    if (fromItemType.Contains("Equippable") == false)
                    {
                        DeselectSlot();
                    }
                    else
                    if (fromItemType == toItemType)
                    {
                        ss.itemInSlot = from;
                        selectedSlot.itemInSlot = to;

                        selectedSlot.DisplayItem();
                        ss.DisplayItem();

                        DeselectSlot();

                        pem.SaveEquipment();
                        pem.UpdateShipEquipmentStats();
                    }
                    else
                    {
                        DeselectSlot();
                    }
                }
            }
            else
            if (toSlotType.Contains("Equippable"))
            {
                string fromItemType = selectedSlot.itemInSlot.itemType.Substring(9);

                if (ss.itemInSlot == null)
                {
                    if (ss.slotType == selectedSlot.itemInSlot.itemType)
                    {
                        ss.itemInSlot = from;
                        selectedSlot.itemInSlot = to;

                        selectedSlot.DisplayItem();
                        ss.DisplayItem();

                        DeselectSlot();

                        pem.SaveEquipment();
                        pem.UpdateShipEquipmentStats();
                    }
                    else
                    {
                        DeselectSlot();
                    }
                }
                else
                {
                    string toItemType = ss.itemInSlot.itemType.Substring(8);

                    if (fromItemType == toItemType)
                    {
                        ss.itemInSlot = from;
                        selectedSlot.itemInSlot = to;

                        selectedSlot.DisplayItem();
                        ss.DisplayItem();

                        DeselectSlot();

                        pem.SaveEquipment();
                        pem.UpdateShipEquipmentStats();
                    }
                    else
                    {
                        DeselectSlot();
                    }
                }
            }
        }
    }
}
