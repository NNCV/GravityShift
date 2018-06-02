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
                //-31498.61 x
                //-8099.644 z
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
