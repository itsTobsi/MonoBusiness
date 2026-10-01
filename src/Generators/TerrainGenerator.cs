using System;

namespace MonoBusiness.World;

public static class TerrainGenerator
{
    public static void Generate(Grid grid, int seed)
    {
        // World gen
        var rng = new Random(seed);
        float ox = rng.NextSingle() * 100f;
        float oy = rng.NextSingle() * 100f;

        for (int column = 0; column < grid.Width; column++)
        {
            for (int row = 0; row < grid.Height; row++)
            {
                float n =
                    MathF.Sin((column + ox) * 0.21f) * MathF.Cos((row + oy) * 0.17f)
                    + 0.5f * MathF.Sin((column + row) * 0.11f + ox)
                    + 0.25f * MathF.Cos((column - row) * 0.3f + oy);
                int h = (int)MathF.Round((n + 1.2f) / 2.9f * grid.ZAxis);
                grid.Tiles[column, row].Height = Math.Clamp(h, 0, grid.ZAxis);
            }
        }
    }
}
