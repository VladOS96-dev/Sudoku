using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameField : MonoBehaviour
{
    private SudokuGenerator sudokuGenerator = new SudokuGenerator();
    [SerializeField]private GameManager gameManager;
    private SaveManager saveManager=new SaveManager();
    private Item[,] matrix;

    public bool IsNoteMode { get; private set; }

    private void Awake()
    {
        GlobalEventUI.OnClickGenerateFiledButton += GenerateField;
        GlobalEventUI.OnCheckField += CheckSolution;
        GlobalEventUI.OnCheckCell += CheckCell;
        GlobalEventUI.OnChooseCellInFiled += ActivateHighligth;
        GlobalEventUI.OnToggleNote += ToggleNote;
      
    }
    private void Start()
    {
        FillMatrix();
    }
    public void SetMatrix(Item[,] matrix)
    {
        this.matrix = matrix;
    }
    private void ToggleNote(Item cell)
    {
        IsNoteMode = !IsNoteMode;
        cell.ToggleNoteMode();
    }
    private void FillMatrix()
    {
        MatrixItem[] matrixItems = GetComponentsInChildren<MatrixItem>();
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

    }
    private void GenerateField(int difficulty)
    {

        int[,] puzzle = sudokuGenerator.GeneratePuzzle(difficulty);
        gameManager.SetSolution(puzzle);

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                matrix[i, j].SetTypeItem(puzzle[i, j] == 0 ? TypeItem.Empty : TypeItem.Fill);
                matrix[i, j].ClearCell();
                if (puzzle[i, j] != 0)
                {
                    matrix[i, j].SetNumber(puzzle[i, j]);
                }
                matrix[i, j].SetRowColumn(i,j);
                matrix[i, j].SetHighligth(TypeHighligth.EmptyHighligth);
               
            }
        }
    }

    private void CheckSolution()
    {
        GlobalEventUI.InvokeOnActiveWinPanel( IsSudokuValid());

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
            int number = matrix[row, col].Number;
            if (number == 0 || !seen.Add(number))
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
            int number = matrix[row, col].Number;
            if (number == 0 || !seen.Add(number))
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
                int number = matrix[startRow + row, startCol + col].Number;
                if (number == 0 || !seen.Add(number))
                {
                    return false;
                }
            }
        }
        return true;
    }
    public void LoadField() 
    {
        int[,] grid = saveManager.LoadGame();
        if (grid != null)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    matrix[i,j].ClearCell();
                    matrix[i, j].SetTypeItem(grid[i, j] == 0 ? TypeItem.Empty : TypeItem.Fill);
                    matrix[i, j].SetNumber(grid[i, j]);
                    matrix[i, j].SetRowColumn(i,j);
                    
                }
            }
        }
    }
    public void SaveField() 
    {
        int[,] grid = new int[9, 9];
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                grid[i, j] = matrix[i, j].Number;
            }
        }
        saveManager.SaveGame(grid);
    }
    private void CheckCell(Item cell)
    {
        bool flag = false;

        for (int col = 0; col < 9; col++)
        {
            matrix[cell.row, col].SetHighligth(TypeHighligth.Chooseline);
            if (matrix[cell.row, col].Number != 0 && cell.column != matrix[cell.row, col].column)
            {
                if (matrix[cell.row, col].Number == cell.Number)
                {   
                    flag = true;
                    matrix[cell.row, col].SetHighligth(TypeHighligth.MatchCell);
                }
            }
        }

        for (int row = 0; row < 9; row++)
        {
            matrix[row, cell.column].SetHighligth(TypeHighligth.Chooseline);
            if (matrix[row, cell.column].Number != 0 && cell.row != matrix[row, cell.column].row)
            {
                if (matrix[row, cell.column].Number == cell.Number)
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
                if (matrix[startRow + row, startCol + col].Number != 0 && cell.row != matrix[startRow + row, startCol + col].row && cell.column != matrix[startRow + row, startCol + col].column)
                {
                    if (matrix[startRow + row, startCol + col].Number == cell.Number)
                    {
                        matrix[startRow + row, startCol + col].SetHighligth(TypeHighligth.MatchCell);
                        flag = true;
                    }
                }
            }
        }

        cell.SetValidCell(flag);
    }

    private void ActivateHighligth(Item cell)
    {
        if (cell.typeItem == TypeItem.Fill)
            return;
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                matrix[row, col].SetHighligth(TypeHighligth.EmptyHighligth);
            }
        }

        for (int i = 0; i < 9; i++)
        {
            matrix[cell.row, i].SetHighligth(TypeHighligth.Chooseline);
            matrix[i, cell.column].SetHighligth(TypeHighligth.Chooseline);
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
    private void OnDestroy()
    {
        GlobalEventUI.OnClickGenerateFiledButton -= GenerateField;
        GlobalEventUI.OnCheckField -= CheckSolution;
        GlobalEventUI.OnCheckCell -= CheckCell;
        GlobalEventUI.OnChooseCellInFiled -= ActivateHighligth;
        GlobalEventUI.OnToggleNote -= ToggleNote;
    }
}
