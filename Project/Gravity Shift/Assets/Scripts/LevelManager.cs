using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public Random rand;
    
    //make "LevelObject" scriptable object
    //import the multiple types of levels here
    //randomize values
    //set values
    //save

    public ParticleSystem[] pss;

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
