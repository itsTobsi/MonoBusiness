using System;
using Microsoft.Xna.Framework;
using static MonoBusiness.Rendering.TileMetrics;

namespace MonoBusiness.Rendering;

public static class IsoProjection
{
    // Top vertex of a tile's top face, raised by height.
    public static Vector2 TileToWorld(int column, int row, int height) =>
        new((column - row) * TileW / 2f, (column + row) * TileH / 2f - height * BlockDepth);

    // Which tile a world point falls on, assuming that tile is at the given height.
    public static Point WorldToTile(Vector2 world, int height)
    {
        float y = world.Y + height * BlockDepth;
        float fc = (world.X / (TileW / 2f) + y / (TileH / 2f)) / 2f;
        float fr = (y / (TileH / 2f) - world.X / (TileW / 2f)) / 2f;
        return new Point((int)MathF.Floor(fc), (int)MathF.Floor(fr));
    }
}
