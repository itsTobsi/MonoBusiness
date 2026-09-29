using System;
using Microsoft.Xna.Framework;

namespace MonoBusiness.Rendering;

public static class TerrainPalette
{
    public static Color TopColor(int h, float time) =>
        h switch
        {
            0 => Color.Lerp(
                new Color(40, 90, 200),
                new Color(70, 140, 230),
                0.5f + 0.5f * MathF.Sin(time * 2f)
            ), // water
            1 => new Color(230, 210, 140), // sand
            2 or 3 => new Color(100, 190, 80), // grass
            4 => new Color(70, 140, 60), // forest
            5 => new Color(140, 135, 130), // stone
            _ => new Color(245, 245, 255), // snow
        };

    public static Color EarthColor(int z) =>
        z < 2 ? new Color(150, 110, 70) : new Color(120, 115, 110);
}
