using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static MonoBusiness.Rendering.TileMetrics;

namespace MonoBusiness.Rendering;

public static class TileTextures
{
    // Makes the textures for the tiles
    public static Texture2D CreateBlockTexture(GraphicsDevice device)
    {
        int width = TileMetrics.TileW,
            height = TileMetrics.TileH + TileMetrics.BlockDepth;

        var data = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float px = x + 0.5f,
                    py = y + 0.5f;
                float d =
                    MathF.Abs(px - width / 2f) / (width / 2f)
                    + MathF.Abs(py - TileMetrics.TileH / 2f) / (TileMetrics.TileH / 2f);
                Color col = Color.Transparent;

                if (d <= 1f)
                    col = d > 0.92f ? new Color(205, 205, 205) : Color.White; // top face + soft edge
                else
                {
                    // bottom edge of the top diamond at this x
                    float yb =
                        px < width / 2f
                            ? TileMetrics.TileH / 2f + px * 0.5f
                            : TileMetrics.TileH - (px - width / 2f) * 0.5f;
                    if (py > yb && py <= yb + TileMetrics.BlockDepth)
                        col =
                            px < width / 2f
                                ? new Color(185, 185, 185) // left face
                                : new Color(135, 135, 135); // right face
                }
                data[y * width + x] = col;
            }
        }

        var tex = new Texture2D(device, width, height);
        tex.SetData(data);
        return tex;
    }

    public static Texture2D CreateHighlightTexture(GraphicsDevice device)
    {
        var data = new Color[TileMetrics.TileW * TileMetrics.TileH];
        for (int y = 0; y < TileMetrics.TileH; y++)
        {
            for (int x = 0; x < TileMetrics.TileW; x++)
            {
                float d =
                    MathF.Abs(x + 0.5f - TileMetrics.TileW / 2f) / (TileMetrics.TileW / 2f)
                    + MathF.Abs(y + 0.5f - TileMetrics.TileH / 2f) / (TileMetrics.TileH / 2f);
                data[y * TileMetrics.TileW + x] =
                    d > 1f ? Color.Transparent
                    : d > 0.85f ? Color.White
                    : Color.White * 0.25f; // premultiplied alpha
            }
        }

        var tex = new Texture2D(device, TileMetrics.TileW, TileMetrics.TileH);
        tex.SetData(data);
        return tex;
    }
}
