using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageWaveScript : MonoBehaviour {

    public Vector3 scaleInit, scaleMax;
    public Material mat;
    public float speed;
    public float damagingPower;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponentInChildren<PlayerManager>().pem.TakeDamage(damagingPower);
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
