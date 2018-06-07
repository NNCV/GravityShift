using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class SectorObject : ScriptableObject {
    
    public string sectorName;
    public string sectorType;
    public Sprite sectorMenuImage;
    public ObjectiveObject sectorObjective;
    public GameObject sectorGO;
    public GameObject mapGO;
    public float rot1, rot2;

    public SectorObject()
    {

    }

}
