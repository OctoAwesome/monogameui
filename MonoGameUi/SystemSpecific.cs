namespace MonoGameUi
{
    public sealed class SystemSpecific
    {
        public static void ClearClipboard()
        {
            System.Windows.Forms.Clipboard.Clear();
        }

        public static void SetClipboardText(string text)
        {
            System.Windows.Forms.Clipboard.SetText(text);
        }

        public static string GetClipboardText()
        {
            return System.Windows.Forms.Clipboard.GetText();
        }
    }
}
