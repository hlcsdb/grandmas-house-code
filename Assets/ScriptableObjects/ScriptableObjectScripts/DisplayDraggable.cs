using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DisplayDraggable : MonoBehaviour
{
    public DraggableItem draggable;
    public TextMeshProUGUI wordString;
    public Image draggableArtwork;
    public GameObject tile;
    public Vector2 startPos;
    //internal Vector2 startRandPos;
    public Vector2 rectTransform;
    internal Vector2 randPos;
    public int randI;
    public GameObject textBackground;
    internal int siblingIndex;
    int backgroundColorState;

    public void Start()
    {
        draggableArtwork.sprite = draggable.GetImage(0);
        draggable.startPos = transform.localPosition;
        randI = draggable.thisRandIndex;
        siblingIndex = transform.GetSiblingIndex();
    }

    public void SetRandPos(Vector2 rPos)
    {
        HideWord();
        randPos = rPos;
        transform.localPosition = randPos;
    }

    public void ColourTileOutline(int state)
    {
        backgroundColorState = state;

        tile.transform.GetChild(0).GetComponent<Image>().color = draggable.tileStateOutlineColors[state];
    }

    public void FadeTileImage(bool fade)
    {
        if (!draggable.IsItDragged() && fade)
        {
            draggableArtwork.GetComponent<Image>().color = new Color(1, 1, 1, 0.4f);
        }
        else { draggableArtwork.GetComponent<Image>().color = new Color(1, 1, 1, 1); }
    }

    public int BackgroundColorState()
    {
        return backgroundColorState;
    }

    public void SetWord()
    {
        textBackground.SetActive(true);
        wordString.text = draggable.WordString();
    }

    public void ShowHideTile()
    {
        ColourTileOutline(0);

        if (tile.activeSelf)
        {
            tile.SetActive(false);
        }
        else
        {
            tile.SetActive(true);
        }
    }

    public void HideWord()
    {
        textBackground.SetActive(false);

        wordString.text = "";
    }

    public void DroppedDraggable()
    {
        draggableArtwork.sprite = draggable.GetImage(1);
        transform.localScale = draggable.dropSize;
        transform.localPosition = draggable.dropPos;
        draggable.Dragged(true);
        ColourTileOutline(0);
        ShowHideTile();
        transform.SetSiblingIndex(siblingIndex);
    }

    public void ReturnDraggable()
    {
        Debug.Log(BackgroundColorState());
        
        transform.localPosition = ThisRandomPos();
        transform.SetSiblingIndex(siblingIndex);
        ColourTileOutline(0);
    }


    internal void ShowLayerImage(bool show)
    {
        Debug.Log(transform.GetChild(1).gameObject.name);
        transform.GetChild(1).gameObject.SetActive(show);
    }

    public void ResetDraggableDisplay()
    {
        ColourTileOutline(0);
        draggableArtwork.sprite = draggable.GetImage(0);
        transform.localPosition = draggable.startPos;
        ShowLayerImage(true);
        tile.SetActive(true);
        transform.localScale = draggable.startSize;
    }

    public Vector2 ThisRandomPos()
    {
        return randPos;
    }


    public bool OverlappingDropZone()
    {
        Vector2[] dzB = draggable.dzB; //Set in SceneSO and declared to all draggableitems in scene at start.

        if (transform.localPosition.x > dzB[0].x && transform.localPosition.x < dzB[2].x && transform.localPosition.y > dzB[0].y && transform.localPosition.y < dzB[1].y)
        {
            return true;
        }
        return false;
    }
}