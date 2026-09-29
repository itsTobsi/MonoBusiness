using System;
using Microsoft.Xna.Framework;
using MonoBusiness.World;

namespace MonoBusiness.Rendering;

public static class TilePicker
{
    // Test every height level; pick the front-most tile whose raised top face contains the point.
    public static Point? Pick(Grid grid, Vector2 world)
    {
        Point? best = null;
        int bestDepth = -1;

        for (int height = 0; height <= grid.ZAxis; height++)
        {
            var tile = IsoProjection.WorldToTile(world, height);
            if (!grid.InBounds(tile.X, tile.Y) || grid.Tiles[tile.X, tile.Y].height != height)
            {
                continue;
            }

            int depth = tile.X + tile.Y;
            if (depth > bestDepth)
            {
                bestDepth = depth;
                best = tile;
            }
        }

        /*for (int height = 0; height <= grid.Height; height++)
        {
            float y = world.Y + height * TileMetrics.BlockDepth;
            float fc = (world.X / (TileMetrics.TileW / 2f) + y / (TileMetrics.TileH / 2f)) / 2f;
            float fr = (y / (TileMetrics.TileH / 2f) - world.X / (TileMetrics.TileW / 2f)) / 2f;
            int column = (int)MathF.Floor(fc),
                row = (int)MathF.Floor(fr);
            if (
                column < 0
                || row < 0
                || column >= grid.Width
                || row >= grid.Height
                || grid.Tiles[column, row].height != height
            )
                continue;

            if (column + row > bestDepth)
            {
                bestDepth = column + row;
                best = new Point(column, row);
            }
        }*/
        return best;
    }
}
