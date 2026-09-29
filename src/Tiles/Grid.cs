using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoBusiness;

public class Grid
{
    public Tile[,] Tiles;

    // Could make a getter function for these instead
    public int Width { get; }
    public int Height { get; }
    public int ZAxis { get; } // Find a better name for this. structureHeight?

    public Grid(int width, int height, int zAxis)
    {
        Width = width;
        Height = height;
        ZAxis = zAxis;
        Tiles = new Tile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tiles[x, y] = new Tile { Position = new Point(x, y) };
            }
        }
    }

    public bool InBounds(int column, int row) =>
        column >= 0 && row >= 0 && column < Width && row < Height;

    public void Raise(int column, int row) =>
        Tiles[column, row].height = Math.Min(ZAxis, Tiles[column, row].height + 1);

    public void Lower(int column, int row) =>
        Tiles[column, row].height = Math.Max(0, Tiles[column, row].height - 1);

    public void Draw(SpriteBatch spriteBatch)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Tiles[x, y].Draw(spriteBatch);
            }
        }
    }
}
