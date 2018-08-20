using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour {

    public float scaleInit, scaleMax, speed;
    public float distInit, distFin;
    public Vector2 speedInit, speedFin;
    public float initFL, finalFL;
    private float ratio;
    public Material mat;

    private void Start()
    {
        if(FindObjectOfType<PlayerManager>().isDead == true)
        {
            mat = gameObject.GetComponent<MeshRenderer>().material;
        }
        mat.SetVector("Vector2_B8BB5A6B", speedInit);
        mat.SetFloat("Vector1_5F675393", distInit);
        mat.SetFloat("Vector1_A3572D9C", initFL);
        transform.localScale = new Vector3(scaleInit, scaleInit, scaleInit);
    }

    private void Update()
    {
        ratio = transform.localScale.x / scaleMax;

        if (ratio <= 0.98f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(scaleMax, scaleMax, scaleMax), Time.deltaTime * speed);
            Vector2 vecComp = Vector2.Lerp(speedInit, speedFin, ratio);
            mat.SetVector("Vector2_B8BB5A6B", vecComp);
            mat.SetFloat("Vector1_5F675393", Mathf.Lerp(distInit, distFin, ratio));
            mat.SetFloat("Vector1_A3572D9C", Mathf.Lerp(initFL, finalFL, ratio));
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
