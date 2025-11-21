using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using TetrisAvalonia.Game;

namespace TetrisAvalonia.Controls;

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

        for (var y = 0; y < grid.Rows; y++)
        {
            for (var x = 0; x < grid.Cols; x++)
            {
                if (grid[y, x] != 0)
                {
                    context.DrawRectangle(Brushes.Violet, null, new Rect(x * size, y * size, size, size));
                }
            }
        }
        // draw tetromino
        foreach (var (px, py) in Engine.Active.OccupiedCells())
        {
            context.DrawRectangle(Brushes.LightGoldenrodYellow, null, new Rect(px * size, py * size, size, size));
        }
        // save the drawing state
        if (Engine.Saved != null)
        {
            var offsetX = 260; // offset x
            var offsetY = 40; // offset y

            foreach (var (px, py) in Engine.Saved.OccupiedCells())
            {
                context.DrawRectangle(Brushes.LightGoldenrodYellow, null, new Rect(offsetX + px * size,offsetY + py * size, size, size));
            }
        }
        
    }
}