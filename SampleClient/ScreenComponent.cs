using MonoGameUi;
using System;
using Microsoft.Xna.Framework;
using SampleClient.Screens;

namespace SampleClient
{
    internal class ScreenComponent : BaseScreenComponent
    {
        public ScreenComponent(Game game) : base(game)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Skin.Current = new CustomSkin(Content);

            Frame.Background = new BorderBrush(Color.CornflowerBlue);

            NavigateFromTransition = new AlphaTransition(Frame, Transition.Linear, TimeSpan.FromMilliseconds(200), 0f);
            NavigateToTransition = new AlphaTransition(Frame, Transition.Linear, TimeSpan.FromMilliseconds(200), 1f);

            NavigateToScreen(new StartScreen(this));
        }
    }
}
