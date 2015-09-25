using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SampleClient
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class SampleGame : Game
    {
        GraphicsDeviceManager graphics;

        public SampleGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Components.Add(new ScreenComponent(this));

            base.Initialize();
        }
    }
}
