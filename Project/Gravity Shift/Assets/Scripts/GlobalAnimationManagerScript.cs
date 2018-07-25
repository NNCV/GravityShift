using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAnimationManagerScript : MonoBehaviour {
    
    public void clearSectorParts()
    {
        foreach (GameObject oldSector in GameObject.FindGameObjectsWithTag("SectorGOPart"))
        {
            Destroy(oldSector);
        }
        
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }
    }

}
