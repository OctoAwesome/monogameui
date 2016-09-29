using engenious;
using engenious.Graphics;

namespace MonoGameUi
{
    /// <summary>
    /// Steuerelement, das ein Bild darstellt.
    /// </summary>
    public class Image : Control
    {
        // TODO: Skalierung als Parameter (also stretch, scale, fill,...)

        /// <summary>
        /// Das darzustellende Bild als Textur.
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// Erzeugt eine neue Instanz eines Bildsteuerlements.
        /// </summary>
        /// <param name="manager">Der verwendete <see cref="IScreenManager"/></param>
        /// <param name="style">(Optional) Der zu verwendende Style.</param>
        public Image(BaseScreenComponent manager, string style = "") :
            base(manager, style)
        {
            ApplySkin(typeof(Image));
        }

        /// <summary>
        /// Ist für die Berechnung des Client-Contents zuständig und erleichtert das automatische Alignment.
        /// </summary>
        /// <returns></returns>
        public override Point CalculcateRequiredClientSpace(Point available)
        {
            if (Texture != null)
                return new Point(Texture.Width, Texture.Height);
            return base.CalculcateRequiredClientSpace(available);
        }

        /// <summary>
        /// Malt den Content des Controls.
        /// </summary>
        /// <param name="batch">Spritebatch.</param>
        /// <param name="area">Bereich für den Content in absoluten Koordinaten.</param>
        /// <param name="gameTime">GameTime.</param>
        /// <param name="alpha">Die Transparenz des Controls.</param>
        protected override void OnDrawContent(SpriteBatch batch, Rectangle area, GameTime gameTime, float alpha)
        {
            if (Texture != null)
                batch.Draw(Texture, area, Color.White * alpha);
        }
    }
}
