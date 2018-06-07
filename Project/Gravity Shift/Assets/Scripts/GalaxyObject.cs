using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalaxyObject : ScriptableObject {

    public SystemLevelObject[] systems;
    public string galaxyName;
    public int galaxySystemCount;
    public SystemLevelObject galaxyCenter;

}
