using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using TetrisAvalonia.Game;

namespace TetrisAvalonia.Controller;

public class GameCanvas : Control
{
    public GameEngine? Engine { get; set; }

    public override void Render(DrawingContext context)
    {
        base.Render(context);
        if (Engine is null)
        {
            return;
        }

        const int size = 24;
        var grid = Engine.Grid;

        for (int y = 0; y < grid.Rows; y++)
        {
            for (int x = 0; x < grid.Cols; x++)
            {
                if(grid[y,x] != 0)
                {
                    context.DrawRectangle(Brushes.Gray, null, new Rect(x  * size, y * size, size, size));
                }
            }
        }

        foreach (var (px, py) in Engine.Active.OccupiedCells())
        {
            int gx = Engine.Active.X + px;
            int gy = Engine.Active.Y + py;
            
            context.DrawRectangle(Brushes.Cyan, null, new Rect(gx * size, gy * size, size, size));
        }
    }
}