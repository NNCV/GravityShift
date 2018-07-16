using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SystemLevelObject : ScriptableObject {

    public string systemName;
    public int systemPlanetCount;
    public SunSector systemCentre;
    public PlanetObject[] systemPlanets;
    public string systemType;
    public int systemOrbitStage;

    public SystemLevelObject()
    {
    }

    public SystemLevelObject(string name)
    {
        systemName = name;
    }

}
