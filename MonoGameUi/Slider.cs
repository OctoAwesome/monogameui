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
        /// Gibt den aktuellen Wert zurück oder legt diesen fest.
        /// </summary>
        public int Value { get; set; }

        public Slider(IScreenManager manager, string style = "")
            : base(manager, style)
        {
            CanFocus = true;
            TabStop = true;

            ApplySkin(typeof(Slider));
        }
    }
}
