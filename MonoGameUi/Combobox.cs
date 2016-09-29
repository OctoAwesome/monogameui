using System.Linq;
using engenious;

namespace MonoGameUi
{
    /// <summary>
    /// Typische Combobox zur Auswahl eines Elements aus einer Liste.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Combobox<T> : ListControl<T> where T : class
    {
        public Listbox<T> Selector { get; private set; }

        private ContentControl mainControl;

        public Combobox(BaseScreenComponent manager)
            : base(manager)
        {
            mainControl = new ContentControl(manager)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Margin = Border.All(0),
                Padding = Border.All(0),
            };
            Children.Add(mainControl);

            Selector = new Listbox<T>(manager);
            Selector.HorizontalAlignment = HorizontalAlignment.Left;
            Selector.VerticalAlignment = VerticalAlignment.Top;
            Selector.MaxHeight = 100;
            Selector.TemplateGenerator = GenerateControl;
            Selector.SelectedItemBrush = null;
            Selector.Background = new BorderBrush(Color.White);
            Selector.SelectedItemChanged += Selector_SelectedItemChanged;

            ApplySkin(typeof(Combobox<T>));
        }

        void Selector_SelectedItemChanged(Control sender, SelectionEventArgs<T> args)
        {
            if (Selector.Parent == null) return;

            ScreenManager.Flyback(Selector);
            SelectedItem = args.NewItem;
            Selector.SelectedItem = null;
            Focus();
        }

        private Control GenerateControl(T item)
        {
            return TemplateGenerator(item);
        }

        protected override void OnInsert(T item, int index)
        {
            Selector.Items.Insert(index, item);
        }

        protected override void OnRemove(T item, int index)
        {
            // TODO: Prüfen, ob es sich um das selektierte Element handelt
            Selector.Items.Remove(item);
        }

        protected override void OnSelectedItemChanged(SelectionEventArgs<T> args)
        {
            base.OnSelectedItemChanged(args);

            mainControl.Content = TemplateGenerator(args.NewItem);
        }

        protected override void OnLeftMouseClick(MouseEventArgs args)
        {
            base.OnLeftMouseClick(args);

            if (Selector.Parent == null)
            {
                Selector.Width = ActualSize.X - Margin.Left - Margin.Right;
                ScreenManager.Flyout(Selector, new Point(AbsolutePosition.X + Margin.Left, AbsolutePosition.Y + ActualSize.Y));
            }
            else
            {
                ScreenManager.Flyback(Selector);
            }
            args.Handled = true;
        }
    }
}
