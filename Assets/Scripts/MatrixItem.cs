using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class MatrixItem : MonoBehaviour
{
    public Item [,] matrix;
    public Item[] items;
    void Awake()
    {
        matrix=new Item[3,3];
        Item[] items = GetComponentsInChildren<Item>();
        this.items= items;
        int index = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                matrix[i,j] = items[index];
                index++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}