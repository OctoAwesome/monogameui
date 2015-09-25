namespace MonoGameUi
{
    public class Checkbox : ContentControl
    {
        public Brush BoxBrush { get; set; }

        public Brush HookBrush { get; set; }

        public bool Checked { get; set; }

        public Checkbox(IScreenManager manager) : base(manager)
        {
            ApplySkin(typeof(Checkbox));
        }
    }
}
