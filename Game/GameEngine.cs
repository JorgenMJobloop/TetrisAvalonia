using System;
using Avalonia.Threading;

namespace TetrisAvalonia.Game;

public class GameEngine
{
    private readonly DispatcherTimer _timer;
    public Grid Grid { get; }
    public Tetromino Active { get; private set; }
    public Tetromino? Saved { get; private set; }
    private bool _hasSavedTetromino = false;
    public int Level { get; private set; } = 1;
    public int LinesCleared { get; private set; } = 0;
    public int Score { get; private set; } = 0;
    public event Action? OnGameOver;
    
    public event Action? StateChanged;

    public GameEngine()
    {
        Grid = new Grid(20, 10);
        Spawn();
        _timer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromMilliseconds(500)
        };
        _timer.Tick += (_, _) => Tick();
        _timer.Start();
    }
    
    public void Start() =>  _timer.Start();
    public void Stop() => _timer.Stop();

    private void UpdateLevelAndSpeed()
    {
        Level = (LinesCleared / 10) + 1;
        _timer.Interval = TimeSpan.FromMilliseconds(Math.Max(60, 500 - (Level - 1) * 40));
    }

    private void AddScore(int cleared)
    {
        var points = cleared switch
        {
            1 => 100,
            2 => 300,
            3 => 500,
            4 => 800,
            _ => 0
        };
        Score += points * Level;
    }

    private void Tick()
    {
        if (Grid.CanMoveDown(Active))
        {
            Active.Y++;
        }
        else
        {
            Grid.Merge(Active);
            var cleared = Grid.ClearFullLines();
            if (cleared > 0)
            {
                LinesCleared += cleared;
                AddScore(cleared);
                UpdateLevelAndSpeed();
            }

            _hasSavedTetromino = false;
            Spawn();
        }
        StateChanged?.Invoke();
    }

    private void Spawn()
    {
        Active = Tetromino.CreateRandom();
        Active.X = 3;
        Active.Y = 0;
        
        // Add game over state
        foreach (var (px, py) in Active.OccupiedCells())
        {
            var gx = Active.X + px;
            var gy = Active.Y + py;

            if (!Grid.IsEmpty(gx, gy))
            {
                Stop();
                OnGameOver?.Invoke();
                return;
            }
        }
    }
    
    public void MoveLeft()
    {
        if (Grid.CanMoveHorizontally(Active, -1))
        {
            Active.X--;
        }
        StateChanged?.Invoke();
    }

    public void MoveRight()
    {
        if (Grid.CanMoveHorizontally(Active, +1))
        {
            Active.X++;
        }
        StateChanged?.Invoke();
    }

    public void Rotate()
    {
        if (Active.TryRotate(Grid))
        {
            StateChanged?.Invoke();
        }
    }

    public void SoftDrop()
    {
        if (Grid.CanMoveDown(Active))
        {
            Active.Y++;
            StateChanged?.Invoke();
        }
    }

    public void HardDrop()
    {
        while (Grid.CanMoveDown(Active))
        {
            Active.Y++;
        }
        
        Grid.Merge(Active);
        var cleared = Grid.ClearFullLines();
        if (cleared > 0)
        {
            LinesCleared += cleared;
            AddScore(cleared);
            UpdateLevelAndSpeed();
        }
        
        _hasSavedTetromino = false;
        Spawn();
        StateChanged?.Invoke();
    }

    public void SaveTetromino()
    {
        if (_hasSavedTetromino)
        {
            return;
        }

        if (Saved == null)
        {
            Saved = Active.Clone(); // TODO: Implement Clone method
            Spawn(); // spawn a new tetromino
        }
        else
        {
            var temp = Active;
            Active = Saved.Clone();
            Active.X = 3;
            Active.Y = 0;
            Saved = temp;
        }
        
        _hasSavedTetromino = true;
        StateChanged?.Invoke();
    }
}