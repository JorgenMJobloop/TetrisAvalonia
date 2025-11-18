using System;
using System.Collections.Generic;

namespace TetrisAvalonia.Game;

public class Tetrimino
{
    public int X {get; set;}
    public int Y {get; set;}
    
    public int[,] Shape { get; private set; }

    private Tetrimino(int[,] shape)
    {
        Shape = shape;
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

    public static Tetrimino CreateRandom()
    {
        var list = new[]
        {
            new[,]{{1,1,1,1}},
            new[,]{{1,1},{1,1}},
            new[,]{{0,1,0},{1,1,1}},
        };
        return new Tetrimino(list[new Random().Next(list.Length)]);
    }
}