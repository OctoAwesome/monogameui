using System.Linq;
using engenious;
using engenious.Graphics;

namespace MonoGameUi
{
    /// <summary>
    /// List-Control für jegliche Art von Daten.
    /// </summary>
    /// <typeparam name="T">Datentyp der aufzulistenden Daten</typeparam>
    public class Listbox<T> : ListControl<T> where T : class
    {
        /// <summary>
        /// Gibt den Scroll-Container zurück der um das Stackpanel der ListBox gespannt ist.
        /// </summary>
        public ScrollContainer ScrollContainer { get; private set; }

        /// <summary>
        /// Gibt das StackPanel zurück das für die Auflistung der Items genutzt wird.
        /// </summary>
        public StackPanel StackPanel { get; private set; }

        public Orientation Orientation
        {
            get { return StackPanel.Orientation; }
            set
            {
                StackPanel.Orientation = value;
                switch (StackPanel.Orientation)
                {
                    case Orientation.Vertical:
                        StackPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
                        StackPanel.VerticalAlignment = VerticalAlignment.Top;
                        ScrollContainer.HorizontalScrollbarEnabled = false;
                        ScrollContainer.VerticalScrollbarEnabled = true;
                        break;
                    case Orientation.Horizontal:
                        StackPanel.HorizontalAlignment = HorizontalAlignment.Left;
                        StackPanel.VerticalAlignment = VerticalAlignment.Stretch;
                        ScrollContainer.HorizontalScrollbarEnabled = true;
                        ScrollContainer.VerticalScrollbarEnabled = false;
                        break;
                }
            }
        }

        /// <summary>
        /// Liefert das Container Control des aktuell selektierten Items zurück.
        /// </summary>
        private Control SelectedItemContainer
        {
            get
            {
                return GetItemContainer(SelectedItem);
            }
        }

        /// <summary>
        /// Liefert das Container-Control des angegebenen Elements.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private Control GetItemContainer(T item)
        {
            if (item != null)
                return StackPanel.Controls.FirstOrDefault(c => c.Tag == item);
            return null;
        }

        public Listbox(BaseScreenComponent manager)
            : base(manager)
        {
            ScrollContainer = new ScrollContainer(manager)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            Children.Add(ScrollContainer);

            StackPanel = new StackPanel(manager);
            Orientation = Orientation.Vertical;
            ScrollContainer.Content = StackPanel;

            ApplySkin(typeof(Listbox<T>));
        }

        protected override void OnInsert(T item, int index)
        {
            Control control = TemplateGenerator(item);
            control.Tag = item;
            StackPanel.Controls.Insert(index, control);
        }

        protected override void OnRemove(T item, int index)
        {
            Control control = GetItemContainer(item);
            if (control != null)
                StackPanel.Controls.Remove(control);
            if (StackPanel.Controls.Count == 0)
                InvalidateDimensions();
        }

        protected override void OnDrawBackground(SpriteBatch batch, Rectangle backgroundArea, GameTime gameTime, float alpha)
        {
            base.OnDrawBackground(batch, backgroundArea, gameTime, alpha);

            // Selektiertes Item mit dem richtigen Brush markieren
            Control control = SelectedItemContainer;
            if (control != null && SelectedItemBrush != null)
            {
                SelectedItemBrush.Draw(batch,
                    new Rectangle(control.AbsolutePosition, control.ActualSize), alpha);
            }
        }

        protected override void OnDrawFocusFrame(SpriteBatch batch, Rectangle contentArea, GameTime gameTime, float alpha)
        {
            if (Skin.Current.FocusFrameBrush != null)
            {
                Rectangle area = StackPanel.ActualClientArea;
                area.X = contentArea.X;
                area.Y = contentArea.Y;
                Skin.Current.FocusFrameBrush.Draw(batch, area, alpha);
            }
        }

        protected override void OnSelectedItemChanged(SelectionEventArgs<T> args)
        {
            base.OnSelectedItemChanged(args);

            if (args.NewItem != null)
                EnsureVisibility(args.NewItem);
        }

        /// <summary>
        /// Stellt die Sichtbarkeit des angegebenen Items sicher.
        /// </summary>
        /// <param name="item"></param>
        public void EnsureVisibility(T item)
        {
            Control container = GetItemContainer(item);
            Rectangle visibleArea = ScrollContainer.VisibleArea;

            // Element ist zu weit unten -> hoch scrollen
            if (container.ActualPosition.Y + container.ActualSize.Y > visibleArea.Bottom)
                ScrollContainer.VerticalScrollPosition =
                    container.ActualPosition.Y + container.ActualSize.Y - ActualClientSize.Y;

            // Element ist zu weit oben -> nach unten scrollen
            if (container.ActualPosition.Y < visibleArea.Top)
                ScrollContainer.VerticalScrollPosition = container.ActualPosition.Y;

            // Element ist zu weit rechts -> nach links scrollen
            if (container.ActualPosition.X + container.ActualSize.X > visibleArea.Right)
                ScrollContainer.HorizontalScrollPosition =
                    container.ActualPosition.X + container.ActualSize.X - ActualClientSize.X;

            // Element ist zu weit links -> nach rechts scrollen
            if (container.ActualPosition.X < visibleArea.Left)
                ScrollContainer.HorizontalScrollPosition = container.ActualPosition.X;
        }

        protected override void OnLeftMouseClick(MouseEventArgs args)
        {
            base.OnLeftMouseClick(args);

            Control nextSelected = null;
            foreach (var item in StackPanel.Controls)
            {
                if (item.Hovered != TreeState.None)
                {
                    nextSelected = item;
                    break;
                }
            }

            if (nextSelected != null)
                SelectedItem = nextSelected.Tag as T;
            else SelectedItem = null;
        }
    }
}
