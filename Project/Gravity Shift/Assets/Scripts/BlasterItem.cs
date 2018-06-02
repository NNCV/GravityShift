using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BlasterItem : EquippableItem
{
    public string overchargeEffect = "";

    public string blasterDmgType1 = "Kinetic";
    public string blasterDmgType2 = "Thermal";

    public GameObject blasterGO;
    
    public float blasterEnergyDrain = 0;
    public float blasterFireRate = 0;
    public float blasterCooldown = 0;

    public float wep1Value = 0;
    public float wep2Value = 0;
    public float wep3Value = 0;
    public float wep4Value = 0;
}