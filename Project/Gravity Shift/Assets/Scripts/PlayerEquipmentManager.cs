using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerEquipmentManager : MonoBehaviour
{
    public PlayerManager pm;

    public HullItem currentHull;
    public BlasterItem[] currentBlasters;
    public ReactorItem[] currentReactors;
    public ShieldItem[] currentShields;
    
    //0 = hull
    //1-4 = weapons
    //5-7 = shields
    //8-10 = reactors
    //11-15 = cpus
    public SlotScript[] equipment;

    public int hullCurrent, hullMax;
    public int shieldCurrent, shieldMax, shieldRecharge, shieldRechargeRate;
    public int energyCurrent, energyMax, energyRecharge, energyRechargeRate;

    public float shieldCooldown;
    public float energyCooldown;

    public int weaponSelected = 0;

    public void UpdateShipEquipmentStats()
    {
        int weaponNumber = 0;
        int shieldNumber = 0;
        int reactorNumber = 0;
        equipment[0].itemInSlot = currentHull;
        //equipment[0].DisplayItem();
        for(int a = 1; a < currentHull.eqEnabled.Length; a++)
        {
            if (currentHull.eqEnabled[a] == 1)
            {
                equipment[a].enabled = true;
                if (a <= 4)
                {
                    if (weaponNumber >= currentBlasters.Length)
                        break;
                    else
                    {
                        equipment[a].itemInSlot = currentBlasters[weaponNumber];
                        weaponNumber++;
                    }
                }
                else if (a > 4 && a <= 7)
                {
                    if (shieldNumber >= currentShields.Length)
                        break;
                    else
                    {
                        equipment[a].itemInSlot = currentShields[shieldNumber];
                        shieldNumber++;
                    }
                }
                else if (a > 7 && a <= 10)
                {
                    if (reactorNumber >= currentReactors.Length)
                        break;
                    {
                        equipment[a].itemInSlot = currentReactors[reactorNumber];
                        reactorNumber++;
                    }
                }
                else if (a > 10 && a <= 17)
                {
                    equipment[a].itemInSlot = null;
                }
            }
            else if(currentHull.eqEnabled[a] == 0)
            {
                equipment[a].enabled = false;
            }
        }
    }

    public void Update()
    {
        
        if(Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        if (shieldRecharge > 0)
        {
            shieldCooldown += Time.deltaTime;
            if (shieldCooldown >= shieldRechargeRate)
            {
                shieldCurrent += shieldRecharge;
            }
        }

        energyCooldown += Time.deltaTime;
        if (energyCooldown >= energyRechargeRate)
        {
            energyCurrent += energyRecharge;
        }

        if (shieldCurrent >= shieldMax)
        {
            shieldCurrent = shieldMax;
        }

        if (energyCurrent >= energyMax)
        {
            energyCurrent = energyMax;
        }
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
            if (blaster != null & currentHull.currentWeaponNumber <= currentHull.maxWeaponNumber)
            {
                currentHull.currentWeaponNumber++;
                //Display new blaster at specified transform
                GameObject blasterToSpawn = blaster.blasterGO;
                blasterToSpawn.GetComponent<BlasterScript>().pm = this.pm;
                GameObject.Instantiate(blasterToSpawn, transform.position + currentHull.blasterTransforms[currentHull.currentWeaponNumber - 1].position + new Vector3(0f, 0f, -1f), transform.rotation * currentHull.blasterTransforms[currentHull.currentWeaponNumber - 1].rotation, this.transform);

            }
        }
    }

    public void Fire()
    {
        if (weaponSelected >= currentBlasters.Length)
        {
            weaponSelected = 0;
        }
        if (energyCurrent >= transform.GetChild(weaponSelected + 1).GetComponent<BlasterScript>().blasterEnergyDrain)
        {
            if (transform.GetChild(weaponSelected + 1).GetComponent<BlasterScript>().blasterCooldown >= transform.GetChild(weaponSelected + 1).GetComponent<BlasterScript>().blasterFireRate)
            {
                transform.GetChild(weaponSelected + 1).GetComponent<BlasterScript>().Fire();
                energyCurrent -= transform.GetChild(weaponSelected + 1).GetComponent<BlasterScript>().blasterEnergyDrain;
                energyCooldown = 0;
            }
        }
        weaponSelected++;
    }
}