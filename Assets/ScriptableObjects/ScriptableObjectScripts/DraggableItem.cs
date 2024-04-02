using UnityEngine;

public enum ItemType
{
    Default,
    Food,
    Utensil,
    Fabric,
    Dishware,
    Jewlery,
    Clothing,
    Toy
}

[CreateAssetMenu(fileName = "New Draggable Item", menuName = "Draggable/DraggableItem")]

public class DraggableItem : ScriptableObject
{
    public ItemType type;
    public string description;
    public string[] wordString;
    public string[] customInstructionText = new string[2];

    public Sprite[] draggableImage; //0 = draggable image, 1 = layer image

    //public GameObject dropZone; //minx maxx miny maxy
    public int thisRandIndex;
    public bool dragged;
    public bool correct;
    public int dialect = 1;
    public Color[] tileStateOutlineColors = new Color[] { new Color (1, 1, 1, 1), new Color(0.9882f, 0.9333f, 0.1294f, 1), new Color(0.9490f, 0.3294f, 0.1882f, 1f) };
    public AudioClip audioClip;
    public AudioClip draggableInstruction;
    public Vector2 dropPos;
    public Vector2 dropSize;
    public Vector2 selectionPos;
    public Vector2 startSize = new Vector3(1, 1, 1);
    public Vector2 startPos;
    public Vector2[] dzB;
    public int batch = 0;
    internal int siblingIndex;
    

    public void SetCurrDialect(int currDialect)
    {
        dialect = currDialect;
    }

    public string Description()
    {
        return description;
    }

    public string WordString()
    {
        return wordString[dialect];
    }

    public string InstructionString()
    {
        string instruction = customInstructionText[dialect];
        return instruction;
    }

    public bool IsInstructionCustom()
    {
        return customInstructionText[dialect] != ""; 
    }

    public string GetItemInstruction()
    {
        string instruction = IsInstructionCustom() ? InstructionString() : WordString();
        return instruction;
    }

    public Sprite GetImage(int draggableStatus)
    {
        return draggableImage[draggableStatus];
    }

    public int GetBatchNum()
    {
        return batch;
    }
    public void SetIfCorrect(bool yes)
    {
        correct = yes;
    }

    public bool IsItCorrect()
    {
        return correct;
    }

    public bool IsItDragged()
    {
        return dragged;
    }

    public void Dragged(bool isDragged)
    {
        dragged = isDragged;
    }

    public void ResetSO()
    {
        dialect = 1;
        dragged = false;
        correct = false;
    }

    public AudioClip GetAudio(){
        return audioClip;
    }

    public AudioClip GetInstructionAudio()
    {
        return draggableInstruction;
    }

    public void ThisItemIndex(int thisIndex)
    {
        thisRandIndex = thisIndex;
    }

    public void SetSiblingIndex(int index)
    {
        siblingIndex = index;
    }
}
