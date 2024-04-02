using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GenerateNavCarouselContent : MonoBehaviour
{
    int nPerPage = 4;
    int numPages = 0;
    public GameObject pagePositions;
    bool isSliding = false;
    public int carouselPage = 0;
    public GameObject[] pictureFramePrefabs;
    public ScenarioSetter scenarioSetter;
    public float moduleSpacing = 100f; // Adjust this value based on your desired spacing between modules

    float featureAreaWidth = 600f; //width between left and right navigation buttons (minus a bit to ensure margin)
    float featureAreaHeight = 400f;

    float pictureframeWidth; //to be calculated at start based on the width of the prefab
    float pictureframeHeight; //to be calculated at start based on the height of the prefab
    private float frameXMargin;//to be calculated at start based on the width of the prefab in relation to the total width
    private float frameYMargin;//to be calculated at start based on the width of the prefab in relation to the total width
    private GameObject[] scenarioFrameGOs;
    public Button rightButton;
    public Button leftButton;

    // Start is called before the first frame update
    void Start()
    {
        pictureframeWidth = pictureFramePrefabs[0].GetComponent<RectTransform>().rect.width;
        pictureframeHeight = pictureFramePrefabs[0].GetComponent<RectTransform>().rect.height;

        frameXMargin = (featureAreaWidth - (pictureframeWidth * 2)) / 3;
        frameYMargin = (featureAreaHeight - (pictureframeHeight * 2)) / 3; ;
        GenerateScenarioPictures();
    }

    private void GenerateScenarioPictures()
    {
        int scenarioCount = scenarioSetter.scenarios.Count;
        numPages = (int)Math.Ceiling((double)scenarioCount / (double)nPerPage);
        scenarioFrameGOs = new GameObject[scenarioCount];

        for (int scenarioIndex = 0; scenarioIndex < scenarioCount; scenarioIndex++)
        {
            InstantiatePictureFrame(scenarioIndex);
        }

        for (int i = 0; i < scenarioCount; i++)
        {
            int quartetIndex = i / nPerPage;
            float quartetOffset = 0.0f + (featureAreaWidth * quartetIndex);
            scenarioFrameGOs[i].transform.localPosition += new Vector3(quartetOffset, 0f, 0f);
        }
    }

    private void InstantiatePictureFrame(int scenarioIndex)
    {
        GameObject scenarioFrameGO = Instantiate(pictureFramePrefabs[scenarioIndex % pictureFramePrefabs.Length], transform);
        scenarioFrameGOs[scenarioIndex] = scenarioFrameGO;

        Vector3 offset = CalculateFrameOffsets(scenarioIndex);
        scenarioFrameGO.transform.localPosition = CalculateFrameOffsets(scenarioIndex);
        float frameTiltZ = GetTilt(scenarioIndex);
        scenarioFrameGO.transform.rotation = Quaternion.Euler(0, 0, frameTiltZ);
        scenarioFrameGO.GetComponent<PictureFrame>().startTilt = frameTiltZ;

        GameObject scenarioImageGO = scenarioFrameGO.transform.Find("ScenarioImage").gameObject;
        Image scenarioImageComponent = scenarioImageGO.GetComponent<Image>();
        scenarioImageComponent.sprite = scenarioSetter.scenarios[scenarioIndex].homeImage;

        RectTransform rectTransform = scenarioImageGO.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, rectTransform.rect.height);

        int index = scenarioIndex;
        Button scenarioButton = scenarioFrameGO.GetComponent<Button>();
        scenarioButton.onClick.AddListener(() => SetActiveScenario(index));

        scenarioFrameGO.SetActive(true);
    }

    private float GetTilt(int scenarioIndex)
    {
        System.Random random = new System.Random();
        float frameTiltZ = (float)random.NextDouble() * 1.5f + 3;
        frameTiltZ *= (scenarioIndex % 4 == 0 || scenarioIndex % 4 == 3) ? 1 : -1;
        return frameTiltZ;
    }

    private Vector3 CalculateFrameOffsets(int scenarioIndex)
    {
        int xOrder = (scenarioIndex % 4 == 0 || scenarioIndex % 4 == 2) ? 1 : 2;
        int yOrder = (scenarioIndex % 4 == 0 || scenarioIndex % 4 == 1) ? 2 : 1;

        int quartetIndex = scenarioIndex / 4;
        float quartetOffset = featureAreaWidth * quartetIndex;

        Vector3 offset = new Vector3(quartetOffset, 0f, 0f);

        offset.x += (frameXMargin*xOrder) + (pictureframeWidth / 2);
        offset.x += xOrder == 2 ? pictureframeWidth : 0;

        offset.y += (frameYMargin * yOrder) + (pictureframeHeight / 2);
        offset.y += yOrder == 2 ? pictureframeHeight : 0;

        Debug.Log("index: " + scenarioIndex + "; side: " + xOrder + "; top bottom: " + yOrder + "; offset X: " +offset.x + "; offset Y: " +offset.y);

        return offset;
    }

    private void SetActiveScenario(int scenarioIndex)
    {
        Debug.Log("scenario index: " + scenarioIndex);
        scenarioSetter.currentScenario = scenarioSetter.scenarios[scenarioIndex];
        scenarioSetter.currentScenarioIndex = scenarioIndex;
        GoToScenario(scenarioIndex);
        // Add code here to transition to the play scene
    }

    private void GoToScenario(int activeScenarioIndex)
    {
        SceneManager.LoadScene("Home");
        scenarioSetter.ChangeScenario(activeScenarioIndex);
    }

    public void slideCarouselBack() //uses left button
    {
        if (!isSliding){StartCoroutine(SlideRoutine(-1));}
    }

    public void slideCarouselForward() //uses right button
    {
        if (!isSliding) { StartCoroutine(SlideRoutine(1)); }
    }

    private IEnumerator SlideRoutine(int dir) //1 for right, -1 for left ; add more empty game objects to PagePositions in hierarchy, offset by -1200.
    {
        float slideDuration = 0.5f;
        isSliding = true;
        interactablesButtons();
        carouselPage += dir;
        Vector3 targetPosition = pagePositions.transform.GetChild(carouselPage).transform.position;

        float elapsedTime = 0f;
        while (elapsedTime < slideDuration)
        {
            float t = elapsedTime / slideDuration;
            transform.position = Vector3.Lerp(transform.position, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Debug.Log(targetPosition.x);
        transform.position = targetPosition;
        isSliding = false;
        interactablesButtons();
    }

    private void interactablesButtons()
    {
        rightButton.interactable = (isSliding || carouselPage == numPages-1) ? false : true;
        leftButton.interactable = (isSliding || carouselPage == 0) ? false : true;
    }
}
