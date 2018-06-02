using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastScript : MonoBehaviour
{
    public Vector2 speed;
    public float distanceMax = 0f;
    public Vector3 initPos;
    
    public Rigidbody2D rb2d;
    public BoxCollider2D bc2d;

    public int damage = 0;

    void Start()
    {
        initPos = transform.position;

        rb2d = GetComponent<Rigidbody2D>();
        rb2d.AddRelativeForce(speed);
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(initPos, transform.position) >= distanceMax)
        {
            Destroy(transform.gameObject);
        }
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyTestTargetScript>().health -= damage;
            Destroy(transform.gameObject);
        }
    }
}