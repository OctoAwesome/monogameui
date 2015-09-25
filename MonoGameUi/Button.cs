using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonoGameUi
{
    /// <summary>
    /// Standard Schaltfläche
    /// </summary>
    public class Button : ContentControl
    {
        public Button(IScreenManager manager, string style = "")
            : base(manager, style)
        {
            TabStop = true;
            CanFocus = true;

            ApplySkin(typeof(Button));
        }

        protected override void OnKeyPress(KeyEventArgs args)
        {
            base.OnKeyPress(args);

            if (Focused == TreeState.Active &&
                (args.Key == Keys.Enter || args.Key == Keys.Space))
            {
                EventArgs e = new EventArgs();

                OnExecuted(e);
                if (Executed != null)
                    Executed(this, e);

                args.Handled = true;
            }
        }

        protected override void OnLeftMouseClick(MouseEventArgs args)
        {
            base.OnLeftMouseClick(args);

            EventArgs e = new EventArgs();
            OnExecuted(e);
            if (Executed != null)
                Executed(this, e);

            args.Handled = true;
        }

        protected virtual void OnExecuted(EventArgs args) { }

        public event EventDelegate Executed;

        /// <summary>
        /// Initialisiert einen Standard-Button mit Text-Inhalt
        /// </summary>
        /// <param name="text">Enthaltener Text</param>
        /// <returns>Button-Instanz</returns>
        public static Button TextButton(IScreenManager manager, string text, string style = "")
        {
            return new Button(manager, style)
            {
                Content = new Label(manager) { Text = text }
            };
        }
    }
}
