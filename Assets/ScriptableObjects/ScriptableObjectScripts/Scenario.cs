using UnityEngine;
using System.Collections.Generic;

public enum ScenarioType
{
    Default,
    Kitchen,
    Bathroom,
    GrandmaRoom,
    KidRoom,
}

[CreateAssetMenu(fileName = "New Scenario", menuName = "Scenarios")]

public class Scenario : ScriptableObject
{
    public ScenarioType scenarioType;
    public string scenarioName;
    public List<GameObject> scenarioDraggableObjects = new List<GameObject>();
    public List<DraggableItem> scenarioDraggableItems;
    public string[] titleName = new string[2];
    public string[] openerPhrase = new string[2];
    public Vector2[] DZB;
    public bool replaceAsDropped;

    public int dialect;
    public string playSceneName;

    public string[] repeaterPhrase; //instructions that proceed the word of every item, eg. LAY DOWN THE plate // if this is different for different objects in the scenario, leave it blank and specify in DisplayDraggable
    public string[] successPhrase;

    public string[] completionPhrase;
    public int numDraggables;
    //public AudioSource audioS;
    public AudioClip titleAud;
    public AudioClip openerPhraseAud;
    public AudioClip repeaterPhraseAud; //instructions that proceed the word of every item, eg. LAY DOWN THE plate - IF CUSTOM TO A TILE.
    public AudioClip successPhraseAud;
    public AudioClip completionPhraseAud;
    public AudioClip htpExampleAud;

    public Sprite homeImage;
    
    public Sprite backgroundImage;
    public Sprite HTPSprite;
    public Sprite vocabBackgroundSprite;
    public Sprite gameOverBackgroundSprite;

    public Sprite dzImage;
    public Vector2 dzPos;
    public Vector2 dzRectDimensions;

    public GameObject dzCover;
    public Vector2 dzCoverPos;
    public int hideDzCoverImgAfter = -1;

    //Selection Feedback -- There must be the same number of feedback audioclip+sprite pairs. these can differ between correct and incorrect.
    public List<AudioClip> correctPhraseAud;
    public List<AudioClip> incorrectSelectionAud;
    public List<Sprite> incorrectDzSprites; 
    public List<Sprite> correctDzSprites;

    //set in scenario scriptable object and clear list here.
    public List<Vector2> randSlots;
    

    public void Awake()
    {
        
        //PopulateStartSlots(numDraggables);
    }

    internal AudioClip GetTitleAudioClip()
    {
        return titleAud;
    }

    internal void SetObjectsScenarioParam()
    {
        foreach(DraggableItem draggableSO in scenarioDraggableItems)
        {
            draggableSO.dzB = DZB;
        }
    }

    public void SetDialect(int currDialect)
    {
        dialect = currDialect;
        SetDialectOfDraggables(currDialect);
    }

    public void SetDialectOfDraggables(int currDialect)
    {
        //foreach(DraggableItem draggable in scenarioDraggables)
        //{
        //    draggable.SetCurrDialect(currDialect);
        //}
    }


    public AudioClip GetRepeaterAudio()
    {
        return repeaterPhraseAud;
    }
}