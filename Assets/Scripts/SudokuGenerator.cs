
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class SudokuGenerator
{

    private int[,] grid = new int[9, 9];
    public List<int> tmpNumber = new List<int>();
    private System.Random random = new System.Random();
    private List<int> templteNumber = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    public int[,] GeneratePuzzle(int difficulty)
    {
        Generate();
        RemoveNumbers(difficulty);
        return grid;
    }

    private void RemoveNumbers(int difficulty)
    {
        int cellsToRemove = difficulty; // Чем больше значение, тем сложнее головоломка
        while (cellsToRemove > 0)
        {
            int row = random.Next(0, 9);
            int col = random.Next(0, 9);

            if (grid[row, col] != 0)
            {
                grid[row, col] = 0;
                cellsToRemove--;
            }
        }
    }

    public int[,] Generate()
    {
        if (FillGrid())
        {
            return grid;
        }
        else
        {
            return Generate();
        }
    }

    private bool FillGrid()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (grid[row, col] == 0)
                {
                    List<int> numbers = new List<int>(templteNumber);
                    Shuffle(numbers);

                    foreach (int number in numbers)
                    {
                        if (IsValid(number, row, col))
                        {
                            grid[row, col] = number;
                            if (FillGrid())
                            {
                                return true;
                            }
                            grid[row, col] = 0; 
                        }
                    }
                    return false; 
                }
            }
        }
        return true; 
    }

    private void Shuffle(List<int> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            int value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    private bool IsValid(int number, int row, int col)
    {
        
        for (int i = 0; i < 9; i++)
        {
            if (grid[row, i] == number || grid[i, col] == number)
            {
                return false;
            }
        }

        
        int startRow = (row / 3) * 3;
        int startCol = (col / 3) * 3;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (grid[startRow + i, startCol + j] == number)
                {
                    return false;
                }
            }
        }

        return true;
    }

}
