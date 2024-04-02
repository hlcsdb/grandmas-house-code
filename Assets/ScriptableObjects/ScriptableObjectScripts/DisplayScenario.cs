using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayScenario : MonoBehaviour
{
    public ScenarioSetter scenarioSetter;
    public Scenario scenario;
    public Transform draggableContainer;

    //public GameObject scenariosInHierarchy;

    public TextMeshProUGUI sceneText;
    public TextMeshProUGUI scenarioName;
    public TextMeshProUGUI openerText;
    public TextMeshProUGUI openerTextEngl;
    public TextMeshProUGUI successText;
    public TextMeshProUGUI successTextEngl;
    public TextMeshProUGUI completionText;
    public TextMeshProUGUI completionTextEngl;


    public Image backgroundImage;
    public Image HTPImage;
    public Image vocabBackground;
    public Image gameOverBackground;
    internal ChallengeController challengeController;
    private List<Vector2> startSlots;
    public GameObject dzGO;

    //AUDIO
    internal AudioSource audSource;
    internal AudioClip sceneDescriptionAud;
    internal AudioClip openerPhraseAud;
    internal AudioClip repeaterPhraseAud; //instructions that proceed the word of every item, eg. LAY DOWN THE plate
    internal AudioClip successPhraseAud;
    internal AudioClip completionPhraseAud;

    public AudioButtonUI HTPexampleButton;
    public AudioOnLoad openerPhraseButton;
    public AudioOnLoad completionPhraseButton;

    public List<Vector2> vocabPositions = new List<Vector2>() { new Vector2(-220,46), new Vector2(-60,46), new Vector2(100,46), new Vector2(260,46), new Vector2(-220, -74), new Vector2( -60, - 74), new Vector2(100,-74), new Vector2(260,-74) };

    // Start is called before the first frame update
    void Start()
    {
        challengeController = GameObject.Find("Challenge Manager").GetComponent<ChallengeController>();
        scenario = scenarioSetter.currentScenario;
        audSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
        SetScenario();
    }

    //triggered from ChallengeController.cs
    internal void SetScenario()
    {
        SetVocabScreen();
        SpawnDraggables();
        SetHTPScreen();
        SetPlayScreen();
        SetGameOverScreen();
    }

    internal void SetVocabScreen()
    {
        vocabBackground.sprite = scenario.vocabBackgroundSprite;
        EmptyScenarioText();
    }

    internal void SetHTPScreen()
    {
        HTPImage.sprite = scenario.HTPSprite;
        openerText.text = scenario.openerPhrase[1];
        openerTextEngl.text = scenario.openerPhrase[0];
        openerPhraseButton.SetAudioclip(scenario.openerPhraseAud);
        HTPexampleButton.SetAudioClip(scenario.htpExampleAud);
    }

    internal void SetPlayScreen()
    {
        backgroundImage.sprite = scenario.backgroundImage;
        SetDzImage();
        repeaterPhraseAud = scenario.repeaterPhraseAud;
        scenario.SetObjectsScenarioParam();
        //successPhraseAud = scenario.successPhraseAud;
    }

    internal void SetGameOverScreen()
    {
        completionPhraseButton.audioOnActive = scenario.completionPhraseAud;
        gameOverBackground.sprite = scenario.gameOverBackgroundSprite;
        completionText.text = scenario.completionPhrase[1];
        completionTextEngl.text = scenario.completionPhrase[0];
    }

    internal void SpawnDraggables()
    {
        Debug.Log("exc SpawnDraggables()");
        ResetDraggableSOs();
        Vector2 scale = new Vector2(1, 1);
        int i = 0;
        foreach(GameObject draggable in scenario.scenarioDraggableObjects)
        {
            GameObject ourDraggable = Instantiate(draggable);
            ourDraggable.transform.SetParent(draggableContainer);
            ourDraggable.transform.localPosition = vocabPositions[i];
            ourDraggable.transform.localScale = scale;
            challengeController.draggableObjects.Add(ourDraggable);
            i++;
        }
    }

    internal void ResetDraggableSOs()
    {
        scenario.scenarioDraggableItems.ForEach(x => x.ResetSO());
      
    }


    internal void SetDzImage()
    {
        var dzImageRectTransform = dzGO.transform as RectTransform;

        dzImageRectTransform.sizeDelta = scenario.dzRectDimensions;
        dzGO.transform.localPosition = scenario.dzPos;

        ChangeDZImage(scenario.dzImage);
    }

    internal void ChangeDZImage(Sprite imageToSet)
    {
        dzGO.GetComponent<Image>().sprite = imageToSet;
    }

    public void ShowRepeater(string wordText)
    {
        sceneText.text = ""+ scenario.repeaterPhrase[1] + " " + wordText + ".";
    }

    public void ShowCustomInstruction(string customInstructionText)
    {
        sceneText.text = "" + customInstructionText;
    }

    public void EmptyScenarioText()
    {
        sceneText.text = "";
        successText.text = "";
        successTextEngl.text = "";
    }

    public void ShowSuccessText()
    {
        sceneText.text = "";
        successText.text = scenario.successPhrase[1];
        successTextEngl.text = scenario.successPhrase[0];
    }

    public void ClearSuccessText()
    {
        successText.text = "";
        successTextEngl.text = "";
    }

    public void ShowCompletionText()
    {
        sceneText.text = scenario.completionPhrase[scenario.dialect];
    }

    public void ShowScenarioName()
    {
        scenarioName.text = scenario.scenarioName;
    }

    public void BackToSelection()
    {
        ShowScenarioName();
        EmptyScenarioText();
    }

    internal void DoFeedbackMedia(bool correct)
    {
        int numAudio = correct? scenario.correctPhraseAud.Count: scenario.incorrectSelectionAud.Count;

        int feedbackInt = Random.Range(0, numAudio);
        AudioClip feedbackAud = correct ? scenario.correctPhraseAud[feedbackInt]: scenario.incorrectSelectionAud[feedbackInt];
        Sprite feedbackSprite = correct ? scenario.correctDzSprites[feedbackInt] : scenario.incorrectDzSprites[feedbackInt];

        StartCoroutine(DoFeedbackMediaSequence());

        IEnumerator DoFeedbackMediaSequence()
        {
            ChangeDZImage(feedbackSprite);
            yield return new WaitUntil(() => !audSource.isPlaying);
            audSource.PlayOneShot(feedbackAud);
            yield return new WaitUntil(() => !audSource.isPlaying);
            ChangeDZImage(scenario.dzImage);
        }
    }
}
