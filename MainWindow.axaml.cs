using Avalonia.Controls;
using Avalonia.Input;
using TetrisAvalonia.Controller;
using TetrisAvalonia.Game;

namespace TetrisAvalonia;

public partial class MainWindow : Window
{
    private readonly GameEngine _engine;
    private readonly GameCanvas _canvas = new GameCanvas();
    public MainWindow()
    {
        InitializeComponent();
        _engine = new GameEngine();
        _canvas.Engine = _engine;

        _engine.StateChanged += () => _canvas.InvalidateVisual();
        this.KeyDown += OnKeyDown;
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Left:
                _engine.MoveLeft(); // TODO: Implement this method
                break;
            case Key.Right:
                _engine.MoveRight(); // TODO: Implement this method
                break;
            case Key.Up:
                _engine.Rotate(); // TODO: Implement this method
                break;
            case Key.Down:
                _engine.FastForward(); // TODO: Implement this method
                break;
            case Key.Space:
                _engine.InstantDrop(); // TODO: Implement this method
                break;
            case Key.Tab:
                _engine.SaveTetromino(); // TODO: Implement this method
                break;
        }
    }
}