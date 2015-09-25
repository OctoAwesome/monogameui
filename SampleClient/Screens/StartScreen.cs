using Microsoft.Xna.Framework;
using MonoGameUi;

namespace SampleClient.Screens
{
    internal class StartScreen : Screen
    {
        public StartScreen(IScreenManager manager) : base(manager)
        {
            Background = new BorderBrush(Color.DarkRed);

            Button nextButton = Button.TextButton(manager, "Next", "special");
            nextButton.LeftMouseClick += (s, e) =>
            {
                manager.NavigateToScreen(new SplitScreen(manager));
            };
            Controls.Add(nextButton);
        }
    }
}
