using UnityEngine;
using UnityEngine.EventSystems;

public class PictureFrame : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float startTilt;

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.rotation = Quaternion.Euler(0, 0, startTilt * -1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.rotation = Quaternion.Euler(0, 0, startTilt);
    }
}

