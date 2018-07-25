using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropScript : MonoBehaviour {
    
    public PlayerInventoryManager pim;

    public GeneralItem drop;
    public GameObject text;

    private void Start()
    {
        text = transform.GetChild(0).gameObject;
        text.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            text.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            text.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                pim = collision.gameObject.GetComponent<PlayerInventoryManager>();
                pim.AddItemToLastPosition(drop);
                Destroy(gameObject);
            }
        }
    }
}
