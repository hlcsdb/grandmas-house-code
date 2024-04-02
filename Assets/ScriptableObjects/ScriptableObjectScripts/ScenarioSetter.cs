using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "Scenario Setter")]

public class ScenarioSetter : ScriptableObject
{
    public List<Scenario> scenarios;
    public Scenario currentScenario; //change to internal if using scenariosetterscene
    internal int currentScenarioIndex;

    void Start()
    {
        currentScenarioIndex = 2; //3 is only because I'm working with jewelry right now, which is index 2;
        currentScenario = scenarios[currentScenarioIndex];
        //currentScenarioIndex = scenarios.FindIndex(x => x.Equals(currentScenario));
    }

    public void ChangeScenario(int scenarioIndex)
    {
        currentScenario = scenarios[scenarioIndex];
        currentScenarioIndex = scenarioIndex;
    }

    public void ChangeScenarioSoup()
    {
        ChangeScenario(1);
    }
}
