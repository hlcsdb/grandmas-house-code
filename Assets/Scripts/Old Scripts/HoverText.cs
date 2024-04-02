using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class HoverText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
    public DraggableItem draggable;
    private float timer = 0.0f;
    private float scaleDur = 0.1f;
    private float maxSize = 1.05f;
    private float minSize = 0.95f;
    private float rotateAmount = 10.0f; //Amount to rotate in degrees
    private ChallengeController currSceneController;
    //private FreeplayController currSceneController;
    private GameObject hovertext;
    public AudioSource audioSource;
    internal DisplayDraggable draggableUI;

    private void Awake()
    {
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Challenge"))
        {
            currSceneController = GameObject.Find("Challenge Manager").GetComponent<ChallengeController>();
            //audioSource = currSceneController.audioSource;
        }
        //if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Freeplay"))
        //{
        //    FreeplayController currSceneController = GameObject.Find("Freeplay Controller").GetComponent<FreeplayController>();
        //    audioSource = FreeplayController.audioSource;
        //}
        //draggable = gameObject.Draggable;
        draggableUI = gameObject.GetComponent<DisplayDraggable>();
        hovertext = gameObject.transform.GetChild(1).gameObject;
        Debug.Log("name of hovertext obj: " + gameObject.transform.GetChild(1).name);
        Debug.Log(draggable.WordString());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(Grow());

        if (currSceneController.inSelection)
        {
            draggableUI.SetWord();
            audioSource.Play();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(Shrink());
        
        if (currSceneController.inSelection)
        {
            //hovertext.SetActive(false);
            draggableUI.HideWord();
            audioSource.Stop();
        }
    }

    private IEnumerator Grow()
    {
        Vector2 startScale = transform.localScale;
        Vector2 maxScale = new Vector2(maxSize, maxSize);
        do
        {
            transform.localScale = Vector3.Lerp(startScale, maxScale, timer / scaleDur);
            timer += Time.deltaTime;
            yield return null;
        }
        while (timer<scaleDur);
        timer = 0;
    }

    private IEnumerator Shrink()
    {
        Vector2 startScale = transform.localScale;
        Vector2 minScale = new Vector2(minSize, minSize);
        do
        {
            transform.localScale = Vector3.Lerp(startScale, minScale, timer / scaleDur);
            timer += Time.deltaTime;
            yield return null;
        }
        while (timer < scaleDur);
        timer = 0;
    }

    public void HighlightCorrectItem()
    {
        //StartCoroutine(TwistDraggable());
        StartCoroutine(GrowShrinkLoop());
    }

    public IEnumerator TwistDraggable()
    {
        Debug.Log("twisting");
        transform.Rotate(0.0f, 0.0f, rotateAmount / 2);
        for (int i = 0; i < 2; i++)
        {
            if (transform.rotation.z > 0)
            {
                transform.Rotate(0.0f, 0.0f, -rotateAmount);
            }
            else if (transform.rotation.z < 0)
            {
                transform.Rotate(0.0f, 0.0f, rotateAmount);
            }
            yield return new WaitForSeconds(0.07f);
        }
        transform.Rotate(0.0f, 0.0f, -(rotateAmount / 2));
    }

    public IEnumerator GrowShrinkLoop()
    {
        for (int i = 0; i < 2; i++)
        {
            StartCoroutine(Grow());
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(Shrink());
            yield return new WaitForSeconds(0.2f);
        }
    }

    public IEnumerator ShowLabelForSeconds(int seconds)
    {
        hovertext = gameObject.transform.GetChild(0).gameObject;
        hovertext.SetActive(true);
        yield return new WaitForSeconds(4);
        hovertext.SetActive(false);
    }
}
