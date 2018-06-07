using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMapTargetManager : MonoBehaviour {
    
    public PlayerUIManager puim;
    public List<SystemLevelObject> slos;
    public List<GameObject> gos;

    public void addSystemToList(GameObject go)
    {
        //puim.setGalaxyViewStats(slo);
        slos.Add(go.GetComponent<MapDataContainerScript>().slo);
        gos.Add(go);
    }

    public void stopSpriteRenderer()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void startSpriteRenderer()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }

    private void FixedUpdate()
    {
        if(slos.Count > 0)
        {
            puim.setGalaxyViewStats(slos[0]);
            gos.Clear();
            slos.Clear();
        }
        else
        {
            puim.disableGalaxyViewStats();
        }
        
    }

}
