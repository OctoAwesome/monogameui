using System;
using engenious;

namespace MonoGameUi
{
    /// <summary>
    /// Transition, die eine Trnslation auf das Zielcontrol anwendet.
    /// </summary>
    public class TranslateTransition : Transition
    {
        private Point from;
        private Point to;

        /// <summary>
        /// Erzeugt eine neue AlphaTransition für das angegebene Control.
        /// </summary>
        /// <param name="control">Zielcontrol.</param>
        /// <param name="curve">Bewegungskurve.</param>
        /// <param name="duration">Animationslänge.</param>
        /// <param name="to">Zieltranformation.</param>
        public TranslateTransition(Control control, Func<float, float> curve, TimeSpan duration, Point to)
            : this(control, curve, duration, TimeSpan.Zero, to) { }

        /// <summary>
        /// Erzeugt eine neue AlphaTransition für das angegebene Control.
        /// </summary>
        /// <param name="control">Zielcontrol.</param>
        /// <param name="curve">Bewegungskurve.</param>
        /// <param name="duration">Animationslänge.</param>
        /// <param name="delay">Wartezeit bis zum Start der Animation.</param>
        /// <param name="to">Zieltransformation.</param>
        public TranslateTransition(Control control, Func<float,float> curve, TimeSpan duration, TimeSpan delay, Point to) 
            : base(control, curve, duration, delay)
        {
            from = new Point(
                (int)control.Transformation.Translation.X, 
                (int)control.Transformation.Translation.Y);
            this.to = to;
        }

        /// <summary>
        /// Wendet die Transition auf das Steuerelement an.
        /// </summary>
        /// <param name="control">Zielcontrol der Transition.</param>
        /// <param name="value">Wert im zeitlichen Ablauf der Transition.</param>
        protected override void ApplyValue(Control control, float value)
        {
            Point diff = to - from;

            Matrix trans = control.Transformation;
            trans.M41 = from.X + (diff.X * value);
            trans.M42 = from.Y + (diff.Y * value);
            control.Transformation = trans;
        }

        /// <summary>
        /// Fertigt eine Kopie dieser Transition an, ersetzt aber das Zielcontrol.
        /// </summary>
        /// <param name="control">Das neue Zielcontrol.</param>
        /// <returns></returns>
        public override Transition Clone(Control control)
        {
            return new TranslateTransition(control, Curve, Duration, Delay, to);
        }
    }
}
