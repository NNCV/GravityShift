using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTestTargetScript : MonoBehaviour {
    
    public int health;
    public int maxHealth;

    public float spawnRate;
    public float spawnCooldown;

    public bool dead = false;

    public float pRotMin, pRotMax;
    public float mSpeed;
    public Quaternion quat;
    
    public float timeRand;
    public float timeMin, timeMax;
    public float timeCurrent;

    public float speed;
    public float rotSpeed;

    public PlayerManager pm;

    private void Start()
    {
        timeRand = Random.Range(timeMin, timeMax);
    }

    public void disableAndResetTrails()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<TrailRenderer>().enabled = false;
        }
    }

    public void enableTrails()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<TrailRenderer>().enabled = true;
        }
    }

    private void Update()
    {
        if(health <= 0)
        {
            if (dead == false)
            {
                disableAndResetTrails();
                spawnCooldown = 0;
                dead = true;
            }
        }

        if(dead)
        {

            spawnCooldown += Time.deltaTime;

            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);

            if (spawnCooldown >= spawnRate)
            {
                enableTrails();
                health = maxHealth;
                dead = false;
                GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            }
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, Time.deltaTime * rotSpeed);
            transform.position = Vector3.Lerp(transform.position, transform.position + transform.up * mSpeed, Time.deltaTime * speed);
            timeCurrent += Time.deltaTime;
            if(timeCurrent >= timeRand)
            {
                float rot = Random.Range(pRotMin, pRotMax);
                quat = transform.rotation * Quaternion.Euler(0f, 0f, rot);
                timeCurrent = 0;
                timeRand = Random.Range(timeMin, timeMax);
            }
        }
    }
}
