using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyScript : MonoBehaviour {

    //Variabile generale necesare pentru o nava simpla
    public float healthCurrent, healthMax;
    public float shieldCurrent, shieldMax;
    public bool dead = false;
    public int playState = -1;
    public Transform player;
    
    //Alte nave mostenitoare vor putea sa nu foloseasca portiunea asta (ex: nave mari care sunt facute din mai multe piese)
    public virtual void Start()
    {
        healthCurrent = healthMax;
        shieldCurrent = shieldMax;
    }

    //Cand nava nu mai are viata, navele ce vor mosteni clasa asta vor putea sa isi adauge propriul efect de explozie
    public virtual void FixedUpdate()
    {
        if(healthCurrent <= 0)
        {
            dead = true;
        }
    }
    
    public virtual void Explode()
    {
        Destroy(gameObject);
    }
}
