using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu]
public class DialogueObject : ScriptableObject {

    public Sprite characterImage = null;
    public string characterName = "Unknown";
    public string characterText = "...";
    public float timeToDisappear = 0f;
    public bool isSkippable = false;

}
