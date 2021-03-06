﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueScript : MonoBehaviour {

    public PlayerManager pm;
    public DialogueManager dm;

    public bool canBeOverlayed = false;
    public bool canBeSkippedEntirely = false;

    public RectTransform pos;

    public DialogueObject[] dialogueLines;
    public Animator anim;
    public int animState = 0;

    public int currentDialogueLine = 0;

    public Text characterNameBox;
    public Text characterTextBox;
    public Image characterImageBox;

    public float timeToStay = 0f;
    public float timeCurrent = 0f;

    public bool isLastDialogueLine = false;
    public float timeToFadeOut = 0f;

    public float timeToLoad = 0f;

    public Vector3 offset;

    private void Start()
    {
        dm = FindObjectOfType<DialogueManager>();
        pm = FindObjectOfType<PlayerManager>();
        if (dialogueLines[currentDialogueLine].characterName == "PLAYER")
        {
            characterNameBox.text = pm.playerName;
        }
        else
        {
            characterNameBox.text = dialogueLines[currentDialogueLine].characterName;
        }
        if (dialogueLines[currentDialogueLine].characterText.Contains("PLAYER"))
        {
            dialogueLines[currentDialogueLine].characterText = dialogueLines[currentDialogueLine].characterText.Replace("PLAYER", pm.playerName);
            characterTextBox.text = dialogueLines[currentDialogueLine].characterText;
        }
        else
        {
            characterTextBox.text = dialogueLines[currentDialogueLine].characterText;
        }
        characterImageBox.sprite = dialogueLines[currentDialogueLine].characterImage;
    }

    void Update () {

        timeToStay = dialogueLines[currentDialogueLine].timeToDisappear;

        if (isLastDialogueLine == false)
        {
            anim.SetInteger("AnimationStage", animState);

            if (characterNameBox.text != dialogueLines[currentDialogueLine].characterName)
            {
                animState = 0;
            }

            if (timeCurrent >= timeToLoad)
            {
                if (dialogueLines[currentDialogueLine].characterName == "PLAYER")
                {
                    characterNameBox.text = pm.playerName;
                }
                else
                {
                    characterNameBox.text = dialogueLines[currentDialogueLine].characterName;
                }
                if (dialogueLines[currentDialogueLine].characterText.Contains("PLAYER"))
                {
                    dialogueLines[currentDialogueLine].characterText = dialogueLines[currentDialogueLine].characterText.Replace("PLAYER", pm.playerName);
                    characterTextBox.text = dialogueLines[currentDialogueLine].characterText;
                }
                else
                {
                    characterTextBox.text = dialogueLines[currentDialogueLine].characterText;
                }
                characterImageBox.sprite = dialogueLines[currentDialogueLine].characterImage;
            }

            if (dialogueLines[currentDialogueLine].isSkippable == true)
            {
                if (Input.GetKeyDown(KeyCode.L))
                {
                    currentDialogueLine++;
                    return;
                }

                timeCurrent += Time.deltaTime;
                if (timeCurrent >= timeToStay)
                {
                    timeCurrent = 0;
                    currentDialogueLine++;
                    if (currentDialogueLine >= dialogueLines.Length - 1)
                    {
                        anim.SetInteger("AnimationStage", 10);
                        isLastDialogueLine = true;
                    }
                    return;
                }
            }
            else
            {
                timeCurrent += Time.deltaTime;
                if (timeCurrent >= timeToStay)
                {
                    timeCurrent = 0;
                    currentDialogueLine++;
                    anim.SetInteger("AnimationStage", 10);
                    if (currentDialogueLine >= dialogueLines.Length - 1)
                    {
                        anim.SetInteger("AnimationStage", 10);
                        isLastDialogueLine = true;
                    }
                }
                return;
            }
        }
        else
        {
            timeCurrent += Time.deltaTime;
            if(timeCurrent >= timeToFadeOut)
            {
                dm.inDialogue = false;
                Destroy(this.gameObject);
            }
        }

        if(canBeSkippedEntirely == true)
        {
            if(Input.GetKeyDown(KeyCode.K))
            {
                anim.SetInteger("AnimationStage", 10);
                timeCurrent += Time.deltaTime;
                if (timeCurrent >= timeToFadeOut)
                {
                    dm.inDialogue = false;
                    Destroy(this.gameObject);
                }
            }
        }
	}
}
