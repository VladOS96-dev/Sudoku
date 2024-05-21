using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int[,] solution;

    public void SetSolution(int[,] solution)
    {
        this.solution = solution;
    }

    public int GetNumber(int row, int column)
    {
        return solution[row, column];
    }
}
