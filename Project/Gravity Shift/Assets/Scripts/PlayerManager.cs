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

    public GalaxyObject currentGalaxy;
    public int currentSystem;
    public int currentSector;
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

    public int mode = 0;
    
    private void Update()
    {
        if (mode != 0)
        {
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
        }
    }

    public void Start()
    {
        if (mode != 0)
        {
            Load();
            pem.UpdateShipEquipmentStats();
            lm.currentGalaxy = currentGalaxy;
        }
    }

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

    public void Load()
    {

        if (PlayerPrefs.GetString("PlayerName") == "")
            return;
        else
        {
            playerName = PlayerPrefs.GetString("PlayerName");

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
            
            currentGalaxy = new GalaxyObject();
            currentGalaxy.galaxyName = PlayerPrefs.GetString("GalaxyName");
            currentGalaxy.galaxySystemCount = PlayerPrefs.GetInt("GalaxySystemsCount");

            for(int a = 0; a < 500; a++)
            {
                if(PlayerPrefs.GetString("SystemName" + a) != "")
                {
                    SystemLevelObject slo = new SystemLevelObject();
                    slo.systemName = PlayerPrefs.GetString("SystemName" + a);
                    slo.systemType = PlayerPrefs.GetString("SystemType" + a);
                    
                    for(int b = 0; b < lm.sysPMax; b++)
                    {
                        if(b == 0)
                        {
                            SunSector ss = new SunSector();
                            ss.sectorName = PlayerPrefs.GetString("SystemCentreSectorName" + a);
                            ss.rot1 = PlayerPrefs.GetFloat("SystemCenterSectorR1" + a);
                            ss.rot2 = PlayerPrefs.GetFloat("SystemCenterSectorR2" + a);
                            ss.sunColor.r = PlayerPrefs.GetFloat("SystemCenterSectorColorR" + a);
                            ss.sunColor.g = PlayerPrefs.GetFloat("SystemCenterSectorColorG" + a);
                            ss.sunColor.b = PlayerPrefs.GetFloat("SystemCenterSectorColorB" + a);
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
            
        }
    }

    public void Save()
    {
        PlayerPrefs.SetInt("CurrentLevel", playerLevel);
        PlayerPrefs.SetInt("CurrentXP", currentXP);
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.SetString("ShipHull", pem.currentHull.itemName);
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

        SaveGalaxy(currentGalaxy);

    }

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
                
                for (int b = 0; b < lm.sysPMax; b++)
                {
                    if (b == 0)
                    {
                        PlayerPrefs.SetString("SystemCenterSectorName" + a, currentGalaxy.systems[a].systemCentre.sectorName);
                        PlayerPrefs.SetFloat("SystemCenterSectorR1" + a, currentGalaxy.systems[a].systemCentre.rot1);
                        PlayerPrefs.SetFloat("SystemCenterSectorR2" + a, currentGalaxy.systems[a].systemCentre.rot2);
                        PlayerPrefs.SetFloat("SystemCenterSectorColorR" + a, currentGalaxy.systems[a].systemCentre.sunColor.r);
                        PlayerPrefs.SetFloat("SystemCenterSectorColorG" + a, currentGalaxy.systems[a].systemCentre.sunColor.g);
                        PlayerPrefs.SetFloat("SystemCenterSectorColorB" + a, currentGalaxy.systems[a].systemCentre.sunColor.b);
                    }
                    if (currentGalaxy.systems[a].systemPlanets[b] != null)
                    {
                        PlayerPrefs.SetString("System" + a + "Sector" + b + "Name", currentGalaxy.systems[a].systemPlanets[b].sectorName);
                        PlayerPrefs.SetFloat("System" + a + "Sector" + b + "R1", currentGalaxy.systems[a].systemPlanets[b].rot1);
                        PlayerPrefs.SetFloat("System" + a + "Sector" + b + "R2", currentGalaxy.systems[a].systemPlanets[b].rot2);
                        PlayerPrefs.SetFloat("System" + a + "Sector" + b + "ColorR", currentGalaxy.systems[a].systemPlanets[b].planetMainColor.r);
                        PlayerPrefs.SetFloat("System" + a + "Sector" + b + "ColorG", currentGalaxy.systems[a].systemPlanets[b].planetMainColor.g);
                        PlayerPrefs.SetFloat("System" + a + "Sector" + b + "ColorB", currentGalaxy.systems[a].systemPlanets[b].planetMainColor.b);
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
