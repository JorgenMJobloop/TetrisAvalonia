namespace TetrisAvalonia.Game;

public class Grid
{
    private readonly int[,] _cells;
    public int Rows { get; }
    public int Cols { get; }

    public Grid(int rows, int cols)
    {
        Rows = rows;
        Cols = cols;   
        _cells = new int[rows, cols];
    }

    public bool IsInside(int x, int y)
    {
        return x >= 0 && x < this.Cols && y >= 0 && y < this.Rows;
    }

    public bool IsEmpty(int x, int y)
    {
        return IsInside(x, y) && _cells[x, y] == 0;
    }

    public void Merge(Tetrimino t)
    {
        foreach (var (px, py) in t.OccupiedCells())
        {
            _cells[t.Y + py, t.X + px] = 1;
        }
    }

    public bool CanMoveDown(Tetrimino tetrimino)
    {
        foreach (var (px, py) in tetrimino.OccupiedCells())
        {
            var newX = tetrimino.X + px;
            var newY = tetrimino.Y + py;

            if (!IsInside(newX, newY) || IsEmpty(newX, newY))
            {
                return false;
            }
        }
        return true;
    }
    
    public int this[int y, int x] =>  _cells[y, x];
}