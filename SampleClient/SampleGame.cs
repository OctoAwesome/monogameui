using System;
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
            ScreenComponent screenComponent = new ScreenComponent(this);
            Components.Add(screenComponent);

            screenComponent.KeyDown += (args) =>
            {
              Console.WriteLine("Down: " + args.Key.ToString());
            };

            screenComponent.KeyPress += (args) =>
            {
                Console.WriteLine("Press: " + args.Key.ToString());
            };

            screenComponent.KeyUp += (args) =>
            {
                Console.WriteLine("Up: " + args.Key.ToString());
            };


            base.Initialize();
        }
    }
}
