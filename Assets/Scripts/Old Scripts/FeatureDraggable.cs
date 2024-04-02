using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FeatureDraggable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float rotateAmount = 40.0f; //Amount to rotate in degrees
    //public float speed = 1.0f;  //speed of rotation in degrees/sec

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(TwistDraggable());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine(TwistDraggable());
    }

    public IEnumerator TwistDraggable()
    {
        transform.Rotate(0.0f, 0.0f, rotateAmount/2);
        for(int i = 0; i < 4; i++)
        {
            if(transform.rotation.z > 0)
            {
                Debug.Log(i + ": run left" + transform.rotation.z);
                transform.Rotate(0.0f, 0.0f, -rotateAmount);
            }
            else if (transform.rotation.z < 0)
            {
                Debug.Log(i + ": run right" + transform.rotation.z);
                transform.Rotate(0.0f, 0.0f, rotateAmount);
            }
            yield return new WaitForSeconds(0.07f);
        }

        transform.Rotate(0.0f, 0.0f, -(rotateAmount / 2));
        Debug.Log("toRotate" + transform.rotation.z);
    }
}
