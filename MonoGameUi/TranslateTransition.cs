using System;
using Microsoft.Xna.Framework;

namespace MonoGameUi
{
    public class TranslateTransition : Transition
    {
        private Point from;
        private Point to;

        public TranslateTransition(Control control, Func<float, float> curve, TimeSpan time, Point to)
            : this(control, curve, time, TimeSpan.Zero, to) { }

        public TranslateTransition(Control control, Func<float,float> curve, TimeSpan time, TimeSpan delay, Point to) 
            : base(control, curve, time, delay)
        {
            from = new Point(
                (int)control.Transformation.Translation.X, 
                (int)control.Transformation.Translation.Y);
            this.to = to;
        }

        protected override void ApplyValue(Control control, float value)
        {
            Point diff = to - from;

            Matrix trans = control.Transformation;
            trans.M41 = from.X + (diff.X * value);
            trans.M42 = from.Y + (diff.Y * value);
            control.Transformation = trans;
        }

        public override Transition Clone(Control control)
        {
            return new TranslateTransition(control, Curve, Time, Delay, to);
        }
    }
}
