using System;
using engenious;
using engenious.Input;

namespace SampleClient
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class SampleGame : Game
    {
        //GraphicsDeviceManager graphics;

        public SampleGame()
        {
            //graphics = new GraphicsDeviceManager(this);
            //graphics.PreferredBackBufferWidth = 1280;
            //graphics.PreferredBackBufferHeight = 720;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            KeyboardState state = Keyboard.GetState();
            if(state.IsKeyDown(Keys.F11))
            {
                //graphics.ToggleFullScreen();
            }
            if(state.IsKeyDown(Keys.F12))
            {
                //graphics.PreferredBackBufferWidth = 1920;
                //graphics.PreferredBackBufferHeight = 1080;
                //graphics.ApplyChanges();
            }
        }
    }
}
