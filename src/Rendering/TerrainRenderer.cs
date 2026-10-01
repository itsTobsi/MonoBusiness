using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoBusiness.World;

namespace MonoBusiness.Rendering;

public sealed class TerrainRenderer : IDisposable
{
    static readonly Vector2 HalfWidth = new(TileMetrics.TileW / 2f, 0);
    readonly Texture2D _block,
        _grass,
        _highlight;

    public TerrainRenderer(GraphicsDevice device, ContentManager content)
    {
        _block = content.Load<Texture2D>("blocks");
        _grass = content.Load<Texture2D>("grass");
        _highlight = TileTextures.CreateHighlightTexture(device);
        // TileTextures.SaveGeneratedTexture(_block, "blocks"); // Used to save the 2D texture as a file
    }

    // Back to front: increasing row, then column, then height.
    public void Draw(SpriteBatch spriteBatch, Grid grid, float time, Point? hover)
    {
        for (int row = 0; row < grid.Height; row++)
        for (int column = 0; column < grid.Width; column++)
        {
            int h = grid.Tiles[column, row].Height;
            for (int z = 0; z <= h; z++)
            {
                var color =
                    z == h ? TerrainPalette.TopColor(h, time) : TerrainPalette.EarthColor(z);
                
                spriteBatch.Draw(
                    2 <= h && h <= 4 ? _grass : _block,
                    IsoProjection.TileToWorld(column, row, z) - HalfWidth,
                    color
                );
            }

            // Draw the highlight here so tiles in front still overlap it.
            if (hover is Point p && p.X == column && p.Y == row)
                spriteBatch.Draw(
                    _highlight,
                    IsoProjection.TileToWorld(column, row, h) - HalfWidth,
                    Color.Yellow
                );

            
        }
    }

    public void Dispose()
    {
        _block.Dispose();
        _highlight.Dispose();
    }
}
