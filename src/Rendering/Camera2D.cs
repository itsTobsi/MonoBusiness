using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoBusiness.Rendering;

public class Camera2D
{
    // World position shown at the center of the screen.
    public Vector2 Position { get; set; }
    public float Zoom { get; private set; } = 1f;
    public float MinZoom { get; set; } = 0.25f;
    public float MaxZoom { get; set; } = 4f;

    // World -> screen:
    // 1. Translate so the camera position becomes the origin
    // 2. Scale by Zoom around the origin
    // 3. Translate the origin to the middle of the screen
    public Matrix GetTransform(Viewport viewport) =>
        Matrix.CreateTranslation(-Position.X, -Position.Y, 0)
        * Matrix.CreateScale(Zoom)
        * Matrix.CreateTranslation(viewport.Width / 2f, viewport.Height / 2f, 0);

    public Vector2 ScreenToWorld(Vector2 screen, Viewport viewport) =>
        Vector2.Transform(screen, Matrix.Invert(GetTransform(viewport)));

    // Pan by a screen-space amount, so speed feels the same at any zoom.
    public void PanScreen(Vector2 screenDelta) => Position += screenDelta / Zoom;

    // Zoom while keeping the world point under the cursor fixed.
    public void ZoomAt(Vector2 screenPoint, float factor, Viewport viewport)
    {
        var before = ScreenToWorld(screenPoint, viewport);
        Zoom = MathHelper.Clamp(Zoom * factor, MinZoom, MaxZoom);
        Position += before - ScreenToWorld(screenPoint, viewport);
    }
}
