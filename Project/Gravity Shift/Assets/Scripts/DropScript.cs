using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropScript : MonoBehaviour {
    
    public PlayerInventoryManager pim;

    public GeneralItem[] possibleDrops;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            int randomNumber = Random.Range(0, possibleDrops.Length);
            pim.AddItemToLastPosition(possibleDrops[randomNumber]);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            int randomNumber = Random.Range(0, possibleDrops.Length);
            pim.AddItemToLastPosition(possibleDrops[randomNumber]);
        }
    }
}
