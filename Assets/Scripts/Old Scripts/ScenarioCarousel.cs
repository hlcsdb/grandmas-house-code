using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScenarioCarousel : MonoBehaviour
{
    public ScenarioSetter scenarioSetter;
    internal List<Scenario> scenarios;
    public Sprite navImg;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI titleTextEngl;
    public TextMeshProUGUI vocabListText;
    public TextMeshProUGUI vocabListTextEngl;
    public TextMeshProUGUI roomName;

    public Image gameSneakPeakImage;

    public Button upButton;
    public Button downButton;
    int numScenarios;
    List<Color> navDotColors = new List<Color> { new Color(0.78f, 0.78f, 0.875f, 1), new Color(0.78f, 0.78f, 0.875f, 0.4f) };
    List<GameObject> navDots = new List<GameObject>();
    List<Vector2> vocabPositions = new List<Vector2>() { new Vector2(-220, 46), new Vector2(-60, 46), new Vector2(100, 46), new Vector2(260, 46), new Vector2(-220, -74), new Vector2(-60, -74), new Vector2(100, -74), new Vector2(260, -74) };
    List<GameObject> draggableObjects;


    int activeScenarioIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        scenarios = new List<Scenario>(scenarioSetter.scenarios);
        numScenarios = scenarios.Count;
        SpawnNavDots();
        //SpawnDraggables();
        SetVocabList();
        SetGameTitle();
        StartCoroutine(SetGameImage());
    }

    public GameObject navDotContainer;

    void SpawnNavDots()
    {
        float dotContainerWidth = navDotContainer.GetComponent<RectTransform>().rect.width;
        float dotSize = (dotContainerWidth * 0.7f) / numScenarios; // = 42
        Debug.Log(dotSize);
        float dotGapWidth = (dotContainerWidth * 0.3f) / (numScenarios-1); //18
        float x = -(dotContainerWidth / 2);

        for(int i = 0; i < numScenarios; i++)
        {
            GameObject dot = new GameObject();
            dot.transform.SetParent(navDotContainer.transform);
            
           
            dot.AddComponent<Image>().sprite = navImg;
            dot.transform.localScale = new Vector3(1, 1, 1);
            dot.GetComponent<RectTransform>().sizeDelta = new Vector2(dotSize, dotSize);
            dot.transform.localPosition = new Vector2(x, 0);
            dot.GetComponent<Image>().color = navDotColors[0];
            navDots.Add(dot);

            x += dotSize + dotGapWidth; // dot2: -48
            Debug.Log(x);
        }
        navDots[0].GetComponent<Image>().color = navDotColors[1];
    }

    internal List<string> DropdownOptionList()
    {
        List<string> dropOptions = new List<string>();
        foreach (Scenario scenario in scenarios)
        {
            dropOptions.Add(scenario.titleName[0]);
        }
        return dropOptions;
    }


    public void DoDownButton()
    {
        Debug.Log("down");

        activeScenarioIndex++;
        CheckNavBounaries();
        ChangeNavDotColor();
        //DestroyDraggables();
        //SpawnDraggables();
        SetVocabList();

        SetGameTitle();
        StartCoroutine(SetGameImage());
    }

    public void DoUpButton()
    {
        Debug.Log("up");
        activeScenarioIndex--;
        CheckNavBounaries();
        ChangeNavDotColor();
        //DestroyDraggables();
        //SpawnDraggables();
        SetVocabList();
        SetGameTitle();
        StartCoroutine(SetGameImage());
    }

    public void CheckNavBounaries()
    {
        if (activeScenarioIndex == 0)
        {
            upButton.interactable = false;
            downButton.interactable = true;
        }

        else if(activeScenarioIndex == numScenarios - 1)
        {
            upButton.interactable = true;
            downButton.interactable = false;
        }

        else
        {
            upButton.interactable = true;
            downButton.interactable = true;
        }
    }

    public void ChangeNavDotColor()
    {
        for (int i = 0; i < numScenarios; i++)
        {
            if (i == activeScenarioIndex)
            {
                navDots[i].GetComponent<Image>().color = navDotColors[1];
            }
            else
            {
                navDots[i].GetComponent<Image>().color = navDotColors[0];
            }
        }
    }

    public GameObject draggableContainer;

    internal void SpawnDraggables()
    {
        Vector2 scale = new Vector2(1, 1);
        int i = 0;
        draggableObjects = new List<GameObject>(scenarios[activeScenarioIndex].scenarioDraggableObjects);
        foreach (GameObject draggable in draggableObjects)
        {
            GameObject ourDraggable = Instantiate(draggable);
            ourDraggable.transform.SetParent(draggableContainer.transform);
            ourDraggable.transform.localPosition = vocabPositions[i];
            ourDraggable.transform.localScale = scale;
            i++;
        }
    }

    private void DestroyDraggables()
    {
        foreach (GameObject draggable in draggableObjects)
        {
            Destroy(draggable);
        }
    }

    private void SetGameTitle()
    {
        titleText.text = scenarios[activeScenarioIndex].titleName[1];
        titleTextEngl.text = scenarios[activeScenarioIndex].titleName[0];
        roomName.text = scenarios[activeScenarioIndex].scenarioType.ToString();
    }

    private void SetVocabList()
    {
        string vocabList = "";
        //string vocabListEngl = "";
        foreach(DraggableItem draggable in scenarios[activeScenarioIndex].scenarioDraggableItems)
        {
            string draggableString = draggable.wordString[0] + "  \t~\t" + draggable.wordString[1] + "\n";
            vocabList += draggableString;
            //string draggableStringEngl = draggable.wordString[0] + " ~\n";
            //vocabListEngl += draggableStringEngl;

        }
        vocabListText.text = vocabList;
        //vocabListTextEngl.text = vocabListEngl;
    }
    private IEnumerator SetGameImage()
    {
        float fadeDur = 0.15f;
        StartCoroutine(LerpAlphaDown(fadeDur));
        yield return new WaitForSeconds(fadeDur);
        gameSneakPeakImage.sprite = scenarios[activeScenarioIndex].gameOverBackgroundSprite;
        StartCoroutine(LerpAlphaUp(0.15f));
    }

    IEnumerator LerpAlphaUp(float duration)
    {
        List<Color> colors = new List<Color> { new Color(1, 1, 1, 0), new Color(1, 1, 1, 1) };
        
        float timeA = 0;
        while (timeA < duration)
        {
            gameSneakPeakImage.color = Color.Lerp(colors[0], colors[1], timeA / duration);
            timeA += Time.deltaTime;
            yield return null;
        }
        gameSneakPeakImage.color = colors[1];
    }

    IEnumerator LerpAlphaDown(float duration)
    {
        List<Color> colors = new List<Color> { new Color(1, 1, 1, 1), new Color(1, 1, 1, 0) };

        float timeA = 0;
        while (timeA < duration)
        {
            gameSneakPeakImage.color = Color.Lerp(colors[0], colors[1], timeA / duration);
            timeA += Time.deltaTime;
            yield return null;
        }
        gameSneakPeakImage.color = colors[1];
    }

    public void GoToScene()
    {
        SceneManager.LoadScene("Home");

        scenarioSetter.ChangeScenario(activeScenarioIndex);
    }
}