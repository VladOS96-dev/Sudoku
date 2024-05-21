using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private NumberButton prefabNumberButton;
    [SerializeField] private Transform panelNumberButtons;
    private Item chooseItem;
    [SerializeField]private GameField gameFieldPresenter;
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private GameObject RetryPanel;
    [SerializeField] private Image buttonNote;
    private void Awake()
    {
        GenerateNumbers();
      
    }

    void Start()
    {
        GlobalEventUI.OnChooseCellInFiled += OnChooseCellInField;
        GlobalEventUI.OnClickNumber += OnClickNumber;
        GlobalEventUI.OnClickClearButton += OnClearCell;
        GlobalEventUI.OnActiveWinPanel += ActiveWinPanel;

    }

    private void GenerateNumbers()
    {
        for (int i = 1; i <= 9; i++)
        {
            NumberButton button = Instantiate(prefabNumberButton, panelNumberButtons);
            button.SetNumber(i);
            button.name = i.ToString();
        }
    }

    private void OnChooseCellInField(Item item)
    {
        if (item.typeItem==TypeItem.Empty)
        {
            if (chooseItem != null)
            {
                chooseItem.SetHighligth(TypeHighligth.SetValue);
            }
            chooseItem = item;
            chooseItem.SetHighligth(TypeHighligth.SelectedColor);
        }

    }
    public void ToggleNote()
    {
        if (chooseItem != null)
        {
            GlobalEventUI.InvokeOnToggleNote(chooseItem);
            if (gameFieldPresenter.IsNoteMode)
            {
                buttonNote.color = Color.green;
            }
            else
            {
                buttonNote.color = new Color32(68,87,182,255);
            }
        }
        
    }
    private void OnClickNumber(int number)
    {
        if (chooseItem != null)
        {
            if (gameFieldPresenter.IsNoteMode)
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
    public void ActiveWinPanel(bool isCorrect)
    {

        if (isCorrect)
        {
            StartCoroutine(DelayActivePanel(WinPanel));
        }
        else
        {
            StartCoroutine(DelayActivePanel(RetryPanel));
        }
    }
    IEnumerator DelayActivePanel(GameObject panel)
    {
        panel.SetActive(true);
        yield return new WaitForSeconds(3);
        panel.SetActive(false);
    }
    public void LoadGame()
    {
        gameFieldPresenter.LoadField();
    }
    public void SaveGame()
    {
        gameFieldPresenter.SaveField();
    }
    public void OnClearCell()
    {
        if (chooseItem != null)
        {
            chooseItem.ClearCell();
            chooseItem.SetHighligth(TypeHighligth.Chooseline);
        }
    }

    public void OnGenerateField()
    {
        if (int.TryParse(inputField.text, out int userValue))
        {
            GlobalEventUI.InvokeOnClickGenerateFiledButton(userValue);
            chooseItem = null;
        }
    }

    public void OnCheckCell()
    {
        GlobalEventUI.InvokeOnCheckField();
    }

    private void OnDestroy()
    {
        GlobalEventUI.OnChooseCellInFiled -= OnChooseCellInField;
        GlobalEventUI.OnClickNumber -= OnClickNumber;
        GlobalEventUI.OnClickClearButton -= OnClearCell;
        GlobalEventUI.OnActiveWinPanel -= ActiveWinPanel;
    }
}
