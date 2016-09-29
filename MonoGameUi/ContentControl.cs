using System.Linq;

namespace MonoGameUi
{
    public class ContentControl : Control
    {
        /// <summary>
        /// Das enthaltene Control.
        /// </summary>
        public Control Content
        {
            get { return Children.FirstOrDefault(); }
            set
            {
                if (Content != value)
                {
                    Children.Clear();
                    if (value != null)
                        Children.Add(value);
                }
            }
        }

        public ContentControl(BaseScreenComponent manager, string style = "") :
            base(manager, style)
        {
            ApplySkin(typeof(ContentControl));
        }
    }
}
