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
    public GameObject starsystem;

    public Image slotSelector;
    public SlotScript selectedSlot;
    public Vector2 slotSelectorOffset;

    public string playerName;
    public string currentScene;
    public int money;

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

    public IEnumerator TimeToZero()
    {
        timeCurrent += Time.deltaTime;

        if (timeCurrent >= timeWait)
        {
            if (timeMult <= 0.05f)
            {
                timeMult = 0;
                timeStopped = true;
            }
            else
                timeMult = timeMult / timeSubMult;
        }
        
        Time.timeScale = timeMult;

        if (timeStopped == false)
            yield return TimeToZero();
        else yield return null;
    }

    public void Start()
    {
        Load();
        pem.UpdateShipEquipmentStats();
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
        StartCoroutine(TimeToZero());
    }

    public void returnTime()
    {
        Time.timeScale = 1f;
        timeStopped = false;
    }

    public PlayerManager(PlayerEquipmentManager pem, PlayerMovementManager pmm, string name, string scene)
    {
        this.pem = pem;
        this.pmm = pmm;
        currentScene = scene;
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

            currentScene = PlayerPrefs.GetString("CurrentScene");
            money = PlayerPrefs.GetInt("Money");
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
        PlayerPrefs.SetString("CurrentScene", currentScene);
        PlayerPrefs.SetInt("Money", money);
        
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

        ss.itemInSlot = from;
        selectedSlot.itemInSlot = to;

        selectedSlot.DisplayItem();
        ss.DisplayItem();

        DeselectSlot();
    }
}
