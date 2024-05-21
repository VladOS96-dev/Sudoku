using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI textNumber;
    [SerializeField] private Color setValueColor;
    [SerializeField] private Color matchCellColor;
    [SerializeField] private Color chooselineColor;
    [SerializeField] private Color emptyColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color rightCellColor;
    public int row { get; private set; }
    public int column { get;private set; }
    public int Number { get; private set; }
    private Image img;
    private bool isNoteMode;
    private HashSet<int> notes = new HashSet<int>();
    private bool isValid;
    public TypeItem typeItem { get; private set; }

    void Start()
    {
        img = GetComponentInChildren<Image>();
       
    }

    public void SetValidCell(bool isValid)
    {
        img.color = isValid ? matchCellColor : rightCellColor;
        this.isValid = isValid;
    }

    public void SetHighligth(TypeHighligth typeHighligth)
    {
        switch (typeHighligth)
        {
            case TypeHighligth.EmptyHighligth:
                if (!isValid)
                {
                    img.color = emptyColor;
                }
                break;
            case TypeHighligth.MatchCell:
                img.color = matchCellColor;
                break;
            case TypeHighligth.Chooseline:
                if (!isValid)
                {
                    img.color = chooselineColor;
                }
                break;
            case TypeHighligth.SetValue:
                if (Number!=0) 
                { 
                    textNumber.color = setValueColor; 
                }
                
                break;
            case TypeHighligth.SelectedColor:
                img.color = selectedColor;
                break;
        }
    }
    public void SetRowColumn(int row,int column) 
    {
    this.row=row;
        this.column=column;
    }
    public void SetNumber(int number = 0)
    {
        textNumber.text = number != 0 ? number.ToString() : "";
        Number = number;

    }

    public void ClearCell()
    {
        textNumber.text = "";
        Number = 0;
        notes.Clear();
        isNoteMode= false;
        isValid= false;
        textNumber.color = Color.black;
        textNumber.fontSize = 60;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GlobalEventUI.InvokeOnChooseCellInFiled(this);
    }

    public void ToggleNoteMode()
    {
        isNoteMode = true;
        Number = 0;
        if (notes.Count==0)
        {
            textNumber.fontSize = isNoteMode ? 30 : 60;
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
    public void SetTypeItem(TypeItem typeItem)
    {
        this.typeItem = typeItem;
    }
    private void UpdateNoteText()
    {
        textNumber.text = string.Join(",", notes);
    }

}

public enum TypeItem 
{
    Fill,
    Empty
    
}
public enum TypeHighligth 
{
    EmptyHighligth,
    MatchCell,
    Chooseline,
    SetValue,
    SelectedColor

}