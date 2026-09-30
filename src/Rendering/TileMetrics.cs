namespace MonoBusiness.Rendering;

public static class TileMetrics
{
    // Tile geometry (classic 2:1 isometric)
    // TileW * (TileH + Depth)
    public const int TileW = 64; // Width of the entire "block"
    public const int TileH = 32; // The top face of the "block"
    public const int BlockDepth = 16; // The sides of the "block"
}
