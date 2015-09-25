namespace MonoGameUi
{
    public class Panel : ContainerControl
    {
        public Panel(IScreenManager manager) : base(manager)
        {
            ApplySkin(typeof(Panel));
        }
    }
}
