using System;
using System.Collections.Generic;

namespace TetrisAvalonia.Game;

public class Tetromino
{
    public int X {get; set;}
    public int Y {get; set;}
    
    public int ColorId { get; }
    
    public int[,] Shape { get; private set; }

    private Tetromino(int colorId,int[,] shape)
    {
        ColorId = colorId;
        Shape = shape;
    }

    public bool TryRotate(Grid grid)
    {
        var rows = Shape.GetLength(0);
        var cols = Shape.GetLength(1);

        var rotated = new int[cols, rows];
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                rotated[x, rows - 1 - y] = Shape[y, x];
            }
        }
        
        // do a collision check inside the grid
        for (int y = 0; y < rotated.GetLength(0); y++)
        {
            for (int x = 0; x < rotated.GetLength(1); x++)
            {
                if (rotated[y, x] == 0)
                {
                    continue;
                }

                var gridX = X + x;
                var gridY = Y + y;
                if (!grid.IsInside(gridX, gridY) || !grid.IsEmpty(gridX, gridY))
                {
                    return false;
                }
            }
        }

        Shape = rotated;
        return true;
    }
/*
    public void Rotate()
    {
        int rows = Shape.GetLength(0);
        int cols = Shape.GetLength(1);
        var rotated = new int[rows, cols];

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                rotated[x, rows - 1 - y] = Shape[y, x];
            }
        }

        Shape = rotated;
    }
*/
    public Tetromino Clone()
    {
        var rows = Shape.GetLength(0);
        var cols = Shape.GetLength(1);

        var cloneShape = new int[rows, cols];
        Array.Copy(Shape, cloneShape, Shape.Length);

        return new Tetromino(ColorId, cloneShape)
        {
            X = this.X,
            Y = this.Y,
        };
    }

    public IEnumerable<(int x, int y)> OccupiedCells()
    {
        for (var y = 0; y < Shape.GetLength(0); y++)
        {
            for (var x = 0; x < Shape.GetLength(1); x++)
            {
                if (Shape[y, x] != 0)
                {
                    yield return (x, y);
                }
            }
        }
    }

    public static Tetromino CreateRandom()
    {
        var shapes  = new(int id, int[,] shape)[]
        {
            (1,new int[,]{{1,1,1,1}}), // I
            
            (2,new int[,]{{1,1},{1,1}}), // O
            
            (3,new int[,]{{0,1,0},{1,1,1}}), // T
            
            (4,new int[,]{{1,0,0},{1,1,1}}), // J
            
            (5,new int[,]{{0,0,1},{1,1,1}}), // L
            
            (6,new int[,]{{0,1,1},{1,1,0}}), // S
            
            (7,new int[,]{{1,1,0},{0,1,1}}), // Z
        };
        var choice = shapes[Random.Shared.Next(shapes.Length)];
        return new Tetromino(choice.id, choice.shape);
    }
}