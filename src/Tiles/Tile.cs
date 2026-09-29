using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoBusiness;

public class Tile
{
    // Changed it to Point as it already has an int x,y cords.
    // https://docs.monogame.net/api/Microsoft.Xna.Framework.Point.html
    public Point Position { get; set; }
    public int height { get; set; } // Should be able to just use a byte, won't go bellow 0 or above 255

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        // Write later when working on texture altas
    }
}
