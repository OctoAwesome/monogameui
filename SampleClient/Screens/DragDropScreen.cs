using Microsoft.Xna.Framework;
using MonoGameUi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleClient.Screens
{
    internal sealed class DragDropScreen : Screen
    {
        public DragDropScreen(BaseScreenComponent manager) : base(manager)
        {
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
                args.DragObject = "Button 1";
                Console.WriteLine("Start Drag");
            };
            Button button2 = Button.TextButton(manager, "Button 2");
            Button button3 = Button.TextButton(manager, "Button 3");
            button3.StartDrag += (args) => { args.Handled = true; args.DragObject = "Button 3"; };
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
                Background = new BorderBrush(Color.White)
            };

            panel.EndDrop += (args) =>
            {
                args.Handled = true;
                Console.WriteLine(args.DragObject);
            };

            grid.AddControl(panel, 1, 0);
        }
    }
}
