using System;
using engenious;
using engenious.Graphics;

namespace MonoGameUi
{
    public class ProgressBar : Control
    {
        private Orientation orientation = Orientation.Horizontal;

        private int barvalue = 0;

        private int maximum = 100;

        private Brush barBrush = null;

        /// <summary>
        /// Gibt die Ausrichtung der Progress-Bar zurück oder legt diese fest.
        /// </summary>
        public Orientation Orientation
        {
            get { return orientation; }
            set
            {
                if (orientation != value)
                {
                    PropertyEventArgs<Orientation> args = new PropertyEventArgs<Orientation>()
                    {
                        OldValue = orientation,
                        NewValue = value,
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
        /// Gibt den Fortschritt des Balkens an.
        /// </summary>
        public int Value
        {
            get { return barvalue; }
            set
            {
                if (barvalue != value)
                {
                    PropertyEventArgs<int> args = new PropertyEventArgs<int>()
                    {
                        OldValue = barvalue,
                        NewValue = value,
                    };

                    barvalue = value;
                    InvalidateDrawing();

                    OnValueChanged(args);
                    if (ValueChanged != null)
                        ValueChanged(this, args);
                }
            }
        }

        /// <summary>
        /// Gibt den Maximalwert des Balkens an.
        /// </summary>
        public int Maximum
        {
            get { return maximum; }
            set
            {
                if (maximum != value)
                {
                    PropertyEventArgs<int> args = new PropertyEventArgs<int>()
                    {
                        OldValue = maximum,
                        NewValue = value,
                    };

                    maximum = value;
                    InvalidateDrawing();

                    OnMaximumChanged(args);
                    if (MaximumChanged != null)
                        MaximumChanged(this, args);
                }
            }
        }

        /// <summary>
        /// Gibt den Brush an, mit dem der Inhalt des Balkens gemalt werden soll.
        /// </summary>
        public Brush BarBrush
        {
            get { return barBrush; }
            set
            {
                if (barBrush != value)
                {
                    PropertyEventArgs<Brush> args = new PropertyEventArgs<Brush>()
                    {
                        OldValue = barBrush,
                        NewValue = value,
                    };

                    barBrush = value;
                    InvalidateDrawing();

                    OnBarBrushChanged(args);
                    if (BarBrushChanged != null)
                        BarBrushChanged(this, args);
                }
            }
        }

        public ProgressBar(BaseScreenComponent manager, string style = "") :
            base(manager, style)
        {
            ApplySkin(typeof(ProgressBar));
        }

        protected override void OnDrawContent(SpriteBatch batch, Rectangle contentArea, GameTime gameTime, float alpha)
        {
            if (BarBrush == null) return;

            int m = Math.Max(0, Maximum);
            int v = Math.Max(0, Math.Min(m, Value));
            float part = m > 0 ? (float)v / m : 1f;

            if (Orientation == Orientation.Horizontal)
                BarBrush.Draw(batch, new Rectangle(contentArea.X, contentArea.Y, 
                    (int)(contentArea.Width * part), contentArea.Height), alpha);
            else if (Orientation == Orientation.Vertical)
            {
                BarBrush.Draw(batch, new Rectangle(contentArea.X, contentArea.Y,
                    contentArea.Width, (int)(contentArea.Height * part)), alpha);
            }
        }

        protected virtual void OnOrientationChanged(PropertyEventArgs<Orientation> args) { }
        protected virtual void OnValueChanged(PropertyEventArgs<int> args) { }
        protected virtual void OnMaximumChanged(PropertyEventArgs<int> args) { }
        protected virtual void OnBarBrushChanged(PropertyEventArgs<Brush> args) { }

        public event PropertyChangedDelegate<Orientation> OrientationChanged;

        public event PropertyChangedDelegate<int> ValueChanged;

        public event PropertyChangedDelegate<int> MaximumChanged;

        public event PropertyChangedDelegate<Brush> BarBrushChanged;
    }
}
