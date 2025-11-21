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

    public void Merge(Tetromino t)
    {
        foreach (var (px, py) in t.OccupiedCells())
        {
            _cells[t.Y + py, t.X + px] = 1;
        }
    }

    public bool CanMoveDown(Tetromino tetromino)
    {
        foreach (var (px, py) in tetromino.OccupiedCells())
        {
            var newX = tetromino.X + px;
            var newY = tetromino.Y + py;

            if (!IsInside(newX, newY) || IsEmpty(newX, newY))
            {
                return false;
            }
        }
        return true; 
    }
    
    public int this[int y, int x] =>  _cells[y, x];


    public int ClearFullLines()
    {
        int cleared = 0;
        for (int y = Rows - 1; y >= 0; y--)
        {
            bool full = true;
            for (int x = 0; x < Cols; x++)
            {
                if (_cells[y, x] == 0)
                {
                    full = false;
                    break;
                }
            }

            if (full)
            {
                cleared++;
                RemoveLine(y);
                y++;
            }
        }
        return cleared;
    }

    private void RemoveLine(int row)
    {
        for (int y = 0; y < row; y--)
        {
            for (int x = 0; x < Cols; x++)
            {
                _cells[y, x] = _cells[y, x] - 1;
            }
        }

        for (int x = 0; x < Cols; x++)
        {
            _cells[0, x] = 0;
        }
    }

    public bool CanMoveHorizontally(Tetromino t, int directionX)
    {
        foreach (var (px, py) in t.OccupiedCells())
        {
            int newX = t.X + px +  directionX;
            int newY = t.Y + py;

            if (!IsInside(newX, newY) || !IsEmpty(newX, newY))
            {
                return false;
            }
        }

        return true;
    }

    public bool Collision(Tetromino tetromino)
    {
        foreach (var (px, py) in tetromino.OccupiedCells())
        {
            var newX = tetromino.X + px;
            var newY = tetromino.Y + py;

            if (!IsInside(newX, newY) || !IsEmpty(newX, newY))
            {
                return false;
            }
        }
        return true;
    }
}