﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuAnimationManager : MonoBehaviour {

    public Button playNewButton;
    public Button playLoadButton;
    public InputField playNameInputField;
    public Slider particleSlider;

    public PlayerManager pm;
    public LevelManager lm;
    public Animator anim;
    
    public int button = 0;

    void Start()
    {
        PlayerPrefs.SetInt("ParticleAmmount", 16);
    }

    void LateUpdate ()
    {
        if(PlayerPrefs.GetString("ShipHull").Length <= 0)
        {
            playLoadButton.interactable = false;
            playNewButton.interactable = true;
        }
        else
        {
            playNewButton.interactable = true;
            playLoadButton.interactable = true;
        }

        anim.SetInteger("buttonState", button);
    }

    public void clickButton(int buttonToClick)
    {
        button = buttonToClick;
    }

    public void createNewPlayer()
    {
        PlayerPrefs.SetString("ShipHull", "hellstorm hull");
        PlayerPrefs.SetInt("CurrentLevel", 1);
        PlayerPrefs.SetInt("CurrentXP", 0);
        PlayerPrefs.SetString("PlayerName", playNameInputField.text);
        PlayerPrefs.SetString("Blaster0", "diablo turret");
        PlayerPrefs.SetString("Blaster1", "diablo turret");
        PlayerPrefs.SetString("Reactor0", "protos reactor");
        PlayerPrefs.SetString("Shield0", "broken diffuser");
        PlayerPrefs.SetString("CanJump", "false");
        PlayerPrefs.SetInt("HullCurrent", 1000);
        PlayerPrefs.SetInt("ShieldCurrent", 1000);
        PlayerPrefs.SetInt("EnergyCurrent", 0);
        PlayerPrefs.SetInt("ParticleAmmount", PlayerPrefs.GetInt("ParticleAmmount"));
        PlayerPrefs.SetInt("CurrentSystem", 499);
        PlayerPrefs.SetInt("CurrentSector", 5);
        PlayerPrefs.SetString("CompletedSystems", "");

        for (int a = 0; a < 6; a++)
        {
            for (int b = 0; b < 8; b++)
            {
                PlayerPrefs.SetString("InventorySlot" + b + "" + a, "");
            }
        }

        pm.currentGalaxy = pm.lm.GenerateRandomGalaxy();

        pm.saveGalaxy();

        loadGame();
    }

    public void loadGame()
    {
        SceneManager.LoadScene("Game Scene");
    }

    public void saveSettings()
    {
        PlayerPrefs.SetInt("ParticleAmmount", (int)particleSlider.value);
    }

    public void exitGame()
    {
        Application.Quit();
    }

}
