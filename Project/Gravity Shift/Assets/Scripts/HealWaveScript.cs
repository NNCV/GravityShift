using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealWaveScript : MonoBehaviour {

    public Vector3 scaleInit, scaleMax;
    public Material mat;
    public float speed;
    public float healingPower;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            if (collision.gameObject.GetComponent<BasicEnemyScript>().enemyTypeID != 1)
            {
                collision.gameObject.GetComponentInChildren<BasicEnemyScript>().healthCurrent += (collision.gameObject.GetComponentInChildren<BasicEnemyScript>().healthMax - collision.gameObject.GetComponentInChildren<BasicEnemyScript>().healthCurrent) * healingPower;
            }
        }
    }

    public void Start()
    {
        GetComponent<MeshRenderer>().material = mat;
        transform.localScale = scaleInit;
        mat.SetFloat("Vector1_A3572D9C", 0f);
    }

    public void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, scaleMax, Time.deltaTime * speed);
        mat.SetFloat("Vector1_A3572D9C", Mathf.Lerp(mat.GetFloat("Vector1_A3572D9C"), 1f, Time.deltaTime * speed));
        if (mat.GetFloat("Vector1_A3572D9C") >= 0.98f)
        {
            Destroy(gameObject);
        }
    }
}
