using MonoGameUi;
using engenious;
using engenious.Input;

namespace SampleClient.Screens
{
    internal class MouseCaptureScreen : Screen
    {
        private Point position = new Point();

        private Label output;

        public MouseCaptureScreen(BaseScreenComponent manager) : base(manager)
        {
            DefaultMouseMode = MouseMode.Captured;

            Background = new BorderBrush(Color.Green);

            StackPanel stack = new StackPanel(manager);
            Controls.Add(stack);

            Label title = new Label(manager)
            {
                TextColor = Color.White,
                Text = "Press ESC to return to Main Screen",
            };

            output = new Label(manager)
            {
                TextColor = Color.White,
                Text = position.ToString(),
            };

            stack.Controls.Add(title);
            stack.Controls.Add(output);
        }

        protected override void OnKeyPress(KeyEventArgs args)
        {
            if (args.Key == Keys.Escape)
            {
                args.Handled = true;
                Manager.NavigateBack();
            }

            base.OnKeyPress(args);
        }

        protected override void OnMouseMove(MouseEventArgs args)
        {
            if (args.MouseMode == MouseMode.Captured)
            {
                position = args.GlobalPosition;
            }

            output.Text = position.ToString();

            base.OnMouseMove(args);
        }
    }
}
