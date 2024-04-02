using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SettingsPanelController : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject backgroundMusic;
    public Toggle musicToggle;

    public Button engButton; //dialect = 0
    public Button hulButton; //dialect = 1
    public Button hunButton; //dialect = 2

    public int currDialect = 0;

    public void ShowPanel()
    {
        settingsPanel.SetActive(true);
    }

    public void HidePanel()
    {
        settingsPanel.SetActive(false);
    }

    public void MusicOnOff()
    {
        if (musicToggle == false)
        {
            backgroundMusic.SetActive(true);
        }
        else
        {
            backgroundMusic.SetActive(false);
        }
    }

    public void ChangeDialect(int dialect)
    {
        //make function in scripts to change text values to other dialect
        //ChallengeController challengeController = GameObject.Find("Challenge Manager").GetComponent<ChallengeController>();
        //challengeController.selectedScenario.SetDialect(dialect);
        currDialect = dialect;

        if(dialect == 0)
        {
            engButton.interactable = false;
            hulButton.interactable = true;
            hunButton.interactable = true;
        }

        else if (dialect == 1)
        {
            engButton.interactable = true;
            hulButton.interactable = false;
            hunButton.interactable = true;
        }
        else if (dialect == 2)
        {
            engButton.interactable = true;
            hulButton.interactable = true;
            hunButton.interactable = false;
        }

        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Challenge"))
        {
            ChallengeController challengeManager = GameObject.Find("Challenge Manager").GetComponent<ChallengeController>();
            challengeManager.SetDialect(dialect);
        }
    }
}
