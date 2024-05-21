[System.Serializable]
public class SudokuData
{
    public int[] grid;
    public int gridSize = 9;

    public SudokuData(int[,] grid)
    {
        this.grid = new int[gridSize * gridSize];
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                this.grid[i * gridSize + j] = grid[i, j];
            }
        }
    }

    public int[,] GetGrid()
    {
        int[,] grid = new int[gridSize, gridSize];
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                grid[i, j] = this.grid[i * gridSize + j];
            }
        }
        return grid;
    }
}
