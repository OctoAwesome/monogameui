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

        private readonly PropertyEventArgs<Orientation> _orientationChangedEventArgs = new PropertyEventArgs<Orientation>();
        
        /// <summary>
        /// Gibt die Ausrichtung der Progress-Bar zurück oder legt diese fest.
        /// </summary>
        public Orientation Orientation
        {
            get { return orientation; }
            set
            {
                if (orientation == value) return;

                _orientationChangedEventArgs.OldValue = orientation;
                _orientationChangedEventArgs.NewValue = value;
                _orientationChangedEventArgs.Handled = false;

                orientation = value;
                InvalidateDimensions();

                OnOrientationChanged(_orientationChangedEventArgs);
                OrientationChanged?.Invoke(this, _orientationChangedEventArgs);
            }
        }

        private readonly PropertyEventArgs<int> _valueChangedEventArgs = new PropertyEventArgs<int>();
        /// <summary>
        /// Gibt den Fortschritt des Balkens an.
        /// </summary>
        public int Value
        {
            get { return barvalue; }
            set
            {
                if (barvalue == value) return;
                
                
                _valueChangedEventArgs.OldValue = barvalue;
                _valueChangedEventArgs.NewValue = value;
                _valueChangedEventArgs.Handled = false;

                barvalue = value;
                InvalidateDrawing();

                OnValueChanged(_valueChangedEventArgs);
                ValueChanged?.Invoke(this, _valueChangedEventArgs);
            }
        }
        private readonly PropertyEventArgs<int> _maximumChangedEventArgs = new PropertyEventArgs<int>();
        /// <summary>
        /// Gibt den Maximalwert des Balkens an.
        /// </summary>
        public int Maximum
        {
            get { return maximum; }
            set
            {
                if (maximum == value) return;

                _maximumChangedEventArgs.OldValue = maximum;
                _maximumChangedEventArgs.NewValue = value;
                _maximumChangedEventArgs.Handled = false;

                maximum = value;
                InvalidateDrawing();

                OnMaximumChanged(_maximumChangedEventArgs);
                MaximumChanged?.Invoke(this, _maximumChangedEventArgs);
            }
        }
        private readonly PropertyEventArgs<Brush> _barBrushChangedEventArgs = new PropertyEventArgs<Brush>();
        /// <summary>
        /// Gibt den Brush an, mit dem der Inhalt des Balkens gemalt werden soll.
        /// </summary>
        public Brush BarBrush
        {
            get { return barBrush; }
            set
            {
                if (barBrush == value) return;

                _barBrushChangedEventArgs.OldValue = barBrush;
                _barBrushChangedEventArgs.NewValue = value;
                _barBrushChangedEventArgs.Handled = false;

                barBrush = value;
                InvalidateDrawing();

                OnBarBrushChanged(_barBrushChangedEventArgs);
                BarBrushChanged?.Invoke(this, _barBrushChangedEventArgs);
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
