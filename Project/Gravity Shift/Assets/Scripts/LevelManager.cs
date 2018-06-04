using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {


    public float sunHueMin, sunHueMax, sunA;
    public float sunRadiusMin;
    public float sunRadiusMax;

    //make "LevelObject" scriptable object
    //import the multiple types of levels here
    //randomize values
    //set values
    //save

    public ParticleSystem[] pss;
    public SunSector sun;

	void Start () {

        foreach(ParticleSystem ps in pss)
        {
            var m = ps.main;
            m.maxParticles = PlayerPrefs.GetInt("ParticleAmmount");
            ps.Clear();
            ps.Emit(PlayerPrefs.GetInt("ParticleAmmount"));
        }
	}
}
