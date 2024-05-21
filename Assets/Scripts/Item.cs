using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI textNumber;
    public int number;
    public TypeItem typeItem;
    public TypeHighligth typeHighligth;
    public System.Action<int> OnClick;
    public int row;
    public int column;
    public Image img;
    private bool isNoteMode;
    private HashSet<int> notes = new HashSet<int>();

    void Start()
    {
        img = GetComponentInChildren<Image>();
    }

    void Update() { }

    public void SetTypeItem(TypeItem typeItem)
    {
        this.typeItem = typeItem;
    }

    public void SetValidCell(bool isValid)
    {
        if (isValid)
        {
            img.color = Color.red;
        }
        else
        {
            img.color = Color.green;
        }
    }

    public void SetHighligth(TypeHighligth typeHighligth)
    {
        switch (typeHighligth)
        {
            case TypeHighligth.EmptyHighligth:
                img.color = Color.white;
                break;
            case TypeHighligth.MatchCell:
                img.color = Color.grey;
                break;
            case TypeHighligth.Chooseline:
                img.color = Color.blue;
                break;
        }
        this.typeHighligth = typeHighligth;
    }

    public void SetNumber(int number = 0)
    {
        if (this.typeItem == TypeItem.Empty)
        {
            if (number != 0)
            {
                this.number = number;
                textNumber.text = number.ToString();
            }
            else
            {
                this.number = number;
                textNumber.text = "";
            }
        }
    }

    public void ClearCell()
    {
        this.number = 0;
        textNumber.text = "";
        typeItem = TypeItem.Empty;
        notes.Clear();
    }

    public void SetPos(int row, int column)
    {
        this.row = row;
        this.column = column;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GlobalEventUI.InvokeOnChooseCellInFiled(this);
    }

    public void ToggleNoteMode()
    {
        isNoteMode = !isNoteMode;
        if (isNoteMode)
        {
            textNumber.fontSize = 30;
        }
        else
        {
            textNumber.fontSize = 60;
        }
    }

    public void AddNote(int number)
    {
        if (!isNoteMode || number == 0 || !notes.Add(number)) return;
        UpdateNoteText();
    }

    public void RemoveNote(int number)
    {
        if (!isNoteMode || !notes.Remove(number)) return;
        UpdateNoteText();
    }

    private void UpdateNoteText()
    {
        
        textNumber.text = string.Join(",", notes);
    }
}

public enum TypeItem 
{
    Empty,
    Fill
}
public enum TypeHighligth 
{
    EmptyHighligth,
    MatchCell,
    Chooseline

}