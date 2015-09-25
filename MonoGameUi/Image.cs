using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameUi
{
    public class Image : Control
    {
        // TODO: Skalierung als Parameter (also stretch, scale, fill,...)

        public Texture2D Texture { get; set; }

        public Image(IScreenManager manager, string style = "") :
            base(manager, style)
        {
            ApplySkin(typeof(Image));
        }

        public override Point CalculcateRequiredClientSpace(Point available)
        {
            if (Texture != null)
                return new Point(Texture.Width, Texture.Height);
            return base.CalculcateRequiredClientSpace(available);
        }

        protected override void OnDrawContent(SpriteBatch batch, Rectangle area, GameTime gameTime, float alpha)
        {
            if (Texture != null)
            {
                batch.Draw(Texture, area, Color.White * alpha);
            }
        }
    }
}
