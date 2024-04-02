using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeController : MonoBehaviour
{
    //Selection
    public ScenarioSetter scenarioSetter;
    public int dialect = 1;
    public Scenario selectedScenarioSO;
    private DisplayScenario selectedScenarioUI;
    public GameObject draggableContainer;
    public List<DraggableItem> draggables;
    public List<GameObject> draggableObjects = new List<GameObject>();
    public int numObjects;
    internal DraggableItem activeSO;
    internal AudioSource audioSource;
    //Gameplay
    public bool inSelection = true;
    private int numItemsDropped;
    internal int curItem = 0;
    int numErrors = 0;
    public GameObject sceneAudButton;
    internal bool inInstruction = true;
    internal bool HighlightCorrectItem = false;
    internal bool draggingAllowed = false;
    public GameObject stars;
    internal GameObject dzCover;


    private void Start()
    {
        audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
    }

    public void SetSelectedStart()
    {
        Debug.Log("set selected");
        inSelection = false;
        selectedScenarioSO = scenarioSetter.currentScenario;
        selectedScenarioUI = GameObject.Find("Canvas").GetComponent<DisplayScenario>();
        
        numObjects = selectedScenarioSO.scenarioDraggableObjects.Count;

        //Debug.Log(numObjects);
        
        SetDraggableOrder();
        RandomizeDraggablePos();
        if (selectedScenarioSO.hideDzCoverImgAfter != -1) { SpawnDZCover(); }
    }

    //triggers rearranging of DraggableItems.  
    public void SetDraggableOrder()
    {
        Debug.Log("exc SetDraggableOrder()");
        if (draggables?.Any() ?? false) { draggables.Clear(); }
        List<DraggableItem> tempDraggables = new List<DraggableItem>(selectedScenarioSO.scenarioDraggableItems);

        int numBatches = tempDraggables.Select(v => v.GetBatchNum()).Distinct().Count();

        if (numBatches == 1)
        {
            OrderedDraggablesUnbatched(tempDraggables);
        }
        else
        {
            OrderedDraggablesBatched(tempDraggables, numBatches);
        }
    }

    //This will be run if there is no set ordering of the draggables as indicated by int batch in DraggableItem
    internal void OrderedDraggablesUnbatched(List<DraggableItem> draggablesCopy)
    {
        Debug.Log("exc OrderedDraggablesUnbatched()");
        List<int> randInts = RandomIntsOfLength(numObjects);

        for (int i = 0; i < numObjects; i++)
        {
            draggables.Add(draggablesCopy[randInts[i]]);
            draggables[i].ThisItemIndex(i);
        }
        SetDraggableItemIndex();
    }

    //This will be run if there is any set ordering of the draggables as indicated by int batch in DraggableItem
    internal void OrderedDraggablesBatched(List<DraggableItem> draggablesCopy, int numBatches)
    {
        //Debug.Log("num batches: " +numBatches);
        for (int i = 0; i < numBatches; i++)
        {
            List<DraggableItem> itemsInBatch = draggablesCopy.Where(v => v.GetBatchNum() == i).ToList();
            int numInBatch = itemsInBatch.Count();
            List<int> randInts = RandomIntsOfLength(numInBatch);
            //Debug.Log("num rand ints: " + randInts.Count);

            for (int j = 0; j < numInBatch; j++)
            {
                draggables.Add(itemsInBatch[randInts[j]]);
            }
        }
        SetDraggableItemIndex();
    }


    void SetDraggableItemIndex()
    {
        for (int i = 0; i < numObjects; i++)
        {
            draggables[i].ThisItemIndex(i);
        }
        ReorderDraggableObjectsList();
    }

    public void RandomizeDraggablePos()
    {
        Debug.Log("exc RandomizeDraggablePos()");
        List<int> randInts = RandomIntsOfLength(numObjects);
        //Debug.Log("num objects randomizing pos: " + draggableObjects.Count);

        for(int i = 0; i < numObjects; i++)
        {
            Vector2 randSlot = selectedScenarioSO.randSlots[randInts[i]];
            //Debug.Log(randSlot.x);
            draggableObjects[i].GetComponent<DisplayDraggable>().SetRandPos(randSlot);
        }
        StartCoroutine(InstructDragging());
    }

    public void ReorderDraggableObjectsList()
    {
        Debug.Log("exc ReorderDraggableObjectsList()");
        List<GameObject> tempObjs = new List<GameObject>();
        foreach(DraggableItem draggable in draggables)
        {
            Debug.Log(tempObjs.Count + " draggableItem: " + draggable.name);
            //tempObjs.Add(draggableObjects.Single(v => v.name.Contains(draggable.name)));
            tempObjs.Add(draggableObjects.FirstOrDefault(v => v.name.Contains(draggable.name)));
        }

        draggableObjects = new List<GameObject>(tempObjs);
    }

    public IEnumerator InstructDragging()
    {
        FadeAllTiles(true);
        yield return new WaitUntil(() => !audioSource.isPlaying);
        sceneAudButton.SetActive(false);
        yield return new WaitForSeconds(2f);
        if (draggables[curItem].IsInstructionCustom())
            {selectedScenarioUI.ShowCustomInstruction(draggables[curItem].GetItemInstruction());}
        else{ selectedScenarioUI.ShowRepeater(draggables[curItem].GetItemInstruction());}
        
        PlayInstructionAud();
        sceneAudButton.SetActive(true);
        Debug.Log(draggables[curItem].name);
        yield return new WaitUntil(() => !audioSource.isPlaying);
        sceneAudButton.SetActive(true);
        inInstruction = false;
        FadeAllTiles(false);
    }

    internal void FadeAllTiles(bool fade)
    {
        draggableObjects.ForEach(x => x.GetComponent<DisplayDraggable>().FadeTileImage(fade));
    }

    public void PlayInstructionAud()
    {
        if (audioSource.isPlaying) { audioSource.Stop();  }
        if (!inSelection)
        {
            if (draggables[curItem].IsInstructionCustom()) {  StartCoroutine(InstructionAud()); }
            else { StartCoroutine(PairInstructionAud()); }

            IEnumerator InstructionAud()
            {
                audioSource.PlayOneShot(draggables[curItem].GetInstructionAudio());
                yield return new WaitUntil(() => !audioSource.isPlaying);
                draggingAllowed = true;
            }

            IEnumerator PairInstructionAud()
            {
                audioSource.PlayOneShot(selectedScenarioSO.GetRepeaterAudio());
                yield return new WaitUntil(() => !audioSource.isPlaying);
                audioSource.PlayOneShot(draggables[curItem].GetAudio());
                yield return new WaitUntil(() => !audioSource.isPlaying);
                draggingAllowed = true;
            }
        }
    }

    public void CountItemsLayered(bool correct)
    {
        draggingAllowed = false;
        FadeAllTiles(true);
        if (!correct)
        {
            selectedScenarioUI.DoFeedbackMedia(correct);

            numErrors++;
            if (numErrors == 3)
            {
                draggableObjects[curItem].GetComponent<DragItem>().HighlightCorrectItem();
            }
            draggingAllowed = true;
            FadeAllTiles(false);
        }

        else if (correct)
        {
            numErrors = 0; numItemsDropped++;

            //Debug.Log(draggableObjects[curItem].transform.localPosition);
            InstantiateStars(draggables[curItem].dropPos, 0.3f);
            if(selectedScenarioSO.hideDzCoverImgAfter == curItem) { DestroyDZCover(); }

            StartCoroutine(AudAfterCorrDrop());
        }
    }

    internal void SpawnDZCover()
    {
        Debug.Log("should instantiate cover");
        dzCover = Instantiate(selectedScenarioSO.dzCover);
        dzCover.transform.SetParent(GameObject.Find("Draggable Container").transform);
        dzCover.transform.localScale = new Vector3(1, 1, 1);
        dzCover.transform.localPosition = selectedScenarioSO.dzCoverPos;
    }

    internal void DestroyDZCover()
    {
        Destroy(dzCover);
    }

    //only used for testing purposes
    public void TriggerStar()
    {
        InstantiateStars(new Vector2(300,300), 0);
    }

    public void InstantiateStars(Vector2 starPosition, float delaySeconds)
    {
        activeSO = draggables[curItem];
        StartCoroutine(InstantiateOnDelay());
        IEnumerator InstantiateOnDelay()
        {
            yield return new WaitForSeconds(delaySeconds);
            Instantiate(stars);
        }
    }

    public IEnumerator AudAfterCorrDrop()
    {
        audioSource.PlayOneShot(draggables[curItem].GetAudio());
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => !audioSource.isPlaying);
        selectedScenarioUI.DoFeedbackMedia(true);
        
        yield return new WaitUntil(() => !audioSource.isPlaying);
        if (numItemsDropped == draggables.Count)
        {
            yield return new WaitForSeconds(1);
            yield return new WaitWhile(() => audioSource.isPlaying);
            StartCoroutine(Success());
        }

        else
        {
            if (selectedScenarioSO.replaceAsDropped) { draggableObjects[curItem].GetComponent<DisplayDraggable>().ShowLayerImage(false); }
            curItem++;
            StartCoroutine(InstructDragging());
        }
    }

    public IEnumerator Success()
    {
        new WaitForSeconds(2);
        selectedScenarioUI.ShowSuccessText();
        //big particle effect
        yield return new WaitWhile(() => audioSource.isPlaying);
        audioSource.PlayOneShot(selectedScenarioSO.successPhraseAud);
        yield return new WaitWhile(() => audioSource.isPlaying);
        yield return new WaitForSeconds(1);
        selectedScenarioUI.ClearSuccessText();
        StartCoroutine(ShowCompletionScreen());
    }
    public GameObject gameOverScreen;

    public IEnumerator ShowCompletionScreen()
    {
        gameOverScreen.SetActive(true);
        DestroyDraggables();
        yield return new WaitWhile(() => audioSource.isPlaying);
    }

    bool DestroyDraggables()
    {
        foreach (Transform child in draggableContainer.transform)
        {
            DestroyImmediate(child.gameObject);
        }
    
        draggableObjects.Clear();

        if(draggableContainer.transform.childCount == 0)
        {
            return true;
        }
        return false;
    }

    void DestroyFeedback() {
        List<GameObject> gameObjects = GameObject.FindGameObjectsWithTag("Feedback").ToList();

        gameObjects.ForEach(x => Destroy(x));
    }

    public void RemoveImageComponent(GameObject removeImageFrom)
    {
        Debug.Log(removeImageFrom.name);
        removeImageFrom.GetComponent<Image>().enabled = false;
    }

    public void SetDialect(int currDialect)
    {
        dialect = currDialect;
        draggables.ForEach(x => x.SetCurrDialect(currDialect));
    }

    public void Replay()
    {
        StopAllCoroutines();
        DestroyFeedback();
        numItemsDropped = 0;
        curItem = 0;
        
        StartCoroutine(PositionNewDraggables());

        IEnumerator PositionNewDraggables()
        {
            //yield return new WaitUntil(()=>draggableContainer.transform.childCount == 0);
            yield return new WaitUntil(() => DestroyDraggables());
            selectedScenarioUI.SpawnDraggables();
            SetSelectedStart();
        }
    }

    //Helper functions
    internal List<int> RandomizeInts(int n)
    {
        List<int> listIntsOfLength = CreateListIntsOfLength(n);
        List<int> randomized_ints = new List<int>();
        for (int i = 0; i < n; i++)
        {
            int r = Random.Range(0, listIntsOfLength.Count);
            randomized_ints.Add(listIntsOfLength[r]);
            listIntsOfLength.Remove(listIntsOfLength[r]);
        }
        return randomized_ints;
    }

    internal List<int> CreateListIntsOfLength(int l)
    {
        List<int> intsOfLength = new List<int>();
        for (int i = 0; i < l; i++)
        {
            intsOfLength.Add(i);
        }
        return intsOfLength;
    }

    internal List<int> RandomIntsOfLength(int l)
    {
        return RandomizeInts(l);
    }
}
