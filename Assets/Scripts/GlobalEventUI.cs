using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class GlobalEventUI
{
    public static Action<int> OnClickNumber;
    public static Action OnClickClearButton;
    public static Action<int> OnClickGenerateFiledButton;
    public static Action<Item> OnChooseCellInFiled;
    public static Action OnCheckField;
    public static Action<Item> OnCheckCell;

    public static void InvokeOnClickNumber(int number)
    { 
    OnClickNumber?.Invoke(number);
    }
    public static void InvokeOnClickClearButton()
    {
        OnClickClearButton?.Invoke();
    }
    public static void InvokeOnClickGenerateFiledButton(int countField)
    {
        OnClickGenerateFiledButton?.Invoke(countField);
    }
    public static void InvokeOnChooseCellInFiled(Item cell)
    {
        OnChooseCellInFiled?.Invoke(cell);
    }
    public static void InvokeOnCheckField()
    { 
        OnCheckField?.Invoke();
    }
    public static void InvokeOnCheckCell(Item   cell)
    {
        OnCheckCell?.Invoke(cell);
    }
}
