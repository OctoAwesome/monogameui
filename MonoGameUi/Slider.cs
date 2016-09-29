using System;
using engenious;
using engenious.Graphics;
using engenious.Input;

namespace MonoGameUi
{
    /// <summary>
    /// Das Slider-Control erlaubt das Verschieben eines Reglers per Maus oder Tastatur.
    /// </summary>
    public class Slider : Control
    {
        /// <summary>
        /// Gibt die grafische Ausrichtung des Sliders zurück oder legt diese fest.
        /// </summary>
        public Orientation Orientation { get; set; }

        /// <summary>
        /// GIbt den Brush, mit dem der Slider-Hintergrund gemalt werden soll, zurück oder legt diesen fest.
        /// </summary>
        public Brush SliderBackgroundBrush { get; set; }

        /// <summary>
        /// Gibt den Brush, mit dem der Slider-Vordergrund gemalt werden soll, zurück oder legt diesen fest.
        /// </summary>
        public Brush SliderForegroundBrush { get; set; }

        /// <summary>
        /// Gibt den Maximalwert zurück oder legt diesen fest.
        /// </summary>
        public int Range { get; set; }

        /// <summary>
        /// Gibt die Breite/Höhe des Sliders zurück oder legt diesen fest.
        /// </summary>
        public int SliderWidth { get; set; }

        /// <summary>
        /// Der aktuelle Wert
        /// </summary>
        private int sliderValue;

        /// <summary>
        /// Gibt den aktuellen Wert zurück oder legt diesen fest.
        /// </summary>
        public int Value
        {
            get
            {
                return sliderValue;
            }
            set
            {
                sliderValue = value;
                if(ValueChanged != null)
                    ValueChanged.Invoke(sliderValue);
            }
        }

        /// <summary>
        /// Gibt an ob die Maus geklickt wird während das Control fokussiert ist
        /// </summary>
        private bool mouseClickActive = false;

        /// <summary>
        /// Wird ausgelöst wenn sich der Wert ändert
        /// </summary>
        public event ValueChangedDelegate ValueChanged;

        /// <summary>
        /// Erzeugt einen neuen Slider.
        /// </summary>
        /// <param name="manager">Der verwendete <see cref="IScreenManager"/></param>
        /// <param name="style">(Optional) Der zu verwendende Style.</param>
        public Slider(BaseScreenComponent manager, string style = "")
            : base(manager, style)
        {
            CanFocus = true;
            TabStop = true;

            Range = 100;
            Value = 50;

            ApplySkin(typeof(Slider));
        }

        /// <summary>
        /// Malt den Content des Controls
        /// </summary>
        /// <param name="batch">Spritebatch</param>
        /// <param name="contentArea">Bereich für den Content in absoluten Koordinaten</param>
        /// <param name="gameTime">GameTime</param>
        /// <param name="alpha">Die Transparenz des Controls.</param>
        protected override void OnDrawContent(SpriteBatch batch, Rectangle contentArea, GameTime gameTime, float alpha)
        {
            //Berechnet die mögliche Zeichenfläche für den SliderKnob
            Rectangle drawableKnobSpace = new Rectangle(contentArea.X + SliderWidth / 2, contentArea.Y + SliderWidth/2, contentArea.Width - SliderWidth, contentArea.Height - SliderWidth);

            //Berechnen des Werts wenn die Maus gehalten wird & das Control ausgewählt ist
            if (mouseClickActive)
            {
                MouseState mouse = Mouse.GetState();
                Point localMousePos =new Point(mouse.X-contentArea.X, mouse.Y-contentArea.Y); //Berechnet die Position relativ zum Control

                //Wenn der Slider Horizontal ist
                if (Orientation == Orientation.Horizontal)
                {
                    //Berechne den Wert des Sliders
                    Value = (int)Math.Round(localMousePos.X / ((float)contentArea.Width / Range));
                    if (localMousePos.X <= 0) Value = 0;                     //Wenn die Maus Position kleiner als 0 ist -> Value = 0
                    if (localMousePos.X >= contentArea.Width) Value = Range; //Wenn die Maus Position größer als die Breite des Controls -> Value = Range
                }

                //Wenn der Slider vertikal ist
                if (Orientation == Orientation.Vertical)
                {
                    //Berechne den Wert des Sliders
                    Value = (contentArea.Height - localMousePos.Y) / (contentArea.Height / Range);
                    if (Value <= 0) Value = 0;                      //Wenn die Maus Position kleiner als 0 ist -> Value = 0
                    if (Value >= Range) Value = Range;              //Wenn die Maus Position größer als die Breite des Controls -> Value = Range
                }

            }


            ////////////////Zeichnen des Sliders////////////////

            //Zeichne Background
            SliderBackgroundBrush.Draw(batch, contentArea, alpha);

            Rectangle sliderKnob = new Rectangle();

            //Wenn der Slider Horizontal ist
            if (Orientation == Orientation.Horizontal)
            {
                //Berechne die Position des SliderKnobs     
                sliderKnob.Y = contentArea.Y;                                                       //Y Koordinate des Knobs
                sliderKnob.Width = SliderWidth;                                                              //Der Slider ist SliderWidth breit
                float WidthRange = ((float)drawableKnobSpace.Width / Range);                        //Berechnet wieviel Pixel 1 in Value wert ist
                sliderKnob.X = (int)Math.Round(drawableKnobSpace.X + (WidthRange * Value) - SliderWidth/2);    //Berechnet die X Position des Knobs
                sliderKnob.Height = contentArea.Height;                                             //Der SliderKnob ist immer so hoch wie der Slider
            }
            else
            {
                //Berechne die Position des SliderKnobs     
                sliderKnob.X = contentArea.X;                                                       //Der SliderKnob beginnt immer am oberen Rand des Sliders
                sliderKnob.Height = SliderWidth;                                                             //Der Slider ist SliderWidthpx hoch
                float HeightRange = ((float)drawableKnobSpace.Height / Range);                      //Berechnet wieviel Pixel 1 in Value wert ist
                sliderKnob.Y = (int)Math.Round(drawableKnobSpace.Y + drawableKnobSpace.Height - (HeightRange *Value) - SliderWidth/2);    //Berechnet die X Position des Knobs
                sliderKnob.Width = contentArea.Width;                                               //Der SliderKnob ist immer so breit wie der Slider
            }

            SliderForegroundBrush.Draw(batch, sliderKnob, alpha);
        }

        /// <summary>
        /// Wird aufgerufen, wenn die linke Maustaste heruntergedrückt wird.
        /// </summary>
        /// <param name="args">Weitere Informationen zum Event.</param>
        protected override void OnLeftMouseDown(MouseEventArgs args)
        {
            mouseClickActive = true;
        }

        /// <summary>
        /// Wird aufgerufen, wenn die linke Maustaste losgelassen wird.
        /// </summary>
        /// <param name="args">Weitere Informationen zum Event.</param>
        protected override void OnLeftMouseUp(MouseEventArgs args)
        {
            mouseClickActive = false;
        }

        /// <summary>
        /// Delegat zur Änderung des Slider-Wertes.
        /// </summary>
        /// <param name="Value">Der neue Wert.</param>
        public delegate void ValueChangedDelegate(int Value);

        /// <summary>
        /// Wird aufgerufen, wenn eine Taste gedrückt ist.
        /// </summary>
        /// <param name="args">Zusätzliche Daten zum Event.</param>
        protected override void OnKeyPress(KeyEventArgs args)
        {
            if (Focused != TreeState.Active)
                return;


            if (Orientation == Orientation.Horizontal)
            {
                if (args.Key == Keys.Right && Value < Range)
                    Value++;
                if (args.Key == Keys.Left && Value > 0)
                    Value--;
            }
            else
            {
                if (args.Key == Keys.Up && Value < Range)
                    Value++;
                if (args.Key == Keys.Down && Value > 0)
                    Value--;
            }

            if (args.Key == Keys.D0)
                Value = 0;
        }
    }
}
