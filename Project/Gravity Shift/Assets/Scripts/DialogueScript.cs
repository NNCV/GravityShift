using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueScript : MonoBehaviour {

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

    private void Start()
    {
        characterNameBox.text = dialogueLines[currentDialogueLine].characterName;
        characterTextBox.text = dialogueLines[currentDialogueLine].characterText;
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
                characterNameBox.text = dialogueLines[currentDialogueLine].characterName;
                characterTextBox.text = dialogueLines[currentDialogueLine].characterText;
                characterImageBox.sprite = dialogueLines[currentDialogueLine].characterImage;
            }

            if (dialogueLines[currentDialogueLine].isSkippable == true)
            {
                if (Input.GetKeyDown("L"))
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
            if(Input.GetKeyDown("K"))
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
