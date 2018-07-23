using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManagerScript : MonoBehaviour {

    public BasicEnemyScript bes;

    public void setPlayState(int stateIN)
    {
        bes.playState = stateIN;
    }

}
