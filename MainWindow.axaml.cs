using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Threading;
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
        
        Opened += (_,_) => _engine.Start();

        _engine.OnGameOver += () =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                var dlg = new Window
                {
                    Width = 300,
                    Height = 150,
                    Background = Brushes.Black,
                    Content = new TextBlock
                    {
                        Text = "Game Over",
                        Foreground = Brushes.Red,
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                        FontSize = 32
                    }
                };
                dlg.ShowDialog(this);
            });
        };
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
                _engine.SoftDrop(); 
                break;
            case Key.Space:
                _engine.HardDrop();
                break;
            case Key.Tab:
                _engine.SaveTetromino();
                break;
            case Key.Escape:
                Close();
                return;
        }
        GameCanvas.InvalidateVisual();
    }
}