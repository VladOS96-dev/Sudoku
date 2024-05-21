using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public void SaveGame(int[,] grid)
    {
        SudokuData data = new SudokuData(grid);
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("Save", json);
        PlayerPrefs.Save();
        Debug.Log("Game saved");
    }

    public int[,] LoadGame()
    {
        if (PlayerPrefs.HasKey("Save"))
        {
            string json = PlayerPrefs.GetString("Save");
            SudokuData data = JsonUtility.FromJson<SudokuData>(json);
            Debug.Log("Game loaded");

            return data.GetGrid();
        }
        else
        {
            Debug.LogWarning("Save file not found");
            return null;
        }
    }
}
