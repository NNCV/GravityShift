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
            int randomNumber = Random.Range(1, 10) + 1;
            if(randomNumber <= 6)
            {
                pim.AddItemToLastPosition(possibleDrops[0], 0, 0);
            }
            else if (randomNumber <= 9 && randomNumber > 6)
            {
                pim.AddItemToLastPosition(possibleDrops[1], 0, 0);
            }
            else if (randomNumber == 10)
            {
                pim.AddItemToLastPosition(possibleDrops[2], 0, 0);
            }
        }
    }
}
