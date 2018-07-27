using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyScript : MonoBehaviour {

    //Variabile generale necesare pentru o nava simpla
    public int enemyID;
    public int enemyTypeID;
    public GameObject hpViewHolder, shViewHolder;
    public float hpBaseScale, shBaseScale;
    public Vector3 hpOffset, shOffset;
    public GameObject dropGO;
    public GeneralItem[] drops;
    public float[] dropRates;
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
        hpViewHolder.transform.GetChild(1).transform.localScale = new Vector3(hpBaseScale, hpViewHolder.transform.GetChild(1).transform.localScale.y, hpViewHolder.transform.GetChild(1).transform.localScale.z);
        shViewHolder.transform.GetChild(1).transform.localScale = new Vector3(shBaseScale, shViewHolder.transform.GetChild(1).transform.localScale.y, shViewHolder.transform.GetChild(1).transform.localScale.z);
    }

    //Cand nava nu mai are viata, navele ce vor mosteni clasa asta vor putea sa isi adauge propriul efect de explozie
    public virtual void FixedUpdate()
    {
        hpViewHolder.transform.localPosition = transform.localPosition + hpOffset;
        shViewHolder.transform.localPosition = transform.localPosition + shOffset;
        hpViewHolder.transform.GetChild(0).transform.localScale = new Vector3(Mathf.Lerp(hpViewHolder.transform.GetChild(0).transform.localScale.x, (healthCurrent / healthMax) * hpBaseScale, Time.deltaTime * 40f), hpViewHolder.transform.GetChild(0).transform.localScale.y, hpViewHolder.transform.GetChild(0).transform.localScale.z);
        shViewHolder.transform.GetChild(0).transform.localScale = new Vector3(Mathf.Lerp(shViewHolder.transform.GetChild(0).transform.localScale.x, (shieldCurrent / shieldMax) * shBaseScale, Time.deltaTime * 40f), shViewHolder.transform.GetChild(0).transform.localScale.y, shViewHolder.transform.GetChild(0).transform.localScale.z);
        
        if (healthCurrent <= 0)
        {
            dead = true;
        }
    }

    public virtual void Explode()
    {
        foreach(AssasinationObjective oo in FindObjectOfType<ObjectiveManager>().currentObjectives)
        {
            if(oo.enemyID == enemyID)
            {
                oo.objectiveProgress++;
            }
        }

        foreach(ExterminateObjective oo in FindObjectOfType<ObjectiveManager>().currentObjectives)
        {
            if(oo.enemyType == enemyTypeID)
            {
                oo.objectiveProgress++;
            }
        }

        GameObject drop = dropGO;
        int selected = dropRates.Length - 1;
        float chance = Random.Range(0f, 1f);
        bool found = false;
        if (chance >= dropRates[0])
        {
            while(!found)
            {
                if(dropRates[selected] > chance)
                {
                    selected--;
                }
                if(dropRates[selected] <= chance)
                {
                    found = true;
                }
            }
            drop.GetComponent<DropScript>().drop = drops[selected];
            drop.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = drops[selected].itemImage;
            drop.transform.GetChild(2).GetComponent<SpriteRenderer>().color = drops[selected].rarity;
            Instantiate(drop, transform.position, transform.rotation);
        }
    }
}
