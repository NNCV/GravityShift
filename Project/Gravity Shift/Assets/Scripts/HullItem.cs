using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HullItem : EquippableItem
{
    public float maxHullHP = 0;
    public float forwardSpeed = 0;
    public float rotationSpeed = 0;

    public GameObject hullGO;

    public Transform[] blasterTransforms;

    public int currentWeaponNumber = 0;
    public int maxWeaponNumber = 0;
    public int currentNeocortexNumber = 0;
    public int maxNeocortexNumber = 0;
    public int currentShieldsNumber = 0;
    public int maxShieldsNumber = 0;
    public int currentCPUnumber = 0;
    public int maxCPUnumber = 0;

    public int[] eqEnabled = new int[15];
}