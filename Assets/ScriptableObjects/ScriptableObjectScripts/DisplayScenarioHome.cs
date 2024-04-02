using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DisplayScenarioHome : MonoBehaviour
{
    public ScenarioSetter scenarioSetter;

    public Scenario scenario;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI titleTextEngl;
    public GameObject homeScreenBackground;
    public AudioSource audSource;

    // Start is called before the first frame update
    void Start()
    {
        scenario = scenarioSetter.currentScenario;
        SetScenario();
        audSource.PlayOneShot(scenario.GetTitleAudioClip());
    }

    public void SetScenario()
    {
        titleText.text = scenario.titleName[1];
        titleTextEngl.text = scenario.titleName[0];
        homeScreenBackground.GetComponent<Image>().sprite = scenario.homeImage;
    }

    public void PlayTitleAudio()
    {
        Debug.Log(scenario.GetTitleAudioClip().name);
        audSource.PlayOneShot(scenario.GetTitleAudioClip());
    }

    public void GoToScene()
    {
        SceneManager.LoadScene("Play");
    }

    public void GotoSelectionScene()
    {
        //Debug.Log("go home");
        //SceneManager.LoadScene("ScenarioSelection");
        SceneManager.LoadScene("ScenarioCarousel");

    }
}
