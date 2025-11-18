using System;
using Avalonia.Threading;

namespace TetrisAvalonia.Game;

public class GameEngine
{
    private readonly DispatcherTimer _timer;
    public Grid Grid { get; }
    public Tetrimino Active { get; private set; }
    
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

    private void Tick()
    {
        if (!Grid.CanMoveDown(Active))
        {
            Grid.Merge(Active);
            Spawn();
        }
        else
        {
            Active.Y++;
        }
        StateChanged?.Invoke();
    }

    private void Spawn()
    {
        Active = Tetrimino.CreateRandom();
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
    public void MoveRight(){}
    
    public void Rotate(){}

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
    
    public void SaveTetromino(){}
}