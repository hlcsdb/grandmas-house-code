using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Simultaneously rotates, changes color, and scales
// when duration == 4s
// from 0:00 to 1:20 -- transitions from color (1, 1, 1, 0.3f) to (1, 0.94f, 0, 1)
// from 1:20 to 2:40 --  transitions from color (1, 0.94f, 0, 1) to (1, 0.94f, 0, 0.5f)
// from 2:40 to 3:00 -- transitions from colour (1, 0.94f, 0, 0.5f) to (1, 1, 1, 0.3f)

// from  0:00 to 2:48 -- scales from x1 to x2
// from  2:48 to 3:40 -- scales from x2 to x0

// from  0:00 to 2:42 -- rotates from 0deg to 180deg
// from  2:42 to 3:40 -- rotates from 180deg to 0deg


public class StarFeedback : MonoBehaviour
{
    GameObject star;
    Color[] starColors = new Color[] { new Color(1, 1, 1, 0.3f), new Color(1, 0.94f, 0, 1), new Color(1, 0.94f, 0, 0.5f), new Color(1, 1, 1, 0.3f) };
    Image starImage;
    float activeDuration = 4.0f;
    Quaternion rotation = Quaternion.Euler(0, 0, 180);


    void Start()
    {
        star = gameObject;
        //Transform parent = GameObject.Find("Challenge Manager").GetComponent<ChallengeController>().activeDraggableObject.transform;
        transform.SetParent(GameObject.Find("Play UI").transform);
        //transform.SetParent(parent);
        transform.localPosition = GameObject.Find("Challenge Manager").GetComponent<ChallengeController>().activeSO.dropPos;
        starImage = star.GetComponent<Image>();
        StartCoroutine(LerpStars());
    }

    IEnumerator LerpStars()
    {
        StartCoroutine(GrowShrink());
        StartCoroutine(TwistAndBack());
    
        for (int i = 1; i < starColors.Length; i++)
        {
            StartCoroutine(LerpColor(starColors[i-1], starColors[i], activeDuration/starColors.Length));
            yield return new WaitUntil(() => starImage.color == starColors[i]);
        }

        Destroy(gameObject);
    }

    IEnumerator LerpColor(Color startValue, Color endValue, float duration)
    {
        float timeA = 0;
        while (timeA < duration)
        {
            starImage.color = Color.Lerp(startValue, endValue, timeA / duration);
            timeA += Time.deltaTime;
            yield return null;
        }
        
        starImage.color = endValue;
        //Debug.Log(starImage.color);
    }


    private IEnumerator GrowShrink()
    {
        yield return StartCoroutine(ScaleObj(2, (activeDuration * 0.8f)));
        yield return StartCoroutine(ScaleObj(0, (activeDuration * 0.2f)));
    }

    private IEnumerator ScaleObj(float maxSize, float scaleDur)
    {
        float timeB = 0;
        scaleDur *= 0.9f;
        Vector2 startScale = transform.localScale;
        Vector2 maxScale = new Vector2(maxSize, maxSize);
        do
        {
            transform.localScale = Vector3.Lerp(startScale, maxScale, timeB / scaleDur);
            timeB += Time.deltaTime;
            yield return null;
        }
        while (timeB < scaleDur);
        timeB = 0;
        
    }


    private IEnumerator TwistAndBack()
    {
        //Debug.Log("Start twist");

        yield return StartCoroutine(Twist(rotation, (activeDuration * 0.75f)));
        yield return StartCoroutine(Twist(Quaternion.Euler(0, 0, 0), (activeDuration * 0.25f)));
    }

    IEnumerator Twist(Quaternion endRotation, float dur)
    {
        dur *= 0.9f;

        float time = 0;
        Quaternion startValue = transform.rotation;
        while (time < dur)
        {
            transform.rotation = Quaternion.Lerp(startValue, endRotation, time / dur);
            time += Time.deltaTime;
            yield return null;
        }
        transform.rotation = endRotation;
    }
}
