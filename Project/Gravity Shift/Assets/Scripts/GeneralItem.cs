using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class GeneralItem : ScriptableObject {

    public Sprite itemImage;
    public string itemName;
    public string itemDescription;
    public int itemValue;
    public string itemType;
    public Color rarity;

}