using System;
using engenious;
using engenious.Graphics;

namespace MonoGameUi
{
    public sealed class NineTileBrush : Brush
    {
        public Texture2D CenterTexture { get; set; }

        public Texture2D LeftTexture { get; set; }

        public Texture2D RightTexture { get; set; }

        public Texture2D TopTexture { get; set; }

        public Texture2D BottomTexture { get; set; }

        public Texture2D UpperLeftTexture { get; set; }

        public Texture2D UpperRightTexture { get; set; }

        public Texture2D LowerLeftTexture { get; set; }

        public Texture2D LowerRightTexture { get; set; }

        public Color Color { get; set; }

        public NineTileBrush()
        {
            Color = Color.White;
        }

        public override void Draw(SpriteBatch batch, Rectangle area, float alpha)
        {
            Color color = Color * alpha;

            // Center
            batch.Draw(CenterTexture,
                new Rectangle(area.X + UpperLeftTexture.Width, area.Y + UpperLeftTexture.Height, area.Width - UpperLeftTexture.Width - LowerRightTexture.Width, area.Height - UpperLeftTexture.Height - LowerRightTexture.Height),
                new Rectangle(0, 0, area.Width - UpperLeftTexture.Width - LowerRightTexture.Width, area.Height - UpperLeftTexture.Height - LowerRightTexture.Height), color);

            // Borders
            batch.Draw(TopTexture,
                new Rectangle(area.X + UpperLeftTexture.Width, area.Y, area.Width - UpperLeftTexture.Width - UpperRightTexture.Width, TopTexture.Height),
                new Rectangle(0, 0, area.Width - UpperLeftTexture.Width - UpperRightTexture.Width, TopTexture.Height), color);

            batch.Draw(BottomTexture,
                new Rectangle(area.X + UpperLeftTexture.Width, area.Y + area.Height - BottomTexture.Height, area.Width - LowerLeftTexture.Width - LowerRightTexture.Width, BottomTexture.Height),
                new Rectangle(0, 0, area.Width - LowerLeftTexture.Width - LowerRightTexture.Width, BottomTexture.Height), color);

            batch.Draw(LeftTexture,
                new Rectangle(area.X, area.Y + UpperLeftTexture.Height, LeftTexture.Width, area.Height - UpperLeftTexture.Height - LowerLeftTexture.Height),
                new Rectangle(0, 0, LeftTexture.Width, area.Height - UpperLeftTexture.Height - LowerLeftTexture.Height), color);

            batch.Draw(RightTexture,
                new Rectangle(area.X + area.Width - RightTexture.Width, area.Y + UpperRightTexture.Height, RightTexture.Width, area.Height - UpperRightTexture.Height - LowerRightTexture.Height),
                new Rectangle(0, 0, RightTexture.Width, area.Height - UpperRightTexture.Height - LowerRightTexture.Height), color);

            // Corners
            batch.Draw(UpperLeftTexture, new Rectangle(area.X, area.Y, UpperLeftTexture.Width, UpperLeftTexture.Height), color);
            batch.Draw(UpperRightTexture, new Rectangle(area.X + area.Size.Width - UpperRightTexture.Width, area.Y, UpperRightTexture.Width, UpperRightTexture.Height), color);
            batch.Draw(LowerLeftTexture, new Rectangle(area.X, area.Y + area.Size.Height - LowerLeftTexture.Height, LowerLeftTexture.Width, LowerLeftTexture.Height), color);
            batch.Draw(LowerRightTexture, new Rectangle(area.X + area.Size.Width - LowerRightTexture.Width, area.Y + area.Size.Height - LowerRightTexture.Height, LowerRightTexture.Width, LowerRightTexture.Height), color);
        }

        public static NineTileBrush FromSingleTexture(Texture2D texture, int cutX, int cutY)
        {
            if (texture == null)
                throw new ArgumentNullException("texture");

            if (cutX <= 0)
                throw new ArgumentException("cutX is too small");

            if (cutY <= 0)
                throw new ArgumentException("cutY is too small");

            if (2 * cutX >= texture.Width)
                throw new ArgumentException("cutX is too large.");

            if (2 * cutY >= texture.Height)
                throw new ArgumentException("cutY is too large.");

            NineTileBrush brush = new NineTileBrush();
            brush.MinWidth = (cutX * 2) + 1;
            brush.MinHeight = (cutY * 2) + 1;

            #region Ecken

            // Eck-Buffer
            uint[] buffer1 = new uint[cutX * cutY];

            // Upper Left
            brush.UpperLeftTexture = new Texture2D(texture.GraphicsDevice, cutX, cutY);
            texture.GetData(0, new Rectangle(0, 0, cutX, cutY), buffer1, 0, cutX * cutY);
            brush.UpperLeftTexture.SetData(buffer1);

            // Upper Right
            brush.UpperRightTexture = new Texture2D(texture.GraphicsDevice, cutX, cutY);
            texture.GetData(0, new Rectangle(texture.Width - cutX, 0, cutX, cutY), buffer1, 0, cutX * cutY);
            brush.UpperRightTexture.SetData(buffer1);

            // Lower Left
            brush.LowerLeftTexture = new Texture2D(texture.GraphicsDevice, cutX, cutY);
            texture.GetData(0, new Rectangle(0, texture.Height - cutY, cutX, cutY), buffer1, 0, cutX * cutY);
            brush.LowerLeftTexture.SetData(buffer1);

            // Lower Right
            brush.LowerRightTexture = new Texture2D(texture.GraphicsDevice, cutX, cutY);
            texture.GetData(0, new Rectangle(texture.Width - cutX, texture.Height - cutY, cutX, cutY), buffer1, 0, cutX * cutY);
            brush.LowerRightTexture.SetData(buffer1);

            #endregion

            #region Horizontale Kanten

            int buffer2SizeX = (texture.Width - cutX - cutX);
            int buffer2SizeY = cutY;
            uint[] buffer2 = new uint[buffer2SizeX * buffer2SizeY];

            // Upper Border
            brush.TopTexture = new Texture2D(texture.GraphicsDevice, buffer2SizeX, buffer2SizeY);
            texture.GetData(0, new Rectangle(cutX, 0, buffer2SizeX, buffer2SizeY), buffer2, 0, buffer2SizeX * buffer2SizeY);
            brush.TopTexture.SetData(buffer2);

            // Lower Border
            brush.BottomTexture = new Texture2D(texture.GraphicsDevice, buffer2SizeX, buffer2SizeY);
            texture.GetData(0, new Rectangle(cutX, texture.Height - cutY, buffer2SizeX, buffer2SizeY), buffer2, 0, buffer2SizeX * buffer2SizeY);
            brush.BottomTexture.SetData(buffer2);

            #endregion

            #region Vertikale Kanten

            int buffer3SizeX = cutX;
            int buffer3SizeY = (texture.Height - cutY - cutY);
            uint[] buffer3 = new uint[buffer3SizeX * buffer3SizeY];

            // Left Border
            brush.LeftTexture = new Texture2D(texture.GraphicsDevice, buffer3SizeX, buffer3SizeY);
            texture.GetData(0, new Rectangle(0, cutY, buffer3SizeX, buffer3SizeY), buffer3, 0, buffer3SizeX * buffer3SizeY);
            brush.LeftTexture.SetData(buffer3);

            // Right Border
            brush.RightTexture = new Texture2D(texture.GraphicsDevice, buffer3SizeX, buffer3SizeY);
            texture.GetData(0, new Rectangle(texture.Width - cutX, cutY, buffer3SizeX, buffer3SizeY), buffer3, 0, buffer3SizeX * buffer3SizeY);
            brush.RightTexture.SetData(buffer3);

            #endregion

            #region Zentrum

            int buffer4SizeX = (texture.Width - cutX - cutX);
            int buffer4SizeY = (texture.Height - cutY - cutY);
            uint[] buffer4 = new uint[buffer4SizeX * buffer4SizeY];

            // Left Border
            brush.CenterTexture = new Texture2D(texture.GraphicsDevice, buffer4SizeX, buffer4SizeY);
            texture.GetData(0, new Rectangle(cutX, cutY, buffer4SizeX, buffer4SizeY), buffer4, 0, buffer4SizeX * buffer4SizeY);
            brush.CenterTexture.SetData(buffer4);

            #endregion

            return brush;
        }
    }
}
