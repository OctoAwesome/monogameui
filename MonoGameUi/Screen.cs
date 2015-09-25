namespace MonoGameUi
{
    public abstract class Screen : ContainerControl
    {
        public IScreenManager Manager { get; private set; }

        public string Title { get; protected set; }

        public bool IsOverlay { get; protected set; }

        public bool InHistory { get; protected set; }

        public Screen(IScreenManager manager) : base(manager)
        {
            Manager = manager;
            IsOverlay = false;
            InHistory = true;
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
            Margin = Border.All(0);
            Padding = Border.All(20);

            ApplySkin(typeof(Screen));
        }

        internal void InternalNavigateTo(NavigationEventArgs args)
        {
            OnNavigateTo(args);
        }

        internal void InternalNavigatedTo(NavigationEventArgs args)
        {
            OnNavigatedTo(args);
        }

        internal void InternalNavigateFrom(NavigationEventArgs args)
        {
            OnNavigateFrom(args);
        }

        internal void InternalNavigatedFrom(NavigationEventArgs args)
        {
            OnNavigatedFrom(args);
        }

        /// <summary>
        /// Signalisiert die Navigation hin zu diesem Screen. Der Screen kann hier noch entscheiden ob die
        /// Navigation stattfinden soll oder nicht.
        /// </summary>
        protected virtual void OnNavigateTo(NavigationEventArgs args) { }

        /// <summary>
        /// Signalisiert den abgeschlossenen Navigationsaufruf auf diese Seite.
        /// </summary>
        protected virtual void OnNavigatedTo(NavigationEventArgs args) { }

        /// <summary>
        /// Signalisiert den Versuch von dieser Seite weg zu navigieren. Die Seite kann diese Navigation unterbinden.
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnNavigateFrom(NavigationEventArgs args) { }

        /// <summary>
        /// Signalisiert den abgeschlossenen Navigationsaufruf von dieser Seite weg.
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnNavigatedFrom(NavigationEventArgs args) { }
    }
}
