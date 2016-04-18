using System;

namespace MonoGameUi
{
    /// <summary>
    /// Transition zur Veränderung der Transparenz eines Steuerelements.
    /// </summary>
    public sealed class AlphaTransition : Transition
    {
        private float from;
        private float to;

        /// <summary>
        /// Erzeugt eine neue AlphaTransition für das angegebene Control.
        /// </summary>
        /// <param name="control">Zielcontrol.</param>
        /// <param name="curve">Bewegungskurve.</param>
        /// <param name="duration">Animationslänge.</param>
        /// <param name="to">Zieltransparenz des Controls.</param>
        public AlphaTransition(Control control, Func<float, float> curve, TimeSpan duration, float to)
            : this(control, curve, duration, TimeSpan.Zero, to) { }

        /// <summary>
        /// Erzeugt eine neue AlphaTransition für das angegebene Control.
        /// </summary>
        /// <param name="control">Zielcontrol.</param>
        /// <param name="curve">Bewegungskurve.</param>
        /// <param name="duration">Animationslänge.</param>
        /// <param name="delay">Wartezeit bis zum Start der Animation.</param>
        /// <param name="to">Zieltransparenz des Controls.</param>
        public AlphaTransition(Control control, Func<float, float> curve, TimeSpan duration, TimeSpan delay, float to) 
            : base(control, curve, duration, delay)
        {
            from = control.Alpha;
            this.to = to;
        }

        /// <summary>
        /// Wendet die Transition auf das Steuerelement an.
        /// </summary>
        /// <param name="control">Zielcontrol der Transition.</param>
        /// <param name="position">Wert im zeitlichen Ablauf der Transition.</param>
        protected override void ApplyValue(Control control, float position)
        {
            float value = (to - from) * position;
            control.Alpha = from + value;
        }

        /// <summary>
        /// Fertigt eine Kopie dieser Transition an, ersetzt aber das Zielcontrol.
        /// </summary>
        /// <param name="control">Das neue Zielcontrol.</param>
        /// <returns></returns>
        public override Transition Clone(Control control)
        {
            return new AlphaTransition(control, Curve, Duration, Delay, to);
        }
    }
}
