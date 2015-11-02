using Microsoft.Xna.Framework;
using MonoGameUi;
using Microsoft.Xna.Framework.Audio;

namespace SampleClient.Screens
{
    internal class StartScreen : Screen
    {
        public StartScreen(IScreenManager manager) : base(manager)
        {
            Background = new BorderBrush(Color.DarkRed);

            Button nextButton = Button.TextButton(manager, "Next", "special");          //Button mit speziellen Style erstellen
            nextButton.ClickSound = manager.Content.Load<SoundEffect>("click1");        //Click Sound festlegen
            nextButton.HoverSound = manager.Content.Load<SoundEffect>("rollover5");     //Hover Sound festlegen
            nextButton.LeftMouseClick += (s, e) =>                                      //Click Event festlegen
            {
                manager.NavigateToScreen(new SplitScreen(manager));                     //Screen wechseln
            };
            Controls.Add(nextButton);                                                   //Button zu Root hinzufügen
        }
    }
}
