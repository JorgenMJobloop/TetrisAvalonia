using Avalonia.Controls;
using Avalonia.Input;
using TetrisAvalonia.Game;

namespace TetrisAvalonia;

public partial class MainWindow : Window
{
    private readonly GameEngine _engine;

    public MainWindow()
    {
        InitializeComponent();
        _engine = new GameEngine();
        GameCanvas.Engine = _engine;
        _engine.StateChanged += () => GameCanvas.InvalidateVisual();
        
        KeyDown += OnKeyDown;
        
        _engine.Start();
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Left:
                _engine.MoveLeft();
                break;
            case Key.Right:
                _engine.MoveRight(); 
                break;
            case Key.Up:
                _engine.Rotate();
                break;
            case Key.Down:
                _engine.FastForward(); 
                break;
            case Key.Space:
                _engine.InstantDrop();
                break;
            case Key.Tab:
                _engine.SaveTetromino();
                break;
            case Key.Escape:
                return;
        }
        GameCanvas.InvalidateVisual();
    }
    
}