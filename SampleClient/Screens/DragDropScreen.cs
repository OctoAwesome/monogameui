using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameUi;
using System;

namespace SampleClient.Screens
{
    internal sealed class DragDropScreen : Screen
    {
        private Brush defaultBrush = new BorderBrush(Color.LightYellow);

        private Brush dragTargetBrush = new BorderBrush(Color.Yellow);

        private Texture2D dragIcon;

        public DragDropScreen(BaseScreenComponent manager) : base(manager)
        {
            Button backButton = Button.TextButton(manager, "Back");
            backButton.HorizontalAlignment = HorizontalAlignment.Left;
            backButton.VerticalAlignment = VerticalAlignment.Top;
            backButton.LeftMouseClick += (s, e) => { manager.NavigateBack(); };
            Controls.Add(backButton);

            dragIcon = manager.Content.Load<Texture2D>("drag");

            Grid grid = new Grid(manager)
            {
                Width = 600,
                Height = 400,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Background = new BorderBrush(Color.Red)
            };
            Controls.Add(grid);

            grid.Columns.Add(new ColumnDefinition() { ResizeMode = ResizeMode.Parts, Width = 1 });
            grid.Columns.Add(new ColumnDefinition() { ResizeMode = ResizeMode.Parts, Width = 1 });
            grid.Rows.Add(new RowDefinition() { ResizeMode = ResizeMode.Parts, Height = 1 });

            StackPanel buttons = new StackPanel(manager)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };

            Button button1 = Button.TextButton(manager, "Button 1");
            button1.StartDrag += (args) =>
            {
                args.Handled = true;
                args.Content = "Button 1";
                args.Sender = button1;
                args.Icon = dragIcon;
                args.IconSize = new Point(16, 16);
            };
            Button button2 = Button.TextButton(manager, "Button 2");
            Button button3 = Button.TextButton(manager, "Button 3");
            button3.StartDrag += (args) =>
            {
                args.Handled = true;
                args.Content = "Button 3";
                args.Sender = button3;
                args.Icon = dragIcon;
            };
            Button button4 = Button.TextButton(manager, "Button 4");
            buttons.Controls.Add(button1);
            buttons.Controls.Add(button2);
            buttons.Controls.Add(button3);
            buttons.Controls.Add(button4);

            grid.AddControl(buttons, 0, 0);

            Panel panel = new Panel(manager)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Background = defaultBrush
            };

            Label output = new Label(manager)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            panel.Controls.Add(output);

            panel.EndDrop += (args) =>
            {
                args.Handled = true;
                output.Text = args.Content.ToString();
            };

            panel.DropEnter += (args) =>
            {
                panel.Background = dragTargetBrush;
            };

            panel.DropLeave += (args) =>
            {
                panel.Background = defaultBrush;
            };

            grid.AddControl(panel, 1, 0);
        }
    }
}
