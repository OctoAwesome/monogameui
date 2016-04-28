using engenious;
using engenious.Graphics;


namespace MonoGameUi
{
    /// <summary>
    /// Brush für einfarbige Hintergründe und automatisch generierte Umrahmungslinien.
    /// </summary>
    public class BorderBrush : Brush
    {
        private Texture2D tex;

        private int lineWidth;

        private Color lineColor;

        private LineType lineType;

        /// <summary>
        /// Gibt die Hintergrund-Farbe des Rahmens zurück oder legt diese fest.
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// Gibt die Liniendicke des Rahmens zurück oder legt diese fest.
        /// </summary>
        public int LineWidth
        {
            get { return lineWidth; }
            set
            {
                if (lineWidth != value)
                {
                    lineWidth = value;
                    RebuildTexture();
                }
            }
        }

        /// <summary>
        /// Legt die Linienfarbe des Rahmens zurück oder liegt diese fest.
        /// </summary>
        public Color LineColor
        {
            get { return lineColor; }
            set
            {
                if (lineColor != value)
                {
                    lineColor = value;
                }
            }
        }

        /// <summary>
        /// Gibt die Linienart des Rahmens zurück oder legt diese fest.
        /// </summary>
        public LineType LineType
        {
            get { return lineType; }
            set
            {
                if (lineType != value)
                {
                    lineType = value;
                    RebuildTexture();
                }
            }
        }

        /// <summary>
        /// Erzeugt eine neue Instanz der BorderBrush-Klasse
        /// </summary>
        /// <param name="backgroundColor">Die Hintergrundfarbe</param>
        public BorderBrush(Color backgroundColor) :
            this(backgroundColor, LineType.None, Color.Transparent) { }

        /// <summary>
        /// Erzeugt eine neue Instanz der BorderBrush-Klasse
        /// </summary>
        /// <param name="lineType">Der Typ der Umrandungslinie</param>
        /// <param name="lineColor">Die Farbe der Umrandungslinie</param>
        /// <param name="lineWidth">Die Dicke der Umrandungslinie</param>
        public BorderBrush(LineType lineType, Color lineColor, int lineWidth = 1)
            : this(Color.Transparent, lineType, lineColor, lineWidth) { }

        /// <summary>
        /// Erzeugt eine neue Instanz der BorderBrush-Klasse
        /// </summary>
        /// <param name="backgroundColor">Die Hintergrundfarbe</param>
        /// <param name="lineType">Der Typ der Umrandungslinie</param>
        /// <param name="lineColor">Die Farbe der Umrandungslinie</param>
        /// <param name="lineWidth">Die Dicke der Umrandungslinie</param>
        public BorderBrush(Color backgroundColor, LineType lineType, Color lineColor, int lineWidth = 1)
        {
            BackgroundColor = backgroundColor;
            this.lineType = lineType;
            this.lineColor = lineColor;
            this.lineWidth = lineWidth;
            RebuildTexture();
        }

        private void RebuildTexture()
        {
            // Alte Textur entsorgen
            if (tex != null)
            {
                tex.Dispose();
                tex = null;
            }

            Color[] buffer;
            switch (LineType)
            {
                case LineType.None:
                    break;
                case LineType.Solid:
                    buffer = new Color[LineWidth * LineWidth];
                    for (int i = 0; i < buffer.Length; i++)
                        buffer[i] = LineColor;
                    tex = new Texture2D(Skin.Pix.GraphicsDevice, LineWidth, LineWidth);
                    tex.SetData(buffer);
                    MinWidth = MinHeight = (LineWidth * 2) + 1;
                    break;

                case LineType.Dotted:
                    buffer = new Color[LineWidth * LineWidth * 4];
                    for (int y = 0; y < LineWidth * 2; y++)
                    {
                        for (int x = 0; x < LineWidth * 2; x++)
                        {
                            int index = (y * LineWidth * 2) + x;
                            buffer[index] = (x < LineWidth && y < LineWidth ? Color.White : Color.Transparent);
                        }
                    }
                    tex = new Texture2D(Skin.Pix.GraphicsDevice, LineWidth * 2, LineWidth * 2);
                    tex.SetData(buffer);
                    MinWidth = MinHeight = (LineWidth * 2) + 1;
                    break;
            }
        }

        /// <summary>
        /// Zeichnet mit der aktuellen BorderBrush-Instanz
        /// </summary>
        /// <param name="batch">Der (bereits gestartete) SpriteBatch</param>
        /// <param name="area">Render-Bereich</param>
        /// <param name="alpha">Alpha-Blending</param>
        public override void Draw(SpriteBatch batch, Rectangle area, float alpha)
        {
            batch.Draw(Skin.Pix, area, BackgroundColor * alpha);

            // Rahmen malen
            if (tex != null)
            {
                batch.Draw(tex, 
                    new Rectangle(area.X, area.Y, area.Width, LineWidth), 
                    new Rectangle(0, 0, area.Width, LineWidth), LineColor * alpha);
                batch.Draw(tex, 
                    new Rectangle(area.X, area.Y, LineWidth, area.Height), 
                    new Rectangle(0, 0, LineWidth, area.Height), LineColor * alpha);
                batch.Draw(tex, 
                    new Rectangle(area.X, area.Y + area.Height - LineWidth, area.Width, LineWidth), 
                    new Rectangle(0, 0, area.Width, LineWidth), LineColor * alpha);
                batch.Draw(tex, 
                    new Rectangle(area.X + area.Width - LineWidth, area.Y, LineWidth, area.Height),
                    new Rectangle(0, 0, LineWidth, area.Height), LineColor * alpha);
            }
        }
    }

    /// <summary>
    /// Liste von möglichen Linientypen
    /// </summary>
    public enum LineType
    {
        /// <summary>
        /// Keine Linie
        /// </summary>
        None,

        /// <summary>
        /// Durchgängige Linie
        /// </summary>
        Solid,

        /// <summary>
        /// Gepunktete Linie
        /// </summary>
        Dotted,
    }
}
