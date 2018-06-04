using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour {
    
    public bool inDialogue = false;
	
	void Update () {
		
	}

    public void ShowDialogue(DialogueScript ds)
    {
        if(inDialogue)
        {
            if(ds.canBeOverlayed)
            {
                GameObject dialogue = Instantiate(ds.gameObject, this.gameObject.GetComponent<RectTransform>(), false) as GameObject;
                dialogue.GetComponent<RectTransform>().position = ds.pos.position;
                Instantiate(dialogue, this.gameObject.GetComponent<RectTransform>(), false);
                inDialogue = true;
            }
        }
        else
        {
            GameObject dialogue = Instantiate(ds.gameObject, this.gameObject.GetComponent<RectTransform>(), false) as GameObject;
            dialogue.GetComponent<RectTransform>().position = ds.pos.position;
            Instantiate(dialogue, this.gameObject.GetComponent<RectTransform>(), false);
            inDialogue = true;
        }
    }
}
