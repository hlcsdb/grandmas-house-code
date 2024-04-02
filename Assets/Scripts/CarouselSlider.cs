using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarouselSlider : MonoBehaviour
{
    public int setQuant;
    public int slideNum = 0;
    public Button carouselRight;
    public Button carouselLeft;
    public GameObject carouselPositions;
    internal List<GameObject> carouselPositionList;
    private GameObject carouselDestination;
    private float speed = 0.5f;
    public GameObject navDots;
    private Color navShade = new Color32(250, 90, 220, 255);
    private Color navClear = new Color32(255, 255, 255, 255);
    public AudioSource audioSource;
    public ChallengeController challengeManager;

    // Start is called before the first frame update
    void Start()
    {
        challengeManager = GameObject.Find("Challenge Manager").GetComponent<ChallengeController>();
        carouselPositionList = GetPositionList();
    }

    public void SlideRight() //left button
    {
        UnShadeNavDot();
        slideNum--;
        ShadeNavDot();
        CheckNavigation();
        audioSource.Play();
        StartCoroutine(Slide(carouselPositionList[slideNum]));
    }

    public void SlideLeft() //right button
    {
        UnShadeNavDot();
        slideNum++;
        ShadeNavDot();
        CheckNavigation();
        audioSource.Play();
        StartCoroutine(Slide(carouselPositionList[slideNum]));
    }

    IEnumerator Slide(GameObject destinationPosObj) // dir indicates left or right (+/-)
    {
        float elapsedTime = 0;


        while (elapsedTime < speed)
        {
            transform.position = Vector3.Lerp(transform.position, destinationPosObj.transform.position, (elapsedTime / speed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //transform.position = gotoPosition;
        //yield return null;
    }

    void CheckNavigation()
    {
        if(carouselPositionList.Count == 1)
        {
            carouselRight.interactable = false;
        }
        
        if (slideNum > 0)
        {
            carouselLeft.interactable = true;
        }

        else
        {
            carouselLeft.interactable = false;
        }

        if (slideNum < carouselPositionList.Count - 1)
        {
            carouselRight.interactable = true;
        }
        else
        {
            carouselRight.interactable = false;
        }
    }

    void UnShadeNavDot()
    {
        GameObject navDot = navDots.transform.GetChild(slideNum).gameObject;
        navDot.GetComponent<Image>().color = navClear;
    }

    void ShadeNavDot()
    {
        GameObject navDot = navDots.transform.GetChild(slideNum).gameObject;
        navDot.GetComponent<Image>().color = navShade;
    }

    public void ResetSlider()
    {
        UnShadeNavDot();
        slideNum = 0;
        StartCoroutine(Slide(carouselPositionList[slideNum]));
        ShadeNavDot();
        CheckNavigation();
    }

    public List<GameObject> GetPositionList()
    {
        //if (carouselPositionList.Count > 0)
        //{
        //    carouselPositionList.Clear();
        //}

        for (int i = 0; i < carouselPositions.transform.childCount; i++)
        {
            if (carouselPositions.transform.GetChild(i).gameObject.activeSelf)
            {
                Debug.Log("position active");

                //carouselPositionList.Add(carouselPositions.transform.GetChild(i).gameObject);
            }
            
        }
        return carouselPositionList;
    }
}

