using MonoGameUi;
using engenious.Content;


namespace SampleClient
{
    internal class CustomSkin : Skin
    {
        public CustomSkin(ContentManager content) : base(content)
        {
            //SoundEffect click = content.Load<SoundEffect>("click1");
            //SoundEffect hover = content.Load<SoundEffect>("rollover5");

            StyleSkins.Add("special", (c) =>
            {
                if (c is Button)
                {
                    c.Width = 200;
                    Button button = c as Button;
                    //button.ClickSound = click;
                    //button.HoverSound = hover;
                }
            });
        }
    }
}
