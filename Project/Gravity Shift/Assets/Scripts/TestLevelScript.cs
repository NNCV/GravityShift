using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestLevelScript : MonoBehaviour {

    public Transform player;
    public GameObject testEnemy;
    public SystemLevelObject slo;
    public int planet = 1;
    public float step;
    public Text level;
    public LevelManager lm;
    public float toZoom = 125f;
    public Camera[] cams;
    public int activeCam = 0;
    public GameObject technatoriaTeaser;

    public float innerRadius, outerRadius;

    //test
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, innerRadius);
        Gizmos.DrawWireSphere(transform.position, outerRadius);
    }
    
    //test
    public void spawnEnemy()
    {
        float x = Random.Range(innerRadius, outerRadius);
        float y = Random.Range(innerRadius, outerRadius);

        Vector3 pos = new Vector3(x, y, 0f) + player.position;

        GameObject en = testEnemy;
        en.GetComponentInChildren<BasicEnemyScript>().player = player;

        Instantiate(en, pos, Quaternion.Euler(0f, 0f, 0f));
    }

    //test
    public void killEnemies()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }
    }

    //test
    public void switchZoom()
    {
        if(cams[0].orthographicSize >= 120f)
        {
            toZoom = 8f;
        }
        else if(cams[0].orthographicSize <= 12f)
        {
            toZoom = 125f;
        }
    }

    //test
    private void Start()
    {
        level.text = slo.systemPlanets[planet].sectorName;
    }

    //test
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            FindObjectOfType<AutoTargetWeaponScript>().Fire();  
        }

        if (cams[0].enabled == true)
        {
            cams[0].orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, toZoom, Time.deltaTime * 6f);
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            foreach (GameObject oldSector in GameObject.FindGameObjectsWithTag("SectorGOPart"))
            {
                Destroy(oldSector);
            }

            foreach (Camera c in cams)
            {
                c.gameObject.SetActive(false);
            }
            technatoriaTeaser.SetActive(true);
            cams[1].gameObject.SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            foreach (GameObject oldSector in GameObject.FindGameObjectsWithTag("SectorGOPart"))
            {
                Destroy(oldSector);
            }

            foreach (Camera c in cams)
            {
                c.gameObject.SetActive(false);
            }
            technatoriaTeaser.SetActive(false);
            cams[0].gameObject.SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.Z))
        {
            switchZoom();
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveToSector(1);
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            loadSector();
        }
        else if(Input.GetKeyDown(KeyCode.R))
        {
            SystemLevelObject ayy = new SystemLevelObject();
            ayy = lm.GenerateRandomSystem();
            slo = ayy;
            planet = 0;
            level.text = slo.systemPlanets[planet].sectorName;
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            spawnEnemy();
        }
        else if(Input.GetKeyDown(KeyCode.K))
        {
            killEnemies();
        }
    }

    public void moveToSector(int by)
    {
        planet += by;
        
        if(planet <= 0)
        {
            planet = getLastPlanetPosition();
        }
        else if(planet >= slo.systemPlanets.Length)
        {
            planet = 0;
        }

        level.text = slo.systemPlanets[planet].sectorName;
    }

    public int getLastPlanetPosition()
    {
        for (int a = slo.systemPlanets.Length; a >= 0; a--)
        {
            if (slo.systemPlanets[a] != null)
            {
                return a;
            }
        }
        return 0;
    }

    public void loadSector()
    {
        foreach(GameObject oldSector in GameObject.FindGameObjectsWithTag("SectorGOPart"))
        {
            Destroy(oldSector);
        }

        if (planet == 0)
        {
            GameObject sunGO = slo.systemCentre.sectorGO;

            float size = slo.systemCentre.sunRadius * 0.85f;

            sunGO.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = slo.systemCentre.sunColor;
            sunGO.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().color = slo.systemCentre.sunColor;
            sunGO.transform.GetChild(0).GetChild(0).transform.localScale = new Vector3(size, size, size);
            //sunGO.transform.GetChild(0).GetChild(1).transform.localScale = new Vector3(size * 50f, size * 50f, size * 50f);
            sunGO.transform.GetChild(0).GetChild(1).GetComponent<WarpConduitLensFlareScript>().scaleOrigin = size * 40f;

            Instantiate(sunGO, new Vector3(0f, 0f, 4850f), Quaternion.Euler(0f, 0f, 0f));
        }
        else if (planet > 0)
        {
            GameObject planetGO = slo.systemPlanets[planet].sectorGO;

            float size = ((7 - planet) * slo.systemCentre.sunRadius) / step;

            int planetType = 0;
            
            for(int z = 0; z < lm.planetTypes.Length; z++)
            {
                if(slo.systemPlanets[planet].planetType == lm.planetTypes[z])
                {
                    planetType = z;
                }
            }

            planetGO.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sprite = lm.planetsGO[planetType].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
            planetGO.transform.GetChild(0).GetChild(0).localScale = new Vector3(size / 4f, size / 4f, size / 4f);
            planetGO.transform.GetChild(0).GetChild(1).GetComponent<WarpConduitLensFlareScript>().scaleOrigin = size * 5f;
            planetGO.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = slo.systemCentre.sunColor;
            planetGO.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().color = slo.systemCentre.sunColor;
            planetGO.transform.GetChild(0).transform.rotation = Quaternion.Euler(0f, 0f, 180f + slo.systemPlanets[planet].rot1);
            planetGO.transform.GetChild(0).transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            planetGO.transform.GetChild(1).transform.rotation = Quaternion.Euler(0f, 0f, -slo.systemPlanets[planet].rot1);
            planetGO.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().color = slo.systemPlanets[planet].planetMainColor;
            planetGO.transform.GetChild(1).GetChild(0).transform.localScale = new Vector3(1.45f * slo.systemPlanets[planet].planetRadius, 1.45f * slo.systemPlanets[planet].planetRadius, 1.45f * slo.systemPlanets[planet].planetRadius);

            Instantiate(planetGO, new Vector3(0f, 0f, 4850f), Quaternion.Euler(0f, 0f, 0f));
        }
    }

}
