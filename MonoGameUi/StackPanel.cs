using System;
using System.Linq;
using engenious;

namespace MonoGameUi
{
    /// <summary>
    /// Container zur Aufreihnung von weiteren Controls in einer bestimmten Ausrichtung.
    /// </summary>
    public class StackPanel : ContainerControl
    {
        private Orientation orientation = Orientation.Vertical;

        /// <summary>
        /// Gibt die Ausrichtungsrichtung des Stacks an oder legt diesen fest.
        /// </summary>
        public Orientation Orientation
        {
            get { return orientation; }
            set
            {
                if (orientation != value)
                {
                    PropertyEventArgs<Orientation> args = new PropertyEventArgs<Orientation>
                    {
                        OldValue = orientation,
                        NewValue = value
                    };

                    orientation = value;
                    InvalidateDimensions();

                    OnOrientationChanged(args);
                    if (OrientationChanged != null)
                        OrientationChanged(this, args);
                }
            }
        }

        /// <summary>
        /// Erzeugt eine neue Instanz der Klasse StackPanel.
        /// </summary>
        /// <param name="manager">Der <see cref="IScreenManager"/></param>
        public StackPanel(BaseScreenComponent manager) : base(manager)
        {
            ApplySkin(typeof(StackPanel));
        }

        public override Point GetExpectedSize(Point available)
        {
            Point client = GetMaxClientSize(available);
            Point result = GetMinClientSize(available);

            foreach (var control in Controls.ToArray())
            {
                Point expected = control.GetExpectedSize(client);
                if (Orientation == Orientation.Horizontal)
                {
                    result.X += expected.X;
                    result.Y = Math.Max(result.Y, expected.Y);
                }
                else if (Orientation == Orientation.Vertical)
                {
                    result.Y += expected.Y;
                    result.X = Math.Max(result.X, expected.X);
                }
            }

            return result + Borders;
        }

        public override void SetActualSize(Point available)
        {
            Point minSize = GetExpectedSize(available);
            SetDimension(minSize, available);

            // Placement
            Point result = Point.Zero;
            foreach (var control in Controls.ToArray())
            {
                control.SetActualSize(ActualClientSize);
                if (Orientation == Orientation.Horizontal)
                {
                    control.ActualPosition = new Point(result.X, control.ActualPosition.Y);
                    result.X += control.ActualSize.X;
                    result.Y = Math.Max(result.Y, control.ActualSize.Y);
                }
                else if (Orientation == Orientation.Vertical)
                {
                    control.ActualPosition = new Point(control.ActualPosition.X, result.Y);
                    result.Y += control.ActualSize.Y;
                    result.X = Math.Max(result.X, control.ActualSize.X);
                }
            }
        }

        /// <summary>
        /// Signialisiert die Veränderung der Orientation-Eigenschaft.
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnOrientationChanged(PropertyEventArgs<Orientation> args) { }

        public event PropertyChangedDelegate<Orientation> OrientationChanged;
    }
}
