using engenious;
using engenious.Graphics;

namespace MonoGameUi
{
    /// <summary>
    /// Abstrakte Basisklasse für alle Sorten von Brushes.
    /// </summary>
    public abstract class Brush
    {
        /// <summary>
        /// Gibt die Minimale Breite des Brushs um noch gut auszusehen.
        /// </summary>
        public int MinWidth { get; protected set; }

        /// <summary>
        /// Gibt die minimale Höhe des Brushes um noch gut auszusehen.
        /// </summary>
        public int MinHeight { get; protected set; }

        /// <summary>
        /// Draw-Methode für den Brush.
        /// </summary>
        /// <param name="batch">Der (bereits gestartete) SpriteBatch</param>
        /// <param name="area">Render-Bereich</param>
        /// <param name="alpha">Alpha-Blending</param>
        public abstract void Draw(SpriteBatch batch, Rectangle area, float alpha);
    }
}
