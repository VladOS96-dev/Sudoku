using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameField : MonoBehaviour
{
    public MatrixItem[] matrixItems;
    public Item[,] matrix;
    private SudokuGenerator sudokuGenerator = new SudokuGenerator();
    private GameManager gameManager;
    public bool isNoteMode = false;

    private void Start()
    {
        GlobalEventUI.OnClickGenerateFiledButton += GenerateField;
        GlobalEventUI.OnCheckField += CheckSolution;
        GlobalEventUI.OnCheckCell += CheckCell;
        GlobalEventUI.OnChooseCellInFiled += ActivateHighligth;
        gameManager = FindObjectOfType<GameManager>();

        matrixItems = GetComponentsInChildren<MatrixItem>();
        matrix = new Item[9, 9];

        int indexMatrix = 0;
        int row = 0;
        int col = 0;
        int count = 0;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                matrix[i, j] = matrixItems[indexMatrix].matrix[row, col];
                count++;
                col++;
                if (col % 3 == 0)
                {
                    indexMatrix++;
                    col = 0;
                }
                if (count % 9 == 0)
                {
                    indexMatrix = (count / 27) * 3;
                    row++;
                    if (row % 3 == 0)
                    {
                        row = 0;
                    }
                }
                if (indexMatrix == matrixItems.Length)
                {
                    indexMatrix -= 1;
                }
            }
        }



        GenerateField(30);
    }


    public void GenerateField(int countCell)
    {
        int[,] numbers = sudokuGenerator.GeneratePuzzle(countCell);
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                matrix[i, j].ClearCell();
                matrix[i, j].SetNumber(numbers[i, j]);
                matrix[i, j].SetTypeItem(matrix[i, j].number == 0 ? TypeItem.Empty : TypeItem.Fill);
                matrix[i, j].SetPos(i, j);
            }
        }
    }

    public void SaveGame()
    {
        int[,] grid = new int[9, 9];
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                grid[i, j] = matrix[i, j].number;
            }
        }
        gameManager.SaveGame(grid);
    }

    public void LoadGame()
    {
        int[,] grid = gameManager.LoadGame();
        if (grid != null)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    matrix[i, j].SetNumber(grid[i, j]);
                    matrix[i, j].SetTypeItem(grid[i, j] == 0 ? TypeItem.Empty : TypeItem.Fill);
                }
            }
        }
    }

    public void CheckSolution()
    {
        if (IsSudokuValid())
        {
            Debug.Log("The solution is correct!");
        }
        else
        {
            Debug.Log("The solution is incorrect!");
        }
    }

    public void ActivateHighligth(Item cell)
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                matrix[row, col].SetHighligth(TypeHighligth.EmptyHighligth);
            }
        }
        for (int col = 0; col < 9; col++)
        {
            matrix[cell.row, col].SetHighligth(TypeHighligth.Chooseline);
        }

        for (int row = 0; row < 9; row++)
        {
            matrix[row, cell.column].SetHighligth(TypeHighligth.Chooseline);
        }

        int startRow = (cell.row / 3) * 3;
        int startCol = (cell.column / 3) * 3;

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                matrix[startRow + row, startCol + col].SetHighligth(TypeHighligth.Chooseline);
            }
        }
    }

    public void CheckCell(Item cell)
    {
        bool flag = false;

        for (int col = 0; col < 9; col++)
        {
            matrix[cell.row, col].SetHighligth(TypeHighligth.Chooseline);
            if (matrix[cell.row, col].number != 0 && cell.column != matrix[cell.row, col].column)
            {
                if (matrix[cell.row, col].number == cell.number)
                {
                    flag = true;
                    matrix[cell.row, col].SetHighligth(TypeHighligth.MatchCell);
                }
            }
        }

        for (int row = 0; row < 9; row++)
        {
            matrix[row, cell.column].SetHighligth(TypeHighligth.Chooseline);
            if (matrix[row, cell.column].number != 0 && cell.row != matrix[row, cell.column].row)
            {
                if (matrix[row, cell.column].number == cell.number)
                {
                    matrix[row, cell.column].SetHighligth(TypeHighligth.MatchCell);
                    flag = true;
                }
            }
        }

        int startRow = (cell.row / 3) * 3;
        int startCol = (cell.column / 3) * 3;

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                matrix[startRow + row, startCol + col].SetHighligth(TypeHighligth.Chooseline);
                if (matrix[startRow + row, startCol + col].number != 0 && cell.row != matrix[startRow + row, startCol + col].row && cell.column != matrix[startRow + row, startCol + col].column)
                {
                    if (matrix[startRow + row, startCol + col].number == cell.number)
                    {
                        matrix[startRow + row, startCol + col].SetHighligth(TypeHighligth.MatchCell);
                        flag = true;
                    }
                }
            }
        }

        cell.SetValidCell(flag);
    }

    private bool IsSudokuValid()
    {
        for (int i = 0; i < 9; i++)
        {
            if (!IsValidRow(i) || !IsValidColumn(i) || !IsValidBlock(i))
            {
                return false;
            }
        }
        return true;
    }

    private bool IsValidRow(int row)
    {
        HashSet<int> seen = new HashSet<int>();
        for (int col = 0; col < 9; col++)
        {
            int number = matrix[row, col].number;
            if (number != 0 && !seen.Add(number))
            {
                return false;
            }
        }
        return true;
    }

    private bool IsValidColumn(int col)
    {
        HashSet<int> seen = new HashSet<int>();
        for (int row = 0; row < 9; row++)
        {
            int number = matrix[row, col].number;
            if (number != 0 && !seen.Add(number))
            {
                return false;
            }
        }
        return true;
    }

    private bool IsValidBlock(int block)
    {
        int startRow = (block / 3) * 3;
        int startCol = (block % 3) * 3;
        HashSet<int> seen = new HashSet<int>();
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                int number = matrix[startRow + row, startCol + col].number;
                if (number != 0 && !seen.Add(number))
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void ToggleNoteMode()
    {
        isNoteMode = !isNoteMode;
        foreach (var item in matrix)
        {
            item.ToggleNoteMode();
        }
    }

    public void AddNoteToCell(int row, int col, int number)
    {
        matrix[row, col].AddNote(number);
    }

    public void RemoveNoteFromCell(int row, int col, int number)
    {
        matrix[row, col].RemoveNote(number);
    }
    private void OnDestroy()
    {
        GlobalEventUI.OnClickGenerateFiledButton -= GenerateField;
        GlobalEventUI.OnCheckField -= CheckSolution;
        GlobalEventUI.OnCheckCell -= CheckCell;
        GlobalEventUI.OnChooseCellInFiled -= ActivateHighligth;

    }
}
