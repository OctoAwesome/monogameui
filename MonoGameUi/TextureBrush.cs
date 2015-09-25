using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGameUi
{
    public sealed class TextureBrush : Brush
    {
        public Texture2D Texture { get; set; }

        public TextureBrushMode Mode { get; set; }

        public override void Draw(SpriteBatch batch, Rectangle area, float alpha)
        {
            throw new NotImplementedException();
        }
    }

    public enum TextureBrushMode
    {
        Stretch,
        FitMin,
        FitMax,
        Tile
    }
}
