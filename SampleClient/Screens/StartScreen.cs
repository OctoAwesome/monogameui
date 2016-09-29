using MonoGameUi;
using engenious;

namespace SampleClient.Screens
{
    internal class StartScreen : Screen
    {
        public StartScreen(BaseScreenComponent manager) : base(manager)
        {
            Background = new BorderBrush(Color.DarkRed);

            StackPanel stack = new StackPanel(manager);
            Controls.Add(stack);

            // Button zur Controls Demo
            Button controlScreenButton = Button.TextButton(manager, "Controls", "special");          //Button mit speziellen Style erstellen
            controlScreenButton.LeftMouseClick += (s, e) =>                                      //Click Event festlegen
            {
                manager.NavigateToScreen(new SplitScreen(manager));                     //Screen wechseln
            };
            stack.Controls.Add(controlScreenButton);                                                   //Button zu Root hinzufügen

            // Button zur Mouse Capture Demo
            Button capturedMouseButton = Button.TextButton(manager, "Captured Mouse", "special");
            capturedMouseButton.LeftMouseClick += (s, e) => manager.NavigateToScreen(new MouseCaptureScreen(manager));
            stack.Controls.Add(capturedMouseButton);

            Button tabDemoScreen = Button.TextButton(manager, "Tab Demo", "special");
            tabDemoScreen.LeftMouseClick += (s, e) => manager.NavigateToScreen(new TabScreen(manager));
            stack.Controls.Add(tabDemoScreen);

            Button dragDropScreen = Button.TextButton(manager, "Drag & Drop", "special");
            dragDropScreen.LeftMouseClick += (s, e) => manager.NavigateToScreen(new DragDropScreen(manager));
            stack.Controls.Add(dragDropScreen);
        }
    }
}
