using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDataContainerScript : MonoBehaviour {

    public Camera cam;
    public SystemLevelObject slo;
    public GameObject orbitRing;
    public float scalarInside;
    public float offsetInside;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "MapPlayerTarget")
        {
            other.GetComponent<PlayerMapTargetManager>().addSystemToList(this.gameObject);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (cam.GetComponent<CameraMovementManager>().mapSelectedSystem == this.gameObject)
                {
                    cam.GetComponent<CameraMovementManager>().mapSelectedSystem = null;
                    cam.GetComponent<CameraMovementManager>().selected = false;
                }
                else
                {
                    cam.GetComponent<CameraMovementManager>().mapSelectedSystem = this.gameObject;
                    cam.GetComponent<CameraMovementManager>().selected = true;
                }
            }
        }
    }

    void Start () {
        cam = Camera.main;
        GetComponent<SpriteRenderer>().color = slo.systemCentre.sunColor;
        orbitRing = slo.orbitRingGO;
        transform.localScale = new Vector3(slo.systemCentre.sunRadius, slo.systemCentre.sunRadius, slo.systemCentre.sunRadius);
    //    showPlanets(scalarInside, transform);
    }

    public Vector3 returnPlanetPosition(int planet, float scalar, Transform origin)
    {
        Vector3 pos;
        pos.x = origin.position.x + (planet + offsetInside) * scalar * slo.systemPlanets[planet].rot1;
        pos.y = origin.position.y + (planet + offsetInside) * scalar * slo.systemPlanets[planet].rot2;
        pos.z = origin.position.z;

        return pos;
    }

    public void showPlanets(float scalar, Transform origin, int start)
    {
        for(int a = start; a < slo.systemPlanetCount; a++)
        {
            Vector3 pos = new Vector3(0f, 0f, 0f);
            pos.x = origin.position.x + (a + offsetInside) * scalar * slo.systemPlanets[a].rot1;
            pos.y = origin.position.y + (a + offsetInside) * scalar * slo.systemPlanets[a].rot2;
            pos.z = origin.position.z;

            GameObject pgo = slo.systemPlanets[a].mapGO;
            pgo.transform.localScale = new Vector3(slo.systemPlanets[a].planetRadius, slo.systemPlanets[a].planetRadius, slo.systemPlanets[a].planetRadius);

            Instantiate(pgo, pos, origin.rotation, origin);
        }
    }
}
