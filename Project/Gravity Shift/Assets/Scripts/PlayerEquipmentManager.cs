using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerEquipmentManager : MonoBehaviour
{
    public PlayerManager pm;

    public HullItem currentHull;
    public BlasterItem[] currentBlasters;
    public ReactorItem[] currentReactors;
    public ShieldItem[] currentShields;

    public GameObject nothing;

    public float hullMultiplier, shieldMultiplier, energyMultiplier, energyRechargeMultiplier;

    //0 = hull
    //1-4 = weapons
    //5-7 = shields
    //8-10 = reactors
    //11-15 = cpus
    public SlotScript[] equipment;

    public float hullCurrent = 1, hullMax = 1;
    public float shieldCurrent, shieldMax, shieldRecharge, shieldRechargeRate;
    public float energyCurrent, energyMax, energyRecharge, energyRechargeRate;

    public float shieldCooldown;
    public float energyCooldown;

    public float hydrSpeedMult;
    public float hydrCost;
    public float hydrTimeCurrent;
    public float hydrTimeMax;
    public bool isHyperDrifting = false;
    public DialogueScript enterHydr, exitHydr, isInHydr;
    public DialogueScript tutorial;
    public DialogueManager dm;

    public float[] weaponReload;
    public int weaponSelected = 0;

    public GameObject deathExplosion;

    public void SaveEquipment()
    {
        int currentWeaponNumber = 0;
        int currentShieldNumber = 0;
        int currentReactorNumber = 0;

        currentHull = (HullItem)equipment[0].itemInSlot;

        for (int a = 0; a < 6; a++)
        {
            if(equipment[a + 1].enabled == true)
            {
                if (equipment[a + 1].itemInSlot == null)
                {
                    currentBlasters[currentWeaponNumber] = null;
                }
                else
                {
                    currentBlasters[currentWeaponNumber] = (BlasterItem)equipment[a + 1].itemInSlot;
                }

                currentWeaponNumber++;
            }
        }

        for (int b = 0; b < 3; b++)
        {
            if (equipment[b + 7].enabled == true)
            {
                if (equipment[b + 7].itemInSlot == null)
                {
                    currentShields[currentShieldNumber] = null;
                }
                else
                {
                    currentShields[currentShieldNumber] = (ShieldItem)equipment[b + 7].itemInSlot;
                }
                currentShieldNumber++;
            }
        }

        for (int c = 0; c < 3; c++)
        {
            if (equipment[c + 10].enabled == true)
            {
                if (equipment[c + 10].itemInSlot == null)
                {
                    currentReactors[currentReactorNumber] = null;
                }
                else
                {
                    currentReactors[currentReactorNumber] = (ReactorItem)equipment[c + 10].itemInSlot;
                }
                currentReactorNumber++;
            }
        }
    }

    public void UpdateShipEquipmentStats()
    {
        equipment[0].itemInSlot = currentHull;
        equipment[0].DisplayItem();

        //weapons
        for (int a = 0; a < 6 ; a++)
        {
            if (currentHull.eqEnabled[a + 1] == 1)
            {
                equipment[a + 1].enabled = true;
                if (a < currentHull.maxWeaponNumber)
                {
                    if (currentBlasters[a] != null)
                    {
                        equipment[a + 1].itemInSlot = currentBlasters[a];
                    }
                }
                equipment[a + 1].DisplayItem();
            }
            else
            {
                equipment[a + 1].itemInSlot = null;
                equipment[a + 1].DisableChildren();
            }
        }

        //shields
        for (int b = 0; b < 3 ; b++)
        {
            if (currentHull.eqEnabled[b + 7] == 1)
            {
                equipment[b + 7].enabled = true;
                if (b < currentHull.maxShieldsNumber)
                {
                    if (currentShields[b] != null)
                        equipment[b + 7].itemInSlot = currentShields[b];
                }
                equipment[b + 7].DisplayItem();
            }
            else
            {
                equipment[b + 7].itemInSlot = null;
                equipment[b + 7].DisableChildren();
            }
        }

        //reactors
        for (int b = 0; b < 3 ; b++)
        {
            if (currentHull.eqEnabled[b + 10] == 1)
            {
                equipment[b + 10].enabled = true;
                if (b < currentHull.maxNeocortexNumber)
                {
                    if (currentReactors[b] != null)
                        equipment[b + 10].itemInSlot = currentReactors[b];
                }
                equipment[b + 10].DisplayItem();
            }
            else
            {
                equipment[b + 10].itemInSlot = null;
                equipment[b + 10].DisableChildren();
            }
        }

        weaponReload = new float[currentBlasters.Length];
            
        for (int z = 0; z < currentBlasters.Length; z++)
        {
            weaponReload[z] = currentBlasters[z].blasterCooldown;
        }

        /*
        for (int b = 0; b < currentHull.maxCPUnumber; b++)
        {
            equipment[b + 13].enabled = true;
            if ([b] != null)
                equipment[b + 13].itemInSlot = currentReactors[b];
            equipment[b + 13].DisplayItem();
        }
        */

        /*for(int a = 1; a < currentHull.eqEnabled.Length; a++)
        {
            if (currentHull.eqEnabled[a] == 1)
            {
                equipment[a].enabled = true;
                if (a <= 6)
                {
                    if (weaponNumber >= currentBlasters.Length)
                        break;
                    else
                    {
                        equipment[a].itemInSlot = currentBlasters[weaponNumber];
                        weaponNumber++;
                    }
                }
                else if (a > 6 && a <= 9)
                {
                    if (shieldNumber >= currentShields.Length)
                        break;
                    else
                    {
                        equipment[a].itemInSlot = currentShields[shieldNumber];
                        shieldNumber++;
                    }
                }
                else if (a > 9 && a <= 12)
                {
                    if (reactorNumber >= currentReactors.Length)
                        break;
                    {
                        equipment[a].itemInSlot = currentReactors[reactorNumber];
                        reactorNumber++;
                    }
                }
                else if (a > 12 && a <= 17)
                {
                    equipment[a].itemInSlot = null;
                }
            }
            else if(currentHull.eqEnabled[a] == 0)
            {
                equipment[a].enabled = false;
                equipment[a].DisableChildren();
            }
            
        }*/
    }

    public void FixedUpdate()
    {
        if (pm.timeStopped == false && pm.warping == false && pm.warmingUp == false && pm.tutorialIntroAnimation == false)
        {
            if (pm.isDead == false)
            {
                hydrTimeCurrent += Time.deltaTime;

                for (int z = 0; z < currentBlasters.Length; z++)
                {
                    weaponReload[z] += Time.deltaTime;
                }

                if (isHyperDrifting == true)
                {
                    transform.GetComponent<PlayerMovementManager>().hydrSpeed = hydrSpeedMult;
                    energyCurrent -= hydrCost;
                }
                else
                {
                    transform.GetComponent<PlayerMovementManager>().hydrSpeed = 1f;
                }

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    ToggleHyperDrift();
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (pm.warping == false)
                    {
                        Fire();
                    }
                }

                if (shieldRecharge > 0)
                {
                    shieldCooldown += Time.deltaTime;
                    if (shieldCooldown >= shieldRechargeRate)
                    {
                        shieldCurrent += shieldRecharge * shieldRechargeRate;
                    }
                }

                energyCooldown += Time.deltaTime;
                if (energyCooldown >= energyRechargeRate)
                {
                    energyCurrent += energyRecharge * energyRechargeMultiplier;
                }

                if (shieldCurrent >= shieldMax)
                {
                    shieldCurrent = shieldMax;
                }

                if (energyCurrent >= energyMax)
                {
                    energyCurrent = energyMax;
                }

                if (energyCurrent <= 0)
                {
                    isHyperDrifting = false;
                    hydrTimeCurrent = 0;
                }

                if (hullCurrent <= 0)
                {
                    hullCurrent = 0;
                    ExplodePlayer();
                }
            }
        }
    }

    public void ExplodePlayer()
    {
        pm.isDead = true;
        for(int a = transform.childCount - 1; a >= 0; a--)
        {
            Destroy(transform.GetChild(a).gameObject);
        }
        Instantiate(deathExplosion, transform.position, transform.rotation);
    }

    public void DisplayEquipment()
    {
        //Destroy Existing/Old equipment that is still being displayed.
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }

        //Display new hull
        GameObject.Instantiate(currentHull.hullGO, this.transform.position, this.transform.rotation, this.transform);

        //Reset CurrentWeaponNumber Timer
        currentHull.currentWeaponNumber = 0;

        //Recalibrate CurrentWeaponNumber Timer and check for bugs
        foreach (BlasterItem blaster in currentBlasters)
        {
            if (blaster == null)
            {
                currentHull.currentWeaponNumber++;
                GameObject.Instantiate(nothing, transform.position + currentHull.blasterTransforms[currentHull.currentWeaponNumber - 1].position + new Vector3(0f, 0f, -0.0001f), transform.rotation * currentHull.blasterTransforms[currentHull.currentWeaponNumber - 1].rotation, this.transform);
            }
            else
            if (currentHull.currentWeaponNumber <= currentHull.maxWeaponNumber)
            {
                currentHull.currentWeaponNumber++;
                //Display new blaster at specified transform
                GameObject blasterToSpawn = blaster.blasterGO;
                blasterToSpawn.GetComponentInChildren<BlasterScript>().pm = this.pm;
                GameObject.Instantiate(blasterToSpawn, transform.position + currentHull.blasterTransforms[currentHull.currentWeaponNumber - 1].position + new Vector3(0f, 0f, -0.0001f), transform.rotation * currentHull.blasterTransforms[currentHull.currentWeaponNumber - 1].rotation, this.transform);
            }
        }
    }

    public void showTutorialDialogue()
    {
        dm.ShowDialogue(tutorial);
    }

    public void ToggleHyperDrift()
    {
        if(hydrTimeCurrent >= hydrTimeMax)
        {
            isHyperDrifting = !isHyperDrifting;
            hydrTimeCurrent = 0;

            if(isHyperDrifting == true)
            {
                dm.ShowDialogue(enterHydr);
            }
            else
            {
                dm.ShowDialogue(exitHydr);
            }

        }
        else
        {
            dm.ShowDialogue(isInHydr);
        }

    }

    public void Fire()
    {
        if (weaponSelected >= currentBlasters.Length)
        {
            weaponSelected = 0;
        }
        if (currentBlasters[weaponSelected] == null)
        {
            weaponSelected++;
        }
        if (energyCurrent >= transform.GetChild(weaponSelected + 1).GetComponentInChildren<BlasterScript>().blasterEnergyDrain)
        {
            if(weaponReload[weaponSelected] >= currentBlasters[weaponSelected].blasterFireRate)
            {
                weaponReload[weaponSelected] = 0f;
                transform.GetChild(weaponSelected + 1).GetComponentInChildren<BlasterScript>().Fire();
                energyCurrent -= transform.GetChild(weaponSelected + 1).GetComponentInChildren<BlasterScript>().blasterEnergyDrain;
                energyCooldown = 0;
            }
        }
        weaponSelected++;
    }

    public void TakeDamage(float damage)
    {
        if(shieldCurrent > 0)
        {
            if (shieldCurrent >= damage)
            {
                shieldCurrent -= damage;
            }
            else
            {
                float damageToHull = damage - shieldCurrent;
                shieldCurrent -= damage - damageToHull;
                hullCurrent -= damageToHull;
            }
        }
        else
        {
            hullCurrent -= damage;
        }

        shieldCooldown = 0;
    }
}