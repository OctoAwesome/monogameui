using System;
using engenious;
using engenious.Graphics;

namespace MonoGameUi
{
    public sealed class TextureBrush : Brush
    {
        public Texture2D Texture { get; set; }

        public TextureBrushMode Mode { get; set; }

        public TextureBrush(Texture2D texture, TextureBrushMode mode)
        {
            Texture = texture;
            Mode = mode;
        }

        public override void Draw(SpriteBatch batch, Rectangle area, float alpha)
        {
            if (Mode == TextureBrushMode.Stretch)
            {
                batch.Draw(Texture, area, Color.White * alpha);
            }
            else if (Mode == TextureBrushMode.Tile)
            {
                int countX = area.X / Texture.Width;
                int countY = area.Y / Texture.Height;

                for (int x = 0; x <= countX; x++)
                {
                    for (int y = 0; y <= countY; y++)
                    {
                        Rectangle destination = new Rectangle(area.X + x * Texture.Width, area.Y + y * Texture.Height, Texture.Width, Texture.Height);
                        batch.Draw(Texture, destination, Color.White * alpha);
                    }
                }
            }
            else
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
