using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {
    
    //Itemele care pot fi obtinute
    public GeneralItem[] allItems;
    public HullItem[] hulls;
    public BlasterItem[] blasters;
    public ReactorItem[] reactors;
    public ShieldItem[] shields;
    
    //Sistemele jucatorului
    public PlayerEquipmentManager pem;
    public PlayerInventoryManager pim;
    public PlayerMovementManager pmm;
    public PlayerUIManager puim;
    public LevelManager lm;
    public ObjectiveManager om;
    public DialogueManager dm;
    
    //Lucruri pentru inventar
    public Image slotSelector;
    public SlotScript selectedSlot;
    public Vector2 slotSelectorOffset;

    //Inamici
    public string[] enemyNames;
    public GameObject[] enemies;
    public int[] currentEnemies;
    public float setMinP, setMaxP;

    //Variabile specifice instantierii nivelelor
    public float step;

    //Atribute ale jucatorului care nu pot influenta generarea nivelelor
    public string playerName;

    //Atribute ale jucatorului care pot influenta generarea nivelelor
    public int playerLevel = 0;
    public int currentXP = 0;
    public int[] xpLevels;
    public int[] completedSystems;
    public GalaxyObject currentGalaxy = new GalaxyObject();
    public Transform currentPositionMap;
    public int currentSystem = 0;
    public int currentSector = 0;
    
    //Atribute speciale pentru animatii si altele (precum cutscene-uri)
    public bool isInCutscene = false;
    public bool isFrozen = false;
    public float timeMult = 1f;
    public float timeSubMult = 2f;
    public float timeWait = 1f;
    public float timeCurrent = 0f;
    public bool timeStopped = false;
    public bool enteredEqScreen = false;

    //Animator global si animatii de salt
    public Animator globalAnim;
    public float warpTimeCurrent, warpTimeMax;
    public bool warping = false;
    public bool warmingUp = false;
    public bool tutorialIntroAnimation = true;
    public GameObject warpConduit;
    public float jumpRange;
    public float jumpRangeMultiplier;
    public bool canJump;
    public float nebulaSectorJumpMultiplierInflunece;
    public int mode = 0;
    public bool isDead = false;

    //Dialoguri
    public DialogueScript levelUpDialogue;
    public DialogueScript enemiesInboundDialogue;
    
    public void loadCompletedSystems()
    {
        string loadedCompletedSystems = PlayerPrefs.GetString("Completed Systems");
        if (loadedCompletedSystems != "")
        {
            loadedCompletedSystems.Trim();
            string[] decompressedCS = loadedCompletedSystems.Split(' ');
            int[] knownCompletedSystems = new int[decompressedCS.Length];

            for (int a = 0; a < decompressedCS.Length; a++)
            {
                int.TryParse(decompressedCS[a], out knownCompletedSystems[a]);
            }

            completedSystems = knownCompletedSystems;
        }
    }

    public void saveCompletedSystems()
    {
        string compressedCS = "";

        if (completedSystems != null)
        {
            foreach (int cSys in completedSystems)
            {
                compressedCS += cSys.ToString() + " ";
            }
        }
        PlayerPrefs.SetString("Completed Systems", compressedCS);
    }

    public void setMapJUmpDisplayStats()
    {
        Camera.main.GetComponent<CameraMovementManager>().mapJumpRangeDisplay.transform.GetChild(0).localScale = new Vector3(jumpRange * jumpRangeMultiplier * 2, jumpRange * jumpRangeMultiplier * 2, jumpRange * jumpRangeMultiplier * 2);
        Camera.main.GetComponent<CameraMovementManager>().mapJumpRangeDisplay.transform.GetChild(1).localScale = new Vector3(jumpRange * jumpRangeMultiplier * 2, jumpRange * jumpRangeMultiplier * 2, jumpRange * jumpRangeMultiplier * 2);
        Camera.main.GetComponent<CameraMovementManager>().mapJumpRangeDisplay.transform.GetChild(0).transform.position = lm.getPositionOfSystem(currentSystem);
        Camera.main.GetComponent<CameraMovementManager>().mapJumpRangeDisplay.transform.rotation = Quaternion.Euler(0f, 0f, currentGalaxy.systems[currentSystem].systemCentre.rot1);
    }

    public void switchEquipmentScreen()
    {
        enteredEqScreen = !enteredEqScreen;
    }
    
    private void FixedUpdate()
    {
        if (mode != 0)
        {
            if (!isDead)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    SceneManager.LoadScene("Testing Stuff");
                }

                if (currentXP >= xpLevels[playerLevel])
                {
                    playerLevel++;
                    currentXP = 0;
                    puim.level.text = playerLevel.ToString();
                    dm.ShowDialogue(levelUpDialogue);
                }

                if (warmingUp == true)
                {
                    transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0f, 0f, 0f), Time.deltaTime * 12f);
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, 0f), Time.deltaTime * 8f);
                    Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 12f, Time.deltaTime * 6f);
                    if (Camera.main.orthographicSize <= 12.05f && (transform.rotation.z <= 0.5f || transform.rotation.z >= -0.5f) && (transform.position.x >= -0.1f || transform.position.x <= 0.1f) && (transform.position.y >= -0.1f || transform.position.y <= 0.1f))
                    {
                        warping = true;
                        if (currentSystem == 499 && currentSector == 5)
                        {
                            globalAnim.SetInteger("State", 3);
                        }
                        else if (currentSystem == 0)
                        {
                            globalAnim.SetInteger("State", 4);
                        }
                        else
                        {
                            globalAnim.SetInteger("State", 1);
                        }
                        warmingUp = false;
                    }
                }
                else if (warping == true)
                {
                    warpTimeCurrent += Time.deltaTime;
                    puim.setJumpSystemInformation(currentSector);
                    transform.localPosition = new Vector3(0f, 0f, 0f);
                    setMapJUmpDisplayStats();
                    if (warpTimeCurrent >= warpTimeMax)
                    {
                        warping = false;
                        loadSector();
                        globalAnim.SetInteger("State", 0);
                        transform.localPosition = new Vector3(0f, 0f, -4750f);
                        warpTimeCurrent = 0;
                    }
                }
                else
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
                    if (enteredEqScreen == true)
                    {
                        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, 0f), Time.deltaTime * 6f);
                        if (transform.rotation.z <= 0.5f || transform.rotation.z >= -0.5f)
                        {
                            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                        }
                    }
                }
            }
            else
            {
                puim.animState = -200;
            }
        }
    }

    public void Start()
    {
        if (mode != 0)
        {
            FullLoad();
        }
    }
    
    public void startWarp()
    {
        warmingUp = true;
        warpTimeCurrent = 0f;
    }

    public void saveGalaxy()
    {
        PlayerPrefs.SetString("Galaxy Name", currentGalaxy.galaxyName);
        PlayerPrefs.SetInt("Galaxy System Count", currentGalaxy.galaxySystemCount);

        for (int a = 0; a < currentGalaxy.systems.Length; a++)
        {
            if (currentGalaxy.systems[a] != null)
            {
                if (currentGalaxy.systems[a].systemName == "null")
                {
                    PlayerPrefs.SetString("System " + a + " Name", "null");
                }
                else
                {
                    PlayerPrefs.SetInt("System " + a + " Orbit Stage", currentGalaxy.systems[a].systemOrbitStage);
                    PlayerPrefs.SetString("System " + a + " Type", currentGalaxy.systems[a].systemType);
                    PlayerPrefs.SetInt("System " + a + " Planet Count", currentGalaxy.systems[a].systemPlanetCount);
                    PlayerPrefs.SetString("System " + a + " Name", currentGalaxy.systems[a].systemName);
                    PlayerPrefs.SetString("System " + a + " Center Type", currentGalaxy.systems[a].systemCentre.sectorType);
                    PlayerPrefs.SetFloat("System " + a + " Center Color R", currentGalaxy.systems[a].systemCentre.sunColor.r);
                    PlayerPrefs.SetFloat("System " + a + " Center Color G", currentGalaxy.systems[a].systemCentre.sunColor.g);
                    PlayerPrefs.SetFloat("System " + a + " Center Color B", currentGalaxy.systems[a].systemCentre.sunColor.b);
                    PlayerPrefs.SetFloat("System " + a + " Center Rotation", currentGalaxy.systems[a].systemCentre.rot1);
                    PlayerPrefs.SetFloat("System " + a + " Center Radius", currentGalaxy.systems[a].systemCentre.sunRadius);
                    
                    for (int b = 1; b < currentGalaxy.systems[a].systemPlanetCount; b++)
                    {
                        PlayerPrefs.SetString("System " + a + " Planet " + b + " Type", currentGalaxy.systems[a].systemPlanets[b].sectorType);
                        PlayerPrefs.SetString("System " + a + " Planet " + b + " PType", currentGalaxy.systems[a].systemPlanets[b].planetType);
                        PlayerPrefs.SetInt("System " + a + " Planet " + b + " Orbit Number", currentGalaxy.systems[a].systemPlanets[b].orbitNumber);
                        PlayerPrefs.SetFloat("System " + a + " Planet " + b + " Radius", currentGalaxy.systems[a].systemPlanets[b].planetRadius);
                        PlayerPrefs.SetFloat("System " + a + " Planet " + b + " Color R", currentGalaxy.systems[a].systemPlanets[b].planetMainColor.r);
                        PlayerPrefs.SetFloat("System " + a + " Planet " + b + " Color G", currentGalaxy.systems[a].systemPlanets[b].planetMainColor.g);
                        PlayerPrefs.SetFloat("System " + a + " Planet " + b + " Color B", currentGalaxy.systems[a].systemPlanets[b].planetMainColor.b);
                        PlayerPrefs.SetFloat("System " + a + " Planet " + b + " Rotation", currentGalaxy.systems[a].systemPlanets[b].rot1);
                    }
                }
            }
        }
    }

    public void loadGalaxy()
    {
        GalaxyObject loadGalaxy = new GalaxyObject();
        loadGalaxy.galaxyName = PlayerPrefs.GetString("Galaxy Name");
        loadGalaxy.galaxySystemCount = PlayerPrefs.GetInt("Galaxy System Count");
        loadGalaxy.systems = new SystemLevelObject[loadGalaxy.galaxySystemCount];

        for(int a = 0; a < loadGalaxy.galaxySystemCount; a++)
        {
            if (PlayerPrefs.GetString("System " + a + " Name") == "null")
            {
                loadGalaxy.systems[a] = new SystemLevelObject("null");
            }
            else
            {
                loadGalaxy.systems[a] = new SystemLevelObject();
                loadGalaxy.systems[a].systemName = PlayerPrefs.GetString("System " + a + " Name");
                loadGalaxy.systems[a].systemOrbitStage = PlayerPrefs.GetInt("System " + a + " Orbit Stage");
                loadGalaxy.systems[a].systemType = PlayerPrefs.GetString("System " + a + " Type");
                loadGalaxy.systems[a].systemPlanetCount = PlayerPrefs.GetInt("System " + a + " Planet Count");

                loadGalaxy.systems[a].systemCentre = new SunSector();
                loadGalaxy.systems[a].systemCentre.sectorName = loadGalaxy.systems[a].systemName;
                loadGalaxy.systems[a].systemCentre.sectorType = PlayerPrefs.GetString("System " + a + " Center Type");

                if (a == 0)
                {
                    loadGalaxy.systems[a].systemCentre.mapGO = lm.saggitarius.systemCentre.mapGO;
                }
                else
                {
                    loadGalaxy.systems[a].systemCentre.mapGO = lm.sunMapGO;
                    loadGalaxy.systems[a].systemCentre.sectorGO = lm.sunGO;
                }

                loadGalaxy.systems[a].systemCentre.sunColor = new Color(PlayerPrefs.GetFloat("System " + a + " Center Color R"), PlayerPrefs.GetFloat("System " + a + " Center Color G"), PlayerPrefs.GetFloat("System " + a + " Center Color B"));
                loadGalaxy.systems[a].systemCentre.rot1 = PlayerPrefs.GetFloat("System " + a + " Center Rotation");
                loadGalaxy.systems[a].systemCentre.sunRadius = PlayerPrefs.GetFloat("System " + a + " Center Radius");

                loadGalaxy.systems[a].systemPlanets = new PlanetObject[loadGalaxy.systems[a].systemPlanetCount];

                loadGalaxy.systems[a].systemPlanets[0] = new SunSector();
                loadGalaxy.systems[a].systemPlanets[0] = loadGalaxy.systems[a].systemCentre;

                for (int b = 1; b < loadGalaxy.systems[a].systemPlanetCount; b++)
                {
                    loadGalaxy.systems[a].systemPlanets[b] = new PlanetObject();
                    loadGalaxy.systems[a].systemPlanets[b].sectorGO = lm.planetsSectorGO[0];
                    loadGalaxy.systems[a].systemPlanets[b].sectorName = loadGalaxy.systems[a].systemName + " - " + b;
                    loadGalaxy.systems[a].systemPlanets[b].sectorType = PlayerPrefs.GetString("System " + a + " Planet " + b + " Type");
                    loadGalaxy.systems[a].systemPlanets[b].orbitNumber = PlayerPrefs.GetInt("System " + a + " Planet " + b + " Orbit Number");
                    loadGalaxy.systems[a].systemPlanets[b].planetRadius = PlayerPrefs.GetFloat("System " + a + " Planet " + b + " Radius");
                    loadGalaxy.systems[a].systemPlanets[b].planetType = PlayerPrefs.GetString("System " + a + " Planet " + b + " PType");

                    int selectedPlanetID = 0;

                    for (int s = 0; s < lm.planetsGO.Length; s++)
                    {
                        if (loadGalaxy.systems[a].systemPlanets[b].planetType == lm.planetTypes[s])
                        {
                            selectedPlanetID = s;
                        }
                    }

                    loadGalaxy.systems[a].systemPlanets[b].mapGO = lm.planetsGO[selectedPlanetID];
                    loadGalaxy.systems[a].systemPlanets[b].planetMainColor = new Color(PlayerPrefs.GetFloat("System " + a + " Planet " + b + " Color R"), PlayerPrefs.GetFloat("System " + a + " Planet " + b + " Color G"), PlayerPrefs.GetFloat("System " + a + " Planet " + b + " Color B"));
                    loadGalaxy.systems[a].systemPlanets[b].rot1 = PlayerPrefs.GetFloat("System " + a + " Planet " + b + " Rotation");
                }
            }
        }

        currentGalaxy = loadGalaxy;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    
    public void LoadGame()
    {
        SceneManager.LoadScene("Game Scene");
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

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void LoadEquipment()
    {
        isDead = false;

        foreach (HullItem hull in hulls)
        {
            if (hull.itemName == PlayerPrefs.GetString("ShipHull"))
            {
                pem.currentHull = hull;
            }
        }

        pem.hullCurrent = pem.currentHull.maxHullHP;

        jumpRange = pem.currentHull.jumpRange;

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
        float energyTotal = 0;
        float energyRechargeTotal = 0;
        float energyRechargeTimeTotal = 0;
        int reactorNo = 0;

        for (int j = 0; j < pem.currentHull.maxNeocortexNumber; j++)
        {
            foreach (ReactorItem reactor in reactors)
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
        float shieldTotal = 0;
        float shieldRechargeTotal = 0;
        float shieldRechargeTimeTotal = 0;
        int shieldNo = 0;

        for (int z = 0; z < pem.currentHull.maxShieldsNumber; z++)
        {
            foreach (ShieldItem shield in shields)
            {
                if (shield.itemName == PlayerPrefs.GetString("Shield" + z))
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

        pem.hullCurrent = PlayerPrefs.GetFloat("HullCurrent");
        pem.shieldCurrent = PlayerPrefs.GetFloat("ShieldCurrent");
        pem.energyCurrent = PlayerPrefs.GetFloat("EnergyCurrent");

        pem.DisplayEquipment();
    }

    public void LoadInventory()
    {
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
    }

    public void LoadPlayerInfo()
    {
        playerLevel = PlayerPrefs.GetInt("CurrentLevel");
        currentXP = PlayerPrefs.GetInt("CurrentXP");
        playerName = PlayerPrefs.GetString("PlayerName");
        bool.TryParse(PlayerPrefs.GetString("CanJump"), out canJump);
        currentSystem = PlayerPrefs.GetInt("CurrentSystem");
        currentSector = PlayerPrefs.GetInt("CurrentSector");

        puim.level.text = playerLevel.ToString();

        loadCompletedSystems();
    }

    public void Load()
    {
        LoadPlayerInfo();
        LoadEquipment();
        LoadInventory();
    }

    public void FullLoad()
    {
        if (PlayerPrefs.GetString("PlayerName") == "")
        {
            return;
        }

        LoadPlayerInfo();
        LoadEquipment();
        LoadInventory();
        loadGalaxy();

        if (currentSystem == 499 && currentSector == 5)
        {
            warmingUp = false;
            warping = false;
            isFrozen = true;
            isInCutscene = true;
            tutorialIntroAnimation = true;
            globalAnim.SetInteger("State", -1);
        }
        else
        {
            warmingUp = true;
        }

        puim.level.text = playerLevel.ToString();
        pem.UpdateShipEquipmentStats();

        for (int a = 0; a < pem.currentBlasters.Length; a++)
        {
            transform.GetChild(a + 1).transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        }

        setMapJUmpDisplayStats();
    }

    public void SaveEquipment()
    {
        PlayerPrefs.SetString("ShipHull", pem.currentHull.itemName);

        if (pem.currentBlasters != null)
        {
            for (int i = 0; i < pem.currentHull.maxWeaponNumber; i++)
            {
                if (pem.currentBlasters[i] == null)
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
        if (pem.currentShields != null)
        {
            for (int z = 0; z < pem.currentHull.maxShieldsNumber; z++)
            {
                if (pem.currentShields[z] == null)
                {
                    PlayerPrefs.SetString("Shield" + z, "");
                }
                else
                    PlayerPrefs.SetString("Shield" + z, pem.currentShields[z].itemName);
            }
        }

        PlayerPrefs.SetFloat("HullCurrent", pem.hullCurrent);
        PlayerPrefs.SetFloat("ShieldCurrent", pem.shieldCurrent);
        PlayerPrefs.SetFloat("EnergyCurrent", pem.energyCurrent);

    }

    public void SaveInventory()
    {
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
    }

    public void SavePlayerInfo()
    {
        PlayerPrefs.SetInt("CurrentLevel", playerLevel);
        PlayerPrefs.SetInt("CurrentXP", currentXP);
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.SetString("CanJump", canJump.ToString());
        PlayerPrefs.SetInt("CurrentSystem", currentSystem);
        PlayerPrefs.SetInt("CurrentSector", currentSector);

        saveCompletedSystems();
    }

    public void Save()
    {
        SavePlayerInfo();
        if (puim.transform.GetChild(0).transform.GetChild(1).gameObject.activeInHierarchy == true)
        {
            pem.SaveEquipment();
        }
        SaveEquipment();
        SaveInventory();
    }
    
    public void FullSave()
    {
        SavePlayerInfo();
        SaveEquipment();
        SaveInventory();
        saveGalaxy();
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
        else
        {
            if (ss.slotType.Contains("Equippable"))
            {
                slotSelector.enabled = false;
            }
            else
            {
                slotSelector.enabled = true;
                slotSelector.rectTransform.anchoredPosition = ss.GetComponent<RectTransform>().anchoredPosition - slotSelectorOffset;
            }
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
                    }
                    else
                    {
                        DeselectSlot();
                    }
                }
            }
        }

     
        pem.SaveEquipment();

    }

    public ObjectiveObject GenerateRandomSectorObjective(int objectiveType, int enemyType = 0, int enemyID = 0)
    {
        switch (objectiveType)
        {
            case 0:
                AssasinationObjective ao = new AssasinationObjective();
                ao.enemyID = enemyID;
                ao.objectiveProgress = 0;
                ao.enemyID = currentEnemies[enemyType] - 1;
                ao.objectiveRequired = 1;
                ao.objectiveText = "assasinate the " + enemyNames[enemyType] + " target";
                ao.objectiveImage = om.abrarum;
                ao.objectiveDoneText = "assasination completed, here's what we found";
                ao.objectiveXP = Mathf.RoundToInt((enemies[enemyType].GetComponentInChildren<BasicEnemyScript>().xpReward / 2) * (playerLevel + 0.5f));
                return ao;
        }
        return null;
    }

    public void loadSector()
    {
        canJump = false;
        tutorialIntroAnimation = false;

        pem.hullCurrent = pem.hullMax * pem.hullMultiplier;
        pem.shieldCurrent = pem.shieldMax * pem.shieldMultiplier;
        pem.energyCurrent = pem.energyMax * pem.energyMultiplier;

        foreach(ObjectiveObject oExtra in om.currentObjectives)
        {
            if(oExtra.isExtra == true)
            {
                om.currentObjectives.Remove(oExtra);
            }
        }

        foreach(GameObject enemy in enemies)
        {
            enemy.GetComponentInChildren<BasicEnemyScript>().enemyID = 0;
        }

        int total = Random.Range(1, 6);

        currentEnemies = new int[enemies.Length];
        currentEnemies.Initialize();

        GameObject fe = enemies[0];

        fe.GetComponentInChildren<BasicEnemyScript>().enemyID = currentEnemies[fe.GetComponentInChildren<BasicEnemyScript>().enemyTypeID];

        float x1 = Random.Range(setMinP, setMaxP);
        float y1 = Random.Range(setMinP, setMaxP);

        if ((int)Random.Range(0, 1) == 0)
        {
            x1 = -x1;
        }
        if ((int)Random.Range(0, 1) == 0)
        {
            y1 = -y1;
        }

        Vector3 pos1 = new Vector3(x1, y1, -4750f);

        fe.GetComponentInChildren<BasicEnemyScript>().player = transform;

        Instantiate(fe, pos1, Quaternion.Euler(0f, 0f, 0f));

        currentEnemies[fe.GetComponentInChildren<BasicEnemyScript>().enemyTypeID]++;

        for (int a = 0; a < total; a++)
        {
            foreach (GameObject en in enemies)
            {
                GameObject e = en;
                float chance = e.GetComponentInChildren<BasicEnemyScript>().chanceToSpawn;
                float generatedChance = Random.Range(0f, 1f);
                
                if (generatedChance >= chance)
                {
                    e.GetComponentInChildren<BasicEnemyScript>().enemyID = currentEnemies[e.GetComponentInChildren<BasicEnemyScript>().enemyTypeID];
                    
                    float x = Random.Range(setMinP, setMaxP);
                    float y = Random.Range(setMinP, setMaxP);

                    if ((int)Random.Range(0, 1) == 0)
                    {
                        x = -x;
                    }
                    if ((int)Random.Range(0, 1) == 0)
                    {
                        y = -y;
                    }

                    Vector3 pos = new Vector3(x, y, -4750f);

                    e.GetComponentInChildren<BasicEnemyScript>().player = transform;

                    Instantiate(e, pos, Quaternion.Euler(0f, 0f, 0f));

                    currentEnemies[e.GetComponentInChildren<BasicEnemyScript>().enemyTypeID]++;
                }
            }
        }

        //om.currentObjectives.Add(GenerateRandomSectorObjective(0, Random.Range(0, currentEnemies.Length - 1), currentEnemies[Random.Range(0, currentEnemies.Length - 1)]));
        om.currentObjectives.Add(GenerateRandomSectorObjective(0, 0, 0));
        //om.currentObjectives.Add(GenerateRandomSectorObjective(0, Random.Range(0, enemies.Length - 1), currentEnemies[Random.Range(0, currentEnemies[Random.Range(0, enemies.Length - 1)] - 1)] - 1));

        Save();

        isFrozen = false;

        if(currentGalaxy.systems[currentSystem].systemType.Contains("Nebula"))
        {
            jumpRangeMultiplier = nebulaSectorJumpMultiplierInflunece;
        }

        if (currentSector == 0)
        {
            GameObject sunGO = currentGalaxy.systems[currentSystem].systemCentre.sectorGO;

            float size = currentGalaxy.systems[currentSystem].systemCentre.sunRadius * 0.85f;

            sunGO.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = currentGalaxy.systems[currentSystem].systemCentre.sunColor;
            sunGO.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().color = currentGalaxy.systems[currentSystem].systemCentre.sunColor;
            sunGO.transform.GetChild(0).GetChild(0).transform.localScale = new Vector3(size, size, size);
            sunGO.transform.GetChild(0).GetChild(1).GetComponent<WarpConduitLensFlareScript>().scaleOrigin = size * 40f;

            Instantiate(sunGO, new Vector3(0f, 0f, -4850f), Quaternion.Euler(0f, 0f, 0f));
        }
        else if (currentSector > 0)
        {
            GameObject planetGO = currentGalaxy.systems[currentSystem].systemPlanets[currentSector].sectorGO;

            float size = ((7 - currentSector) * currentGalaxy.systems[currentSystem].systemCentre.sunRadius) / step;

            int planetType = 0;

            for (int z = 0; z < lm.planetTypes.Length; z++)
            {
                if (currentGalaxy.systems[currentSystem].systemPlanets[currentSector].planetType == lm.planetTypes[z])
                {
                    planetType = z;
                }
            }

            planetGO.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sprite = lm.planetsGO[planetType].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
            planetGO.transform.GetChild(0).GetChild(0).localScale = new Vector3(size / 4f, size / 4f, size / 4f);
            planetGO.transform.GetChild(0).GetChild(1).GetComponent<WarpConduitLensFlareScript>().scaleOrigin = size * 5f;
            planetGO.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = currentGalaxy.systems[currentSystem].systemCentre.sunColor;
            planetGO.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().color = currentGalaxy.systems[currentSystem].systemCentre.sunColor;
            planetGO.transform.GetChild(0).transform.rotation = Quaternion.Euler(0f, 0f, 180f + currentGalaxy.systems[currentSystem].systemPlanets[currentSector].rot1);
            planetGO.transform.GetChild(0).transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            planetGO.transform.GetChild(1).transform.rotation = Quaternion.Euler(0f, 0f, -currentGalaxy.systems[currentSystem].systemPlanets[currentSector].rot1);
            planetGO.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().color = currentGalaxy.systems[currentSystem].systemPlanets[currentSector].planetMainColor;
            planetGO.transform.GetChild(1).GetChild(0).transform.localScale = new Vector3(1.45f * currentGalaxy.systems[currentSystem].systemPlanets[currentSector].planetRadius, 1.45f * currentGalaxy.systems[currentSystem].systemPlanets[currentSector].planetRadius, 1.45f * currentGalaxy.systems[currentSystem].systemPlanets[currentSector].planetRadius);

            Instantiate(planetGO, new Vector3(0f, 0f, -4850f), Quaternion.Euler(0f, 0f, 0f));
        }
    }
}
