namespace MonoGameUi
{
    /// <summary>
    /// Struct zur Verwaltung von Rahmen-Informationen für Margins und Paddings.
    /// </summary>
    public struct Border
    {
        /// <summary>
        /// Linke Seite
        /// </summary>
        public int Left;

        /// <summary>
        /// Obere Seite
        /// </summary>
        public int Top;
        
        /// <summary>
        /// Rechte Seite
        /// </summary>
        public int Right;

        /// <summary>
        /// Untere Seite
        /// </summary>
        public int Bottom;

        /// <summary>
        /// Erstellt eine Border-Instanz mit den angegebenen Initialwerten.
        /// </summary>
        /// <param name="left">Abstand links</param>
        /// <param name="top">Abstand oben</param>
        /// <param name="right">Abstand rechts</param>
        /// <param name="bottom">Abstand unten</param>
        public Border(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        /// <summary>
        /// Erstellt einen Border mit gleichem Abstand auf allen vier Seiten.
        /// </summary>
        /// <param name="value">Wert für alle vier Seiten.</param>
        /// <returns>Border-Instanz</returns>
        public static Border All(int value)
        {
            return new Border()
            {
                Bottom = value,
                Left = value,
                Right = value,
                Top = value
            };
        }

        /// <summary>
        /// Erstellt einen Border auf Basis der Angaben für horizontale und vertikale Werte.
        /// </summary>
        /// <param name="horizontal">Abstand für horizontale Seiten (links, rechts)</param>
        /// <param name="vertical">Abstand für vertikale Seiten (oben, unten)</param>
        /// <returns>Border-Instanz</returns>
        public static Border All(int horizontal, int vertical)
        {
            return new Border()
            {
                Bottom = vertical,
                Left = horizontal,
                Right = horizontal,
                Top = vertical
            };
        }

        /// <summary>
        /// Erstellt eine Border-Instanz mit den angegebenen Initialwerten.
        /// </summary>
        /// <param name="left">Abstand links</param>
        /// <param name="top">Abstand oben</param>
        /// <param name="right">Abstand rechts</param>
        /// <param name="bottom">Abstand unten</param>
        /// <returns>Border-Instanz</returns>
        public static Border All(int left, int top, int right, int bottom)
        {
            return new Border()
            {
                Bottom = bottom,
                Left = left,
                Right = right,
                Top = top
            };
        }

        /// <summary>
        /// Wandelt die aktuelle Border-Instanz in einen string um.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}/{1}/{2}/{3}", Left, Top, Right, Bottom);
        }

        /// <summary>
        /// Überprüft, ob die aktuelle Border-Instanz gleich der gegebenen ist.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Border))
                return false;

            Border other = (Border)obj;

            return other.Left == Left && 
                other.Right == Right && 
                other.Top == Top && 
                other.Bottom == Bottom;
        }

        /// <summary>
        /// Gibt einen Hashwert zurück
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Left + Right + Top + Bottom;
        }
    }
}
