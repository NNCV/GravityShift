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
        GetComponent<SpriteRenderer>().sprite = slo.systemCentre.mapGO.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        transform.localScale = new Vector3(slo.systemCentre.sunRadius, slo.systemCentre.sunRadius, slo.systemCentre.sunRadius);
    //    showPlanets(scalarInside, transform);
    }

    public Vector3 returnPlanetPosition(int planet, float scalar)
    {
        if (slo.systemPlanets[planet] == null)
            return new Vector3(0f, 0f, 0f);
        else
        {
            Vector3 pos = new Vector3(0f, 0f, 0f);
            pos.x = (planet + offsetInside) * scalar;
            return pos;
        }
    }

    public Quaternion returnPlanetRotation(int planet)
    {
        if (slo.systemPlanets[planet] == null)
            return Quaternion.Euler(0f, 0f, 0f);
        else
        {
            Quaternion returnRotation = Quaternion.Euler(0f, 0f, slo.systemPlanets[planet].rot1);
            return returnRotation;
        }
    }

    public void showPlanets(float scalar, Transform origin, int start)
    {
        for (int a = start; a < slo.systemPlanetCount; a++)
        {
            if (slo.systemPlanets[a] != null)
            {
                Vector3 pos = new Vector3(0f, 0f, 0f);
                Quaternion planetRotation = Quaternion.Euler(0f, 0f, slo.systemPlanets[a].rot1);
                pos.x = (a + offsetInside) * scalar;
                
                GameObject pgo = slo.systemPlanets[a].mapGO;
                pgo.transform.GetChild(0).transform.position = pos;
                pgo.transform.rotation = planetRotation;
                pgo.transform.GetChild(0).transform.localScale = new Vector3(slo.systemPlanets[a].planetRadius / origin.localScale.x, slo.systemPlanets[a].planetRadius / origin.localScale.y, slo.systemPlanets[a].planetRadius / origin.localScale.z);

                slo.systemPlanets[a].mapPosition = pos;

                Instantiate(pgo, origin);
            }
        }
    }
}
