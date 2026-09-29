using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoBusiness.Input;
using MonoBusiness.Rendering;
using MonoBusiness.World;

namespace MonoBusiness;

public class Game1 : Game
{
    readonly GraphicsDeviceManager _graphics;
    readonly Grid _worldGrid = new(WorldSettings.GridW, WorldSettings.GridH, WorldSettings.GridZ);
    readonly Camera2D _camera = new();

    private SpriteBatch _spriteBatch;
    private TerrainRenderer _terrainRenderer;
    private InputState _input;
    private float _time;
    private Point? _hover;
    private bool _isResizing;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = 1280,
            PreferredBackBufferHeight = 720,
        };
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += OnClientSizeChanged;
    }

    protected override void Initialize()
    {
        _camera.Position = IsoProjection.TileToWorld(
            _worldGrid.Width / 2,
            _worldGrid.Height / 2,
            0
        );
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        Services.AddService(_spriteBatch);

        _terrainRenderer = new TerrainRenderer(GraphicsDevice, Content);
        _input = new InputState();
        TerrainGenerator.Generate(_worldGrid, 1996); // Swap the seed later
    }

    protected override void UnloadContent()
    {
        _terrainRenderer.Dispose();
        _spriteBatch.Dispose();
    }

    protected override void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _time += deltaTime;
        _input.Update();

        if (_input.IsDown(Keys.Escape))
            Exit();
        if (_input.WasPressed(Keys.R))
            TerrainGenerator.Generate(_worldGrid, Environment.TickCount);

        UpdateCamera(deltaTime);
        UpdateTileEditing();

        base.Update(gameTime);
    }

    void UpdateCamera(float deltaTime)
    {
        var move = Vector2.Zero;
        if (_input.IsDown(Keys.W) || _input.IsDown(Keys.Up))
            move.Y -= 1;
        if (_input.IsDown(Keys.S) || _input.IsDown(Keys.Down))
            move.Y += 1;
        if (_input.IsDown(Keys.A) || _input.IsDown(Keys.Left))
            move.X -= 1;
        if (_input.IsDown(Keys.D) || _input.IsDown(Keys.Right))
            move.X += 1;
        if (move != Vector2.Zero)
            _camera.PanScreen(Vector2.Normalize(move) * 600f * deltaTime);

        if (_input.MiddleHeld)
            _camera.PanScreen(-_input.MouseDelta);

        if (_input.ScrollDelta != 0)
            _camera.ZoomAt(
                _input.MousePosition,
                _input.ScrollDelta > 0 ? 1.1f : 1f / 1.1f,
                GraphicsDevice.Viewport
            );
    }

    void UpdateTileEditing()
    {
        _hover = IsActive
            ? TilePicker.Pick(
                _worldGrid,
                _camera.ScreenToWorld(_input.MousePosition, GraphicsDevice.Viewport)
            )
            : null;

        if (_hover is not Point p)
        {
            Window.Title = "WASD/MMB pan - scroll zoom - LMB raise - RMB lower - R reroll";
            return;
        }

        if (_input.LeftClicked)
            _worldGrid.Raise(p.X, p.Y);
        if (_input.RightClicked)
            _worldGrid.Lower(p.X, p.Y);

        Window.Title =
            $"Tile ({p.X}, {p.Y}) - height {_worldGrid.Tiles[p.X, p.Y].height} - zoom {_camera.Zoom:0.00}x";
    }

    void OnClientSizeChanged(object sender, EventArgs ev)
    {
        if (_isResizing)
        {
            return;
        }
        if (Window.ClientBounds.Width <= 0 || Window.ClientBounds.Height <= 0)
        {
            return;
        }

        _isResizing = true;
        _graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
        _graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
        _graphics.ApplyChanges();
        _isResizing = false;
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(24, 20, 37));
        _spriteBatch.Begin(
            samplerState: SamplerState.PointClamp,
            transformMatrix: _camera.GetTransform(GraphicsDevice.Viewport)
        );
        _terrainRenderer.Draw(_spriteBatch, _worldGrid, _time, _hover);
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
