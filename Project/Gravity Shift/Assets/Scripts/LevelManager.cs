using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LevelManager : MonoBehaviour {

    public float sunMinR, sunMaxR, planetMinR, planetMaxR;
    public float sunMinCR, sunMinCG, sunMinCB, sunMaxCR, sunMaxCG, sunMaxCB;
    public Shader sunShaderCentre, sunShaderInner, sunShaderMid, sunShaderOuter;
    public int galaxySystemFrequency = 8;
    public int galaxySystemMinReq = 40;
    public int galaxySystemMinLY = 35;
    public int galaxySystemFreqBias = 0;
    public Shader[] planetShadersInner;
    public Shader[] planetShadersOuter;
    public int sysPMin, sysPMax;

    public float scalar = 10f;
    public float scalarInside = 10f;
    public float offsetInside = 1f;

    public GameObject sunGO;
    public GameObject sunMapGO;
    public GameObject[] planetsGO;
    public GameObject orbitRingGO;

    public SystemLevelObject saggitarius;
    public SystemLevelObject tutorial;

    public string[] starNames;
    public string[] planetTypes;

    public ObjectiveObject[] possibleObjectives;

    public SystemLevelObject[] levels;
    public GalaxyObject currentGalaxy;
    
    public void GenerateGalaxyMap(Transform origin)
    {
        for (int a = 0; a < 500; a++)
        {
            if (currentGalaxy.systems[a] != null)
            {
                GameObject mgo = currentGalaxy.systems[a].systemCentre.mapGO;
                mgo.GetComponent<MapDataContainerScript>().slo = currentGalaxy.systems[a];
                mgo.GetComponent<MapDataContainerScript>().scalarInside = scalarInside;
                mgo.GetComponent<MapDataContainerScript>().offsetInside = offsetInside;

                Vector3 pos = new Vector3(0f, 0f, 0f);
                pos.x = origin.position.x + a * scalar * currentGalaxy.systems[a].systemCentre.rot1;
                pos.y = origin.position.y + a * scalar * currentGalaxy.systems[a].systemCentre.rot2;
                pos.z = origin.position.z;

                Instantiate(mgo, pos, origin.rotation, origin);
            }
        }
    }

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
            galaxy.galaxyName = GenerateRandomName("Galaxy ", 2);
            galaxy.systems = new SystemLevelObject[500];
            for (int a = galaxySystemMinLY; a < 500; a++)
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
                else if (a > 300 && a < 500)
                {
                    chance = Random.Range(0, galaxySystemFrequency - 2);
                }

                if (chance == 0)
                {
                    if (a <= 100)
                    {
                        galaxy.systems[a] = GenerateRandomSystem("Augmented", a);
                        galaxy.galaxySystemCount++;
                    }
                    else if (a > 100 && a <= 300)
                    {
                        galaxy.systems[a] = GenerateRandomSystem("Enhanced", a);
                        galaxy.galaxySystemCount++;
                    }
                    else if (a < 300 && a <= 500)
                    {
                        galaxy.systems[a] = GenerateRandomSystem("Normal", a);
                        galaxy.galaxySystemCount++;
                    }
                }
                else galaxy.systems[a] = null;
            }
           // galaxy.systems[0] = saggitarius;
           // galaxy.systems[500] = tutorial;
            return GenerateRandomGalaxy(galaxy);
        }
        else
        {
            return gToGen;
        }
    }

    public void ShowSystemsOfCurrentGalaxy()
    {
        GalaxyObject g = currentGalaxy;
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
                Debug.Log("SYSTEM TYPE : " + g.systems[a].systemType);
            }
        }
    }

    public SunSector GenerateRandomSun()
    {
        SunSector sun = new SunSector();
        sun.rot1 = Mathf.Sin(Random.Range(0, Mathf.PI * 2));
        sun.rot2 = Mathf.Sin(Random.Range(0, Mathf.PI * 2));
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
        system.orbitRingGO = orbitRingGO;
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

    public void ShowRandomName(int st)
    {
        Debug.Log(GenerateRandomName("", st));
    }

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
        planet.rot1 = Mathf.Sin(Random.Range(0, Mathf.PI * 2));
        planet.rot2 = Mathf.Sin(Random.Range(0, Mathf.PI * 2));
        planet.planetRadius = Random.Range(planetMinR, planetMaxR);
        planet.planetMainColor = new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
        planet.planetSecondaryColor = new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
        int pType = Random.Range(0, planetTypes.Length);
        planet.planetType = planetTypes[pType];
        planet.mapGO = planetsGO[pType];
        planet.sectorObjective = possibleObjectives[Random.Range(1, possibleObjectives.Length)];
        planet.sectorType = planet.planetType;
        planet.sectorName = s.sectorName + " - " + orbitNumber;
        return planet;
    }
}
