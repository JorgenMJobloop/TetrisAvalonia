using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using TetrisAvalonia.Game;

namespace TetrisAvalonia.Controls;

public class GameCanvas : Control
{
    public GameEngine? Engine { get; set; }

    private const int CellSize = 24;
    private const int BoardOffsetX = 10;
    private const int BoardOffsetY = 10;

    protected override Size MeasureOverride(Size availableSize)
    {
        return new Size(10 * CellSize + 200, 20 * CellSize + 40);
    }

    public override void Render(DrawingContext context)
    {
        
        base.Render(context);
        if (Engine is null)
        {
            return;
        }
        
        var grid = Engine.Grid;
        
        var boardRect = new Rect(
            BoardOffsetX, BoardOffsetY, grid.Cols * CellSize, grid.Rows * CellSize);
        
        context.DrawRectangle(new SolidColorBrush(Color.FromRgb(5,5,25)), null, boardRect);


        var pen = new Pen(Brushes.Gray, 1);
        for (int y = 0; y <= grid.Rows; y++)
        {
            context.DrawLine(pen, new Point(BoardOffsetX, BoardOffsetY + y * CellSize),
                new Point(BoardOffsetX + grid.Cols * CellSize, BoardOffsetY + y * CellSize));
        }

        for (int x = 0; x <= grid.Cols; x++)
        {
            context.DrawLine(pen, 
                new Point(BoardOffsetX + x * CellSize, BoardOffsetY), 
                new Point(BoardOffsetX + x * CellSize, BoardOffsetY + grid.Rows * CellSize));
        }
        
        // lock blocks
        for (int y = 0; y < grid.Rows; y++)
        {
            for (int x = 0; x < grid.Cols; x++)
            {
                int v = grid[y, x];
                if (v != 0)
                {
                    var rect = new Rect(
                        BoardOffsetX + x * CellSize,
                        BoardOffsetY + y * CellSize,
                        CellSize,
                        CellSize
                        );
                    // fill
                    context.DrawRectangle(GetBrush(v), null, rect);
                    // border
                    context.DrawRectangle(null, new Pen(Brushes.Black, 1), rect);
                }
            }
        }

        foreach (var (px, py) in Engine.Active.OccupiedCells())
        {
            var gx = Engine.Active.X + px;
            var gy = Engine.Active.Y + py;
        
            var rect = new Rect(BoardOffsetX + gx * CellSize, BoardOffsetY + gy * CellSize, CellSize, CellSize);

            context.DrawRectangle(GetBrush(Engine.Active.ColorId), null, rect);
            
            // Grid border
            context.DrawRectangle(null, new Pen(Brushes.Black, 1), rect);
        }

        if (Engine.Saved is not null)
        {
            foreach (var (px, py) in Engine.Saved.OccupiedCells())
            {

                var rect = new Rect(
                    BoardOffsetX + grid.Cols * CellSize + 30 + px * CellSize,
                    BoardOffsetY + 30 + py * CellSize,
                    CellSize,
                    CellSize
                    );
                
                context.DrawRectangle(GetBrush(Engine.Saved.ColorId), null, rect);
                // border
                context.DrawRectangle(null, new Pen(Brushes.Black, 1), rect);
            }
        }
    }

    private IBrush GetBrush(int id) => id switch
    {
        1 => Brushes.Cyan,
        2 => Brushes.Yellow,
        3 => Brushes.MediumPurple,
        4 => Brushes.Blue,
        5 => Brushes.Orange,
        6 => Brushes.LimeGreen,
        7 => Brushes.Red,
        _ => Brushes.White
    };
}