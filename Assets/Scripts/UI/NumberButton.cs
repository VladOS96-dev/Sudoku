using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
public class NumberButton : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI textNumber;
    public int number;
    void Start()
    {
        
    }
    public void SetNumber(int number)
    { 
         this.number = number;
        textNumber.text = number.ToString();    
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GlobalEventUI.InvokeOnClickNumber(number);
    }
}
