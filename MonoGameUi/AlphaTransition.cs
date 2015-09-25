using System;

namespace MonoGameUi
{
    public sealed class AlphaTransition : Transition
    {
        private float from;
        private float to;

        public AlphaTransition(Control control, Func<float, float> curve, TimeSpan time, float to)
            : this(control, curve, time, TimeSpan.Zero, to) { }

        public AlphaTransition(Control control, Func<float, float> curve, TimeSpan time, TimeSpan delay, float to) 
            : base(control, curve, time, delay)
        {
            from = control.Alpha;
            this.to = to;
        }

        protected override void ApplyValue(Control control, float position)
        {
            float value = (to - from) * position;
            control.Alpha = from + value;
        }

        public override Transition Clone(Control control)
        {
            return new AlphaTransition(control, Curve, Time, Delay, to);
        }
    }
}
