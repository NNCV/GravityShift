﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour {

    public PlayerManager pm;
    public bool inDialogue = false;
    public Canvas cameraCanvas;

    private void Start()
    {
        pm = FindObjectOfType<PlayerManager>();
    }
    
    public void ShowDialogue(DialogueScript ds)
    {
        if(inDialogue)
        {
            if(ds.canBeOverlayed)
            {
                GameObject dialogue = Instantiate(ds.gameObject, this.gameObject.GetComponent<RectTransform>(), false) as GameObject;
                dialogue.GetComponent<RectTransform>().position = ds.pos.position;
                if (Camera.main.orthographic == false)
                {
                    Instantiate(dialogue, this.gameObject.GetComponent<RectTransform>(), false);
                }
                else
                {
                    Instantiate(dialogue, transform.position, Quaternion.LookRotation(Camera.main.transform.position, transform.up));
                }
                inDialogue = true;
            }
        }
        else
        {
            if (Camera.main.orthographic == true)
            {
                GameObject dialogue = Instantiate(ds.gameObject, this.gameObject.GetComponent<RectTransform>(), false) as GameObject;
                dialogue.GetComponent<RectTransform>().position = ds.pos.position;
                Instantiate(dialogue, this.gameObject.GetComponent<RectTransform>(), false);
            }
            else
            {
                GameObject dialogue = ds.gameObject;
                dialogue.GetComponent<RectTransform>().position = ds.pos.position;
                dialogue.GetComponent<RectTransform>().localScale = new Vector3(-1f, -1f, -1f);
                dialogue.GetComponent<RectTransform>().rotation = Quaternion.Euler(180f, 180f, 0f);
                Instantiate(dialogue, cameraCanvas.GetComponent<RectTransform>(), false);
            }
            inDialogue = true;
        }
    }
}
