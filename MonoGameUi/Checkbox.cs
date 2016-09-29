using engenious;
using engenious.Graphics;

namespace MonoGameUi
{
    /// <summary>
    /// Steuerelement, in das der Nutzer ein Häkchen setzen kann.
    /// </summary>
    public class Checkbox : ContentControl
    {
        /// <summary>
        /// Brush für das Kästchen.
        /// </summary>
        public Brush BoxBrush { get; set; }

        /// <summary>
        /// Brush für den inneren Bereich des Checkbox-Kästchens.
        /// </summary>
        public Brush InnerBoxBrush { get; set; }

        /// <summary>
        /// Brush für den Haken der Checkbox.
        /// </summary>
        public Brush HookBrush { get; set; }

        private bool boxChecked = false;

        /// <summary>
        /// Gibt an, ob der Haken gesetzt ist.
        /// </summary>
        public bool Checked
        {
            get
            {
                return boxChecked;
            }

            set
            {
                boxChecked = value;
                if (CheckedChanged != null)
                    CheckedChanged.Invoke(boxChecked);
            }
        }

        /// <summary>
        /// Wird aufgerufen, wenn sich der Status der Checkbox geändert hat.
        /// </summary>
        public event CheckedChangedDelegate CheckedChanged;

        /// <summary>
        /// Erzeugt eine neue Instanz der Klasse Checkbox
        /// </summary>
        /// <param name="manager">Der verwendete <see cref="IScreenManager"/></param>
        public Checkbox(BaseScreenComponent manager) : base(manager)
        {
            CanFocus = true;
            TabStop = true;
            ApplySkin(typeof(Checkbox));
        }

        /// <summary>
        /// Malt den Content des Controls.
        /// </summary>
        /// <param name="batch">Spritebatch</param>
        /// <param name="contentArea">Bereich für den Content in absoluten Koordinaten</param>
        /// <param name="gameTime">GameTime</param>
        /// <param name="alpha">Die Transparenz des Controls.</param>
        protected override void OnDrawContent(SpriteBatch batch, Rectangle contentArea, GameTime gameTime, float alpha)
        {
            int innerDistanceX = contentArea.Width / 18;
            int innerDistanceY = contentArea.Height / 18;

            int hookDistanceX = contentArea.Height / 7;
            int hookDistanceY = contentArea.Width / 7;

            BoxBrush.Draw(batch, contentArea, alpha);
            InnerBoxBrush.Draw(batch, new Rectangle(contentArea.X + innerDistanceX, contentArea.Y + innerDistanceY,
                contentArea.Width - innerDistanceX * 2, contentArea.Height - innerDistanceY * 2), alpha);
            if (Checked)
                HookBrush.Draw(batch, new Rectangle(contentArea.X + hookDistanceX, contentArea.Y + +hookDistanceY,
                    contentArea.Width - hookDistanceX * 2, contentArea.Height - hookDistanceY * 2), alpha);
        }

        /// <summary>
        /// Wird aufgerufen, wenn mit der linken Maustaste auf das Steuerelement geklickt wird.
        /// </summary>
        /// <param name="args">Weitere Informationen zum Ereignis.</param>
        protected override void OnLeftMouseClick(MouseEventArgs args)
        {
            Checked = !Checked;
        }

        /// <summary>
        /// Wird aufgerufen, wenn eine Taste gedrückt wird.
        /// </summary>
        /// <param name="args">Zusätzliche Daten zum Event.</param>
        protected override void OnKeyDown(KeyEventArgs args)
        {
            if (Focused == TreeState.Active && args.Key == engenious.Input.Keys.Enter)
                Checked = !Checked;
        }

        /// <summary>
        /// Delegat für CheckedChanged-Events.
        /// </summary>
        /// <param name="Checked">Gibt an, ob das Steuerelement aktiviert ist.</param>
        public delegate void CheckedChangedDelegate(bool Checked);
    }
}
