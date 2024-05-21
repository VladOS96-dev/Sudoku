using System;
using UnityEngine;

public static class GlobalEventUI
{
    public static Action<int> OnClickGenerateFiledButton { get; set; }
    public static Action OnCheckField { get; set; }
    public static Action<Item> OnChooseCellInFiled { get; set; }
    public static Action<int> OnClickNumber { get; set; }
    public static Action OnClickClearButton { get; set; }
    public static Action<Item> OnCheckCell { get; set; }
    public static Action<bool> OnActiveWinPanel { get; set; }
    public static Action<Item> OnToggleNote { get; set; }

    public static void InvokeOnClickGenerateFiledButton(int difficulty) => OnClickGenerateFiledButton?.Invoke(difficulty);
    public static void InvokeOnCheckField() => OnCheckField?.Invoke();
    public static void InvokeOnChooseCellInFiled(Item item) => OnChooseCellInFiled?.Invoke(item);
    public static void InvokeOnClickNumber(int number) => OnClickNumber?.Invoke(number);
    public static void InvokeOnClickClearButton() => OnClickClearButton?.Invoke();
    public static void InvokeOnCheckCell(Item item) => OnCheckCell?.Invoke(item);
    public static void InvokeOnActiveWinPanel(bool isWin)=>OnActiveWinPanel?.Invoke(isWin);
    public static void InvokeOnToggleNote(Item cell)=>OnToggleNote?.Invoke(cell);
}
