using System;
using engenious.Input;

namespace MonoGameUi
{
    /// <summary>
    /// Standard Schaltfläche
    /// </summary>
    public class Button : ContentControl
    {
        /// <summary>
        /// Erzeugt eine neue Instanz der Klasse Button.
        /// </summary>
        /// <param name="manager">Der <see cref="IScreenManager"/></param>
        /// <param name="style">(Optional) der zu verwendende Style</param>
        public Button(BaseScreenComponent manager, string style = "")
            : base(manager, style)
        {
            TabStop = true;
            CanFocus = true;

            ApplySkin(typeof(Button));
        }

        /// <summary>
        /// Wird aufgrefufen, wenn eine Taste gedrückt wird.
        /// </summary>
        /// <param name="args">Ein <see cref="KeyEventArgs"/>-Objekt mit weiteren Informationen zum Event</param>
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

        /// <summary>
        /// Wird aufgerufen, wenn mit der linken Maustaste auf das Steuerelement geklickt wird.
        /// </summary>
        /// <param name="args">Weitere Informationen zum Ereignis.</param>
        protected override void OnLeftMouseClick(MouseEventArgs args)
        {
            base.OnLeftMouseClick(args);

            EventArgs e = new EventArgs();
            OnExecuted(e);
            if (Executed != null)
                Executed(this, e);

            args.Handled = true;
        }

        protected override void OnTouchDown(TouchEventArgs args)
        {
            base.OnTouchDown(args);
        }

        protected override void OnTouchTap(TouchEventArgs args)
        {
            base.OnTouchTap(args);

            EventArgs e = new EventArgs();
            OnExecuted(e);
            if (Executed != null)
                Executed(this, e);

            args.Handled = true;
        }

        /// <summary>
        /// Methode, die aufgerufen wird, nachdem der Click-EventHadler (falls vorhanden) ausgeführt wurde.
        /// </summary>
        /// <param name="args">Weitere Informationen zum Ereignis.</param>
        protected virtual void OnExecuted(EventArgs args) { }

        /// <summary>
        /// Event, das aufgerufen wird, nachdem der Click-EventHadler (falls vorhanden) ausgeführt wurde.
        /// </summary>
        public event EventDelegate Executed;

        /// <summary>
        /// Initialisiert einen Standard-Button mit Text-Inhalt
        /// </summary>
        /// <param name="text">Enthaltener Text</param>
        /// <param name="manager">Der <see cref="IScreenManager"/></param>
        /// <param name="style">(Optional) Der zu verwendende Style</param>
        /// <returns>Button-Instanz</returns>
        public static Button TextButton(BaseScreenComponent manager, string text, string style = "")
        {
            return new Button(manager, style)
            {
                Content = new Label(manager) { Text = text }
            };
        }
    }
}
