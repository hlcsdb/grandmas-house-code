using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{

    public void GotoHomeScene()
    {
        //Debug.Log("go home");
        SceneManager.LoadScene("Home");
    }

    public void GotoPlayScene()
    {
        SceneManager.LoadScene("Play");
    }

    public void GotoNavScene()
    {
        SceneManager.LoadScene("Navigation");
    }

    public ScenarioSetter scenarioSetter;
    public void SetScenario(int iScenario)
    {
        scenarioSetter.ChangeScenario(iScenario);
    }

    public void GotoSelectionScene()
    {
        //Debug.Log("go to scenario selection");
        //SceneManager.LoadScene("ScenarioSelection");
        SceneManager.LoadScene("ScenarioCarousel");
    }


}
