using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class JumpObjective : ObjectiveObject {

    public float system, sector;

    public override void checkIfComplete()
    {
        if(FindObjectOfType<PlayerManager>().currentSystem == system)
        {
            if(FindObjectOfType<PlayerManager>().currentSector == sector)
            {
                objectiveDone = true;
            }
        }
    }

}
