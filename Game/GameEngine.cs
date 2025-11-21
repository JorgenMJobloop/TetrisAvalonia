using System;
using Avalonia.Threading;

namespace TetrisAvalonia.Game;

public class GameEngine
{
    private readonly DispatcherTimer _timer;
    public Grid Grid { get; }
    public Tetromino Active { get; private set; }
    public Tetromino? Saved { get; private set; }
    private bool HasSavedTetromino = false;
    public int Level { get; private set; } = 1;
    public int LinesCleared { get; private set; } = 0;
    public int Score { get; private set; } = 0;
    
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
        if (!Grid.CanMoveDown(Active))
        {
            Grid.Merge(Active);
            var cleared = Grid.ClearFullLines();
            if (cleared > 0)
            {
                LinesCleared += cleared;
                AddScore(cleared);
                UpdateLevelAndSpeed();
            }

            HasSavedTetromino = false;
            Spawn();
        }
        StateChanged?.Invoke();
    }

    private void Spawn()
    {
        Active = Tetromino.CreateRandom();
        Active.X = 3;
        Active.Y = 0;
    }
    
    public void MoveLeft()
    {
        if (Grid.CanMoveHorizontally(Active, -1))
        {
            Active.X--;
        }
    }

    public void FastForward()
    {
        if (Grid.CanMoveDown(Active))
        {
            Active.Y++;
            _timer.Tick += (_, _) => Tick();
        }
    }

    public void MoveRight()
    {
        if (Grid.CanMoveHorizontally(Active, +1))
        {
            Active.X++;
        }
    }

    public void Rotate()
    {
        Active.Rotate();
        if (!Grid.Collision(Active))
        {
            UndoRotate();
        }
    }

    private void UndoRotate()
    {
        Active.Rotate();   
        Active.Rotate();
        Active.Rotate();
    }

    private bool ShouldDropInstantly()
    {
        if (Grid.CanMoveDown(Active))
        {
            Active.Y++;
            return true;
        }
        return false;
    }
    
    public void InstantDrop()
    {
        ShouldDropInstantly();
    }

    public void SaveTetromino()
    {
        if (HasSavedTetromino)
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
        
        HasSavedTetromino = true;
        StateChanged?.Invoke();
    }
}