using System;
using System.Collections.Generic;

namespace TetrisAvalonia.Game;

public class Tetromino
{
    public int X {get; set;}
    public int Y {get; set;}
    
    public int[,] Shape { get; private set; }

    private Tetromino(int[,] shape)
    {
        Shape = shape;
    }

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

    public Tetromino Clone()
    {
        var rows = Shape.GetLength(0);
        var cols = Shape.GetLength(1);

        var cloneShape = new int[rows, cols];
        Array.Copy(Shape, cloneShape, Shape.Length);

        return new Tetromino(cloneShape)
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
        var shapes  = new List<int[,]>
        {
            new int[,]{{1,1,1,1}}, // I
            
            new int[,]{{1,1},{1,1}}, // O
            
            new int[,]{{0,1,0},{1,1,1}}, // T
            
            new int[,]{{1,0,0},{1,1,1}}, // J
            
            new int[,]{{0,0,1},{1,1,1}}, // L
            
            new int[,]{{0,1,1},{1,1,0}}, // S
            
            new int[,]{{1,1,0},{0,1,1}}, // Z
        };
        var index = Random.Shared.Next(shapes.Count);
        return new Tetromino(shapes[index]);
    }
}