using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAreaManager : MonoBehaviour {

    public DialogueManager dm;
    public DialogueScript ds;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ds.dm = dm;
            dm.ShowDialogue(ds);
        }
    }
}
