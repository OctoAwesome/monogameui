using System.Collections.Generic;
using engenious.Input;

namespace MonoGameUi
{
    /// <summary>
    /// Basis-Control für Item-Listen
    /// </summary>
    public abstract class ListControl<T> : Control where T : class
    {
        private Brush selectedItemBrush = null;

        private T selectedItem = null;

        /// <summary>
        /// Liefert eine Liste der enthaltenen Elemente zurück.
        /// </summary>
        public IList<T> Items { get; private set; }

        /// <summary>
        /// Liefert das aktuell selektierte Element zurück oder legt dieses fest.
        /// </summary>
        public T SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (selectedItem != value)
                {
                    SelectionEventArgs<T> args = new SelectionEventArgs<T>()
                    {
                        OldItem = selectedItem,
                        NewItem = value
                    };

                    selectedItem = value;

                    OnSelectedItemChanged(args);
                    if (SelectedItemChanged != null)
                        SelectedItemChanged(this, args);
                }
            }
        }

        /// <summary>
        /// Gibt den Brush für selektierte Elemente zurück oder legt diesen fest.
        /// </summary>
        public Brush SelectedItemBrush
        {
            get { return selectedItemBrush; }
            set
            {
                if (selectedItemBrush != value)
                {
                    PropertyEventArgs<Brush> args = new PropertyEventArgs<Brush>()
                    {
                        OldValue = selectedItemBrush,
                        NewValue = value,
                    };

                    selectedItemBrush = value;
                    InvalidateDrawing();

                    OnSelectedItemBrushChanged(args);
                    if (SelectedItemBrushChanged != null)
                        SelectedItemBrushChanged(this, args);
                }
            }
        }

        /// <summary>
        /// Standard Konstruktor für das List-Control.
        /// </summary>
        /// <param name="manager"></param>
        public ListControl(BaseScreenComponent manager, string style = "")
            : base(manager, style)
        {
            CanFocus = true;
            TabStop = true;

            ItemCollection<T> collection = new ItemCollection<T>();
            collection.OnInsert += (item, index) => OnInsert(item, index);
            collection.OnRemove += (item, index) =>
            {
                if (SelectedItem == item)
                    SelectedItem = null;
                OnRemove(item, index);
            };
            Items = collection;

            ApplySkin(typeof(ListControl<T>));
        }

        protected abstract void OnRemove(T item, int index);

        protected abstract void OnInsert(T item, int index);

        protected override void OnKeyPress(KeyEventArgs args)
        {
            // Nur reagieren, wenn der Fokus gesetzt ist
            if (Focused == TreeState.None) return;

            switch (args.Key)
            {
                case Keys.Up:
                    SelectPreview();
                    args.Handled = true;
                    break;
                case Keys.Down:
                    SelectNext();
                    args.Handled = true;
                    break;
                case Keys.Home:
                    SelectFirst();
                    args.Handled = true;
                    break;
                case Keys.End:
                    SelectLast();
                    args.Handled = true;
                    break;
            }

            base.OnKeyPress(args);
        }

        public void SelectFirst()
        {
            if (Items.Count > 0)
                SelectedItem = Items[0];
        }

        public void SelectLast()
        {
            if (Items.Count > 0)
                SelectedItem = Items[Items.Count - 1];
        }

        public void SelectNext()
        {
            if (SelectedItem == null)
            {
                if (Items.Count > 0)
                    SelectedItem = Items[0];
            }
            else
            {
                int index = Items.IndexOf(SelectedItem);
                if (index < Items.Count - 1)
                    SelectedItem = Items[index + 1];
            }
        }

        public void SelectPreview()
        {
            if (SelectedItem == null)
            {
                if (Items.Count > 0)
                    SelectedItem = Items[Items.Count - 1];
            }
            else
            {
                int index = Items.IndexOf(SelectedItem);
                if (index > 0)
                    SelectedItem = Items[index - 1];
            }
        }

        protected virtual void OnSelectedItemChanged(SelectionEventArgs<T> args) { }

        protected virtual void OnSelectedItemBrushChanged(PropertyEventArgs<Brush> args) { }

        public event SelectionDelegate<T> SelectedItemChanged;

        public event PropertyChangedDelegate<Brush> SelectedItemBrushChanged;

        public GenerateTempalteDelegate<T> TemplateGenerator { get; set; }
    }

    public delegate Control GenerateTempalteDelegate<T>(T item);
}
