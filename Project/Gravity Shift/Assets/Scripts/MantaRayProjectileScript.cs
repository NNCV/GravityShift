using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantaRayProjectileScript : MonoBehaviour {

    public GameObject explosion;
    public Vector3 force;
    public float timeMax;
    public float timeCurrent;
    public int damage;
    public Rigidbody2D rb;

	void Start () {
        rb = GetComponent<Rigidbody2D>();
        rb.AddRelativeForce(force);
	}
	
	void FixedUpdate () {
        timeCurrent += Time.deltaTime;
        if (timeCurrent >= timeMax)
        {
            Destroy(gameObject);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<PlayerEquipmentManager>().TakeDamage(damage);
            Destroy(gameObject.transform.GetChild(0).gameObject);
            Destroy(GetComponent<BoxCollider2D>());
            Destroy(rb);
            //Instantiate(explosion, transform.position, transform.rotation);
        }
    }
}
