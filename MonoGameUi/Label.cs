using System;
using System.Collections.Generic;
using System.Text;
using engenious;
using engenious.Graphics;

namespace MonoGameUi
{
    /// <summary>
    /// Standard Text-Anzeige Control.
    /// </summary>
    public class Label : Control
    {
        private List<string> lines = new List<string>();

        private string text = string.Empty;

        public event PropertyChangedDelegate<String> TextChanged;

        private SpriteFont font = null;

        private Color textColor = Color.Black;

        private HorizontalAlignment horizontalTextAlignment = HorizontalAlignment.Center;

        private VerticalAlignment verticalTextAlignment = VerticalAlignment.Center;

        private bool wordWrap = false;

        /// <summary>
        /// Gibt den enthaltenen Text an oder legt diesen fest.
        /// </summary>
        public string Text
        {
            get { return text ?? string.Empty; }
            set
            {
                if (text != value)
                {
                    if (TextChanged != null)
                        TextChanged(this, new PropertyEventArgs<String>(text, value));
                    text = value;
                    InvalidateDimensions();
                }
            }
        }

        /// <summary>
        /// Gibt die Schriftart an mit der der Inhalt gezeichnet werden soll oder legt diese fest.
        /// </summary>
        public SpriteFont Font
        {
            get { return font; }
            set
            {
                if (font != value)
                {
                    font = value;
                    InvalidateDimensions();
                }
            }
        }

        /// <summary>
        /// Gibt die Textfarbe an mit der der Inhalt gezeichnet werden soll oder legt diese fest.
        /// </summary>
        public Color TextColor
        {
            get { return textColor; }
            set
            {
                if (textColor != value)
                {
                    textColor = value;
                    InvalidateDrawing();
                }
            }
        }

        /// <summary>
        /// Gibt die Ausrichtung des Textes innerhalb des Controls auf horizontaler Ebene an.
        /// </summary>
        public HorizontalAlignment HorizontalTextAlignment
        {
            get { return horizontalTextAlignment; }
            set
            {
                if (horizontalTextAlignment != value)
                {
                    horizontalTextAlignment = value;
                    InvalidateDimensions();
                }
            }
        }

        /// <summary>
        /// Gibt die Ausrichtung des Textes innerhalb des Controls auf vertikaler Ebene an.
        /// </summary>
        public VerticalAlignment VerticalTextAlignment
        {
            get { return verticalTextAlignment; }
            set
            {
                if (verticalTextAlignment != value)
                {
                    verticalTextAlignment = value;
                    InvalidateDimensions();
                }
            }
        }

        /// <summary>
        /// Gibt an, ob das Control automatisch den Text an geeigneter Stelle 
        /// umbrechen soll, falls er nicht in eine Zeile passt.
        /// </summary>
        public bool WordWrap
        {
            get { return wordWrap; }
            set
            {
                if (wordWrap != value)
                {
                    wordWrap = value;
                    InvalidateDimensions();
                }
            }
        }

        public Label(BaseScreenComponent manager, string style = "") :
            base(manager, style)
        {
            ApplySkin(typeof(Label));
        }

        protected override void OnDrawContent(SpriteBatch batch, Rectangle area, GameTime gameTime, float alpha)
        {
            // Rahmenbedingungen fürs Rendern checken
            if (Font == null) return;

            Vector2 offset = new Vector2(area.X, area.Y);

            if (WordWrap)
            {
                int totalHeight = 0;
                foreach (var line in lines)
                {
                    Vector2 lineSize = Font.MeasureString(line);
                    totalHeight += (int)lineSize.Y;
                }

                switch (VerticalTextAlignment)
                {
                    case VerticalAlignment.Top:
                        break;
                    case VerticalAlignment.Bottom:
                        offset.Y = area.Y + area.Height - totalHeight;
                        break;
                    case VerticalAlignment.Center:
                        offset.Y = area.Y + (area.Height - totalHeight) / 2;
                        break;
                }

                foreach (var line in lines)
                {
                    Vector2 lineSize = Font.MeasureString(line);
                    switch (HorizontalTextAlignment)
                    {
                        case HorizontalAlignment.Left:
                            offset.X = area.X;
                            break;
                        case HorizontalAlignment.Center:
                            offset.X = area.X + (area.Width - lineSize.X) / 2;
                            break;
                        case HorizontalAlignment.Right:
                            offset.X = area.X + area.Width - lineSize.X;
                            break;
                    }

                    batch.DrawString(Font, line, offset, TextColor * alpha);

                    offset.Y += (int)lineSize.Y;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Text))
                    batch.DrawString(Font, Text, offset, TextColor * alpha);
            }
        }

        public override Point CalculcateRequiredClientSpace(Point available)
        {
            if (Font == null) return Point.Zero;

            AnalyzeText(available);

            int width = 0;
            int height = 0;

            if (WordWrap)
            {
                foreach (var line in lines)
                {
                    Vector2 lineSize = Font.MeasureString(line);
                    width = Math.Max((int)lineSize.X, width);
                    height += (int)lineSize.Y;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(Text))
                {
                    return new Point(0, Font.LineSpacing);
                }

                Vector2 lineSize = Font.MeasureString(Text);
                return new Point((int)lineSize.X, (int)lineSize.Y);
            }

            return new Point(width, height);
        }

        private void AnalyzeText(Point available)
        {
            lines.Clear();
            if (Font == null) return;

            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrEmpty(Text))
                return;

            string[] l = Text.Split('\n');
            foreach (var line in l)
            {
                string[] words = line.Split(' ');

                foreach (var word in words)
                {
                    Vector2 size = Font.MeasureString(word);
                    Vector2 lineSize = Font.MeasureString(sb.ToString());

                    if (lineSize.X + size.X >= available.X)
                    {
                        lines.Add(sb.ToString());
                        sb.Clear();
                    }
                    sb.Append(word + " ");
                }

                lines.Add(sb.ToString());
                sb.Clear();
            }
        }
    }
}
