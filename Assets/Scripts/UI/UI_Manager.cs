using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private NumberButton prefabNumberButton;
    [SerializeField] private Transform panelNumberButtons;
    [SerializeField] private Item chooseItem;
    [SerializeField]private GameField gameField;
    private void Awake()
    {
        GenerateNumbers();
    }
    void Start()
    {
        GlobalEventUI.OnChooseCellInFiled += OnChooseCellInField;
        GlobalEventUI.OnClickNumber += OnClickNumber;
        GlobalEventUI.OnClickClearButton += OnClearCell;
    }
    private void GenerateNumbers()
    {
        for (int i = 1; i <= 9; i++)
        {
            NumberButton button=Instantiate(prefabNumberButton,panelNumberButtons);
            button.SetNumber(i);
            button.name = i.ToString();
        }
    }
    private void OnChooseCellInField(Item item)
    {
        chooseItem = item;
    }
    private void OnClickNumber(int number)
    {
        if (chooseItem != null)
        {
            if (gameField.isNoteMode)
            {
                chooseItem.AddNote(number);
            }
            else 
            {
                chooseItem.SetNumber(number);
                GlobalEventUI.InvokeOnCheckCell(chooseItem);
            }

        }
    }
    public void OnClearCell()
    {
        if (chooseItem != null)
        {
            chooseItem.SetNumber(0);
            chooseItem.SetHighligth(TypeHighligth.Chooseline);
        }
    }
    public void OnGenerateField()
    {
        int userValue;
        if (int.TryParse(inputField.text, out userValue))
        {
            GlobalEventUI.InvokeOnClickGenerateFiledButton(userValue);
        }
    }
    public void OnCheckCell()
    {
        GlobalEventUI.InvokeOnCheckField();
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void OnDestroy()
    {
        GlobalEventUI.OnChooseCellInFiled -= OnChooseCellInField;
        GlobalEventUI.OnClickNumber -= OnClickNumber;
        GlobalEventUI.OnClickClearButton -= OnClearCell;
    }
}
