using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public float sunMinR, sunMaxR, planetMinR, planetMaxR;
    public int sunMinCR, sunMinCG, sunMinCB, sunMaxCR, sunMaxCG, sunMaxCB;
    public Shader sunShaderCentre, sunShaderInner, sunShaderMid, sunShaderOuter;
    public Shader[] planetShadersInner;
    public Shader[] planetShadersOuter;
    public int sysPMin, sysPMax;

    public string[] starNames;
    public string[] planetTypes;

    public ObjectiveObject[] possibleObjectives;

    SystemLevelObject[] levels;

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

    public SunSector GenerateRandomSun()
    {
        SunSector sun = new SunSector();
        sun.sunRadius = Random.Range(sunMinR, sunMaxR);
        sun.sunColor = new Color(Random.Range(sunMinCR, sunMaxCR), Random.Range(sunMinCG, sunMaxCG), Random.Range(sunMinCB, sunMaxCB));
        Material centreMat = new Material(sunShaderCentre);
        Material innerMat = new Material(sunShaderInner);
        Material midMat = new Material(sunShaderMid);
        Material outerMat = new Material(sunShaderOuter);
        centreMat.SetColor("Sun Color", sun.sunColor);
        innerMat.SetColor("Sun Color", sun.sunColor);
        midMat.SetColor("Sun Color", sun.sunColor);
        outerMat.SetColor("Sun Color", sun.sunColor);
        sun.sectorGO.transform.GetChild(0).GetComponent<Renderer>().material = centreMat;
        sun.sectorGO.transform.GetChild(1).GetComponent<Renderer>().material = innerMat;
        sun.sectorGO.transform.GetChild(2).GetComponent<Renderer>().material = midMat;
        sun.sectorGO.transform.GetChild(3).GetComponent<Renderer>().material = outerMat;
        sun.sectorGO.transform.localScale.Scale(new Vector3(sun.sunRadius, sun.sunRadius, sun.sunRadius));
        sun.sectorObjective = possibleObjectives[Random.Range(0, possibleObjectives.Length)];
        sun.sectorType = "Sun";
        sun.sectorName = GenerateRandomName("", Random.Range(0, 3));
        return sun;
    }

    public SystemLevelObject GenerateRandomSystem()
    {
        SystemLevelObject system = new SystemLevelObject();
        system.systemCentre = GenerateRandomSun();
        system.systemPlanetCount = Random.Range(sysPMin, sysPMax);
        system.systemPlanets = new PlanetObject[system.systemPlanetCount];
        for(int a = 0; a < system.systemPlanetCount; a++)
        {
            system.systemPlanets[a] = GenerateRandomPlanet(system.systemCentre);
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

    public PlanetObject GenerateRandomPlanet(SunSector s, int orbitNumber = 1)
    {
        PlanetObject planet = new PlanetObject();
        planet.planetRadius = Random.Range(planetMinR, planetMaxR);
        planet.planetMainColor = new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
        planet.planetSecondaryColor = new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
        int pType = Random.Range(0, planetTypes.Length);
        planet.planetType = planetTypes[pType];
        Material innerMat = new Material(planetShadersInner[pType]);
        Material outerMat = new Material(planetShadersOuter[pType]);
        innerMat.SetColor("Main Planet Color", planet.planetMainColor);
        outerMat.SetColor("Secondary Planet Color", planet.planetSecondaryColor);
        planet.sectorGO.transform.GetChild(0).GetComponent<Renderer>().material = innerMat;
        planet.sectorGO.transform.GetChild(1).GetComponent<Renderer>().material = outerMat;
        planet.sectorGO.transform.localScale.Scale(new Vector3(planet.planetRadius, planet.planetRadius, planet.planetRadius));
        planet.sectorObjective = possibleObjectives[Random.Range(0, possibleObjectives.Length)];
        planet.sectorType = planet.planetType;
        planet.sectorName = s.sectorName + orbitNumber;
        return planet;
    }
}
