using Microsoft.Xna.Framework;
using MonoGameUi;

namespace SampleClient.Screens
{
    internal class SplitScreen : Screen
    {
        public SplitScreen(IScreenManager manager) : base(manager)
        {
            Background = new BorderBrush(Color.Gray);

            Button backButton = Button.TextButton(manager, "Back");
            backButton.HorizontalAlignment = HorizontalAlignment.Left;
            backButton.VerticalAlignment = VerticalAlignment.Top;
            backButton.LeftMouseClick += (s, e) => { manager.NavigateBack(); };
            Controls.Add(backButton);
        }
    }
}
