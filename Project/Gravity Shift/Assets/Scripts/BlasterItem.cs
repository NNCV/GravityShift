using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BlasterItem : EquippableItem
{
    public string overchargeEffect = "";

    public string blasterDmgType1 = "";
    public string blasterDmgType2 = "";
    //  the following is not yet implemented not because there was no way of doing so, but because at the moment of writing this I don't know what to do with this data, but I knew what just 5 minutes ago
    //  if you see this, then you know I still didn't remember what that was all about.
    //    public BlasterType blasterType;

    public GameObject blasterGO;
    
    public float blasterEnergyDrain = 0;
    public float blasterFireRate = 0;
    public float blasterCooldown = 0;

    public float wep1Value = 0;
    public float wep2Value = 0;
    public float wep3Value = 0;
    public float wep4Value = 0;
}