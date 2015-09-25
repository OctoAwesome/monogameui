using MonoGameUi;
using Microsoft.Xna.Framework.Content;

namespace SampleClient
{
    internal class CustomSkin : Skin
    {
        public CustomSkin(ContentManager content) : base(content)
        {
            StyleSkins.Add("special", (c) =>
            {
                if (c is Button)
                    c.Width = 200;
            });
        }
    }
}
