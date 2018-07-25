using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LevelManager : MonoBehaviour {

    public float sunMinR, sunMaxR, planetMinR, planetMaxR;
    public float sunMinCR, sunMinCG, sunMinCB, sunMaxCR, sunMaxCG, sunMaxCB;
    public int galaxySystemFrequency = 8;
    public int galaxySystemMinReq = 40;
    public int galaxySystemMinLY = 35;
    public int galaxySystemFreqBias = 0;
    public int nebulaChance, nebulaBias;
    public int sysPMin, sysPMax;

    public float systemLayerDivider;
    public float scalar = 10f;
    public float scalarInside = 10f;
    public float offsetInside = 1f;

    public GameObject sunGO;
    public GameObject sunMapGO;
    public GameObject[] planetsGO;
    public GameObject[] planetsSectorGO;
    public GameObject orbitRingGO;

    public SystemLevelObject saggitarius;
    public SystemLevelObject tutorial;
    public SystemLevelObject loaded;

    public string[] starNames;
    public string[] planetTypes;

    public ObjectiveObject[] possibleObjectives;

    public SystemLevelObject[] levels;

    public PlayerManager pm;

    public void GenerateGalaxyMap(Transform origin)
    {
        for (int a = 0 ; a < pm.currentGalaxy.galaxySystemCount; a++)
        {
            if (pm.currentGalaxy.systems[a] != null)
            {
                if (pm.currentGalaxy.systems[a].systemName != "null")
                {
                    GameObject mgo = pm.currentGalaxy.systems[a].systemCentre.mapGO;
                    mgo.transform.GetChild(0).GetComponent<MapDataContainerScript>().slo = pm.currentGalaxy.systems[a];
                    mgo.transform.GetChild(0).GetComponent<MapDataContainerScript>().scalarInside = scalarInside;
                    mgo.transform.GetChild(0).GetComponent<MapDataContainerScript>().offsetInside = offsetInside;

                    Vector3 pos = new Vector3(0f, 0f, 0f);
                    Quaternion systemRotation = Quaternion.Euler(0f, 0f, pm.currentGalaxy.systems[a].systemCentre.rot1);
                    pos.x = origin.position.x + a * scalar;

                    mgo.transform.GetChild(0).transform.position = pos;
                    mgo.transform.rotation = systemRotation;

                    if (pm.currentGalaxy.systems[a] == pm.currentGalaxy.systems[pm.currentSystem])
                    {
                        pm.currentPositionMap.position = pos;
                    }

                    Instantiate(mgo, origin);
                }
            }
        }
    }

    public Vector3 getPositionOfSystem(int sys)
    {
        Vector3 pos = new Vector3(0f, 0f, 0f);
        pos.x = sys * scalar;
        return pos;
    }

    public Quaternion getRotationOfSystem(int sys)
    {
        Quaternion rot = Quaternion.Euler(0f, 0f, pm.currentGalaxy.systems[sys].systemCentre.rot1);
        return rot;
    }


    //Just for testing for now
    public void LoadParticles(ParticleSystem[] pss)
    {
        foreach (ParticleSystem ps in pss)
        {
            var m = ps.main;
            m.maxParticles = PlayerPrefs.GetInt("ParticleAmmount");
            ps.Clear();
            ps.Emit(PlayerPrefs.GetInt("ParticleAmmount"));
        }
    }

    public GalaxyObject GenerateRandomGalaxy(GalaxyObject gToGen = null)
    {
        if (gToGen == null || gToGen.galaxySystemCount <= galaxySystemMinReq)
        {
            GalaxyObject galaxy = new GalaxyObject();
            galaxy.galaxyName = "saggitarius";
            galaxy.systems = new SystemLevelObject[500];
            
            for (int a = 0; a < 500; a++)
            {
                if(a == 0)
                {
                    galaxy.systems[a] = saggitarius;
                    galaxy.galaxySystemCount++;
                }
                else if (a > 0 && a < galaxySystemMinLY)
                {
                    galaxy.systems[a] = new SystemLevelObject("null");
                    galaxy.systems[a].systemOrbitStage = a;
                    galaxy.galaxySystemCount++;
                }
                else if(a == 499)
                {
                    galaxy.systems[a] = tutorial;
                    galaxy.galaxySystemCount++;
                }
                else
                {
                    int chance = Random.Range(0, galaxySystemFrequency);

                    if (a < galaxySystemMinLY)
                    {
                        chance = Random.Range(0, galaxySystemFrequency);
                    }
                    else if (a > 100 && a < 300)
                    {
                        chance = Random.Range(0, galaxySystemFrequency - 1);
                    }
                    else if (a > 300 && a < 499)
                    {
                        chance = Random.Range(0, galaxySystemFrequency - 2);
                    }

                    if (chance == 0)
                    {
                        if (a <= 100)
                        {
                            galaxy.systems[a] = GenerateRandomSystem("augmented", a);
                            galaxy.galaxySystemCount++;
                        }
                        else if (a > 100 && a <= 300)
                        {
                            galaxy.systems[a] = GenerateRandomSystem("enhanced", a);
                            galaxy.galaxySystemCount++;
                        }
                        else if (a > 300 && a <= 498)
                        {
                            galaxy.systems[a] = GenerateRandomSystem("normal", a);
                            galaxy.galaxySystemCount++;
                        }

                        int isNebula = Random.Range(0, nebulaChance + nebulaBias);

                        if(isNebula == 0)
                        {
                            galaxy.systems[a].systemType += " nebula";
                        }
                    }
                    else
                    {
                        galaxy.systems[a] = new SystemLevelObject("null");
                        galaxy.galaxySystemCount++;
                    }
                }
            }

            return GenerateRandomGalaxy(galaxy);
        }
        else
        {
            return gToGen;
        }
    }

    //Just for testing
    public int generateAndRepeat(int r, int bound1, int bound2)
    {
        r = Random.Range(bound1, bound2);
        if (pm.currentGalaxy.systems[r].systemName != "null")
        {
            return r;
        }
        else
        {
            return generateAndRepeat(r, bound1, bound2);
        }
    }
  
    //Just for testing
    public int genAndRepeatPlanet(int r, int b1, int b2)
    {
        r = Random.Range(b1, b2);
        if (pm.currentGalaxy.systems[pm.currentSystem].systemPlanets[r] != null)
        {
            return r;
        }
        else
        {
            return genAndRepeatPlanet(r, b1, b2);
        }
    }

    //Just for testing
    public void ShowSystemsOfCurrentGalaxy()
    {
        GalaxyObject g = pm.currentGalaxy;
        Debug.Log("GALAXY");
        Debug.Log("GALAXY NAME - " + g.galaxyName);
        Debug.Log("GALAXY SYSTEM COUNT " + g.galaxySystemCount);
        Debug.Log("-----------");

        for(int a = 0; a < 500; a ++)
        {
            if(g.systems[a] != null)
            {
                Debug.Log("-----------");
                Debug.Log("STAR : " + g.systems[a].systemName);
                Debug.Log("PLANET COUNT : " + g.systems[a].systemPlanetCount);
                Debug.Log("DISTANCE FROM CENTER : " + a + "00 light years");
             //   Debug.Log("SYSTEM TYPE : " + g.systems[a].systemType);
            }
        }
    }

    public SunSector GenerateRandomSun()
    {
        SunSector sun = new SunSector();
        sun.rot1 = Random.Range(0f, 360f);
        sun.mapGO = sunMapGO;
        sun.sunRadius = Random.Range(sunMinR, sunMaxR);
        sun.sunColor = new Color(Random.Range(sunMinCR, sunMaxCR), Random.Range(sunMinCG, sunMaxCG), Random.Range(sunMinCB, sunMaxCB), 1f);
        sun.sectorGO = sunGO;
        sun.sectorObjective = possibleObjectives[Random.Range(0, possibleObjectives.Length)];
        sun.sectorType = "Sun";
        sun.sectorName = GenerateRandomName("", Random.Range(1, 4));
        return sun;
    }

    public SystemLevelObject GenerateRandomSystem(string systemType = "", int sos = 0)
    {
        SystemLevelObject system = new SystemLevelObject();
        system.systemOrbitStage = sos;
        system.systemCentre = GenerateRandomSun();
        system.systemName = system.systemCentre.sectorName;
        system.systemType = systemType;
        system.systemPlanetCount = Random.Range(sysPMin, sysPMax);
        system.systemPlanets = new PlanetObject[system.systemPlanetCount];
        system.systemPlanets[0] = system.systemCentre;
        for (int a = 1; a < system.systemPlanetCount; a++)
        {
            system.systemPlanets[a] = GenerateRandomPlanet(system.systemCentre, a+1);
            system.systemPlanets[a].sectorObjective.objectiveDifficulty = 500 - sos;
            system.systemPlanets[a].orbitNumber = a;
        }
        return system;
    }

    //Just for testing
    public SystemLevelObject GenerateSpecificSystem(SystemLevelObject slo)
    {
        return slo;
    }

    public string GenerateRandomName(string lastPart = "", int steps = 0)
    {
        lastPart += starNames[Random.Range(0, starNames.Length)];
        steps--;
        if (steps <= 0)
        {
            lastPart.Trim();
            lastPart += " - " + Random.Range(0, 100).ToString();
            return lastPart;
        }
        else
        {
            return GenerateRandomName(lastPart, steps);
        }
    }

    //Just for testing
    public void ShowRandomName(int st)
    {
        Debug.Log(GenerateRandomName("", st));
    }

    //Just for testing
    public void ShowInformationAboutCurrentSystem()
    {
        SystemLevelObject s = GenerateRandomSystem();

        Debug.Log("SYSTEM");
        Debug.Log("SYSTEM NAME : " + s.systemName);
        Debug.Log("SYSTEM PLANET COUNT : " + s.systemPlanetCount);

        Debug.Log("---------");
        Debug.Log("SECTOR CENTER - STAR");
        Debug.Log("STAR NAME : " + s.systemCentre.sectorName);
        Debug.Log("STAR RADIUS : " + s.systemCentre.sunRadius);



        if (s.systemPlanetCount > 0)
        {
            for (int b = 0; b < s.systemPlanetCount; b++)
            {
                Debug.Log("---------");
                Debug.Log("SECTOR TYPE - PLANET");
                Debug.Log("PLANET NAME : " + s.systemPlanets[b].sectorName);
                Debug.Log("PLANET RADIUS : " + s.systemPlanets[b].planetRadius);
                Debug.Log("PLANET TYPE : " + s.systemPlanets[b].planetType);
            }
        }

    }

    public PlanetObject GenerateRandomPlanet(SunSector s, int orbitNumber = 1)
    {
        PlanetObject planet = new PlanetObject();
        planet.rot1 = Random.Range(0f, 360f);
        planet.planetRadius = Random.Range(planetMinR, planetMaxR);
        planet.planetMainColor = new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
        int pType = Random.Range(0, planetTypes.Length);
        planet.planetType = planetTypes[pType];
        planet.mapGO = planetsGO[pType];
        planet.sectorGO = planetsSectorGO[pType];
        planet.sectorObjective = possibleObjectives[Random.Range(1, possibleObjectives.Length)];
        planet.sectorType = planet.planetType;
        planet.sectorName = s.sectorName + " - " + orbitNumber;
        return planet;
    }
}
