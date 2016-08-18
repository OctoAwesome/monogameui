namespace MonoGameUi
{
    /// <summary>
    /// Bildschirmseite zur Aufnahme von Controls.
    /// </summary>
    public abstract class Screen : ContainerControl
    {
        private bool isVisibleScreen = false;

        private bool isActiveScreen = false;

        /// <summary>
        /// Gibt die Referenz auf den aktuellen Screen-Manager zurück.
        /// </summary>
        public BaseScreenComponent Manager { get; private set; }

        /// <summary>
        /// Gibt den Titel des Screens zurück. Der Manager übernimmt diesen Titel 
        /// standardmäßig in der Titelleiste des Fensters.
        /// </summary>
        public string Title { get; protected set; }

        /// <summary>
        /// Gibt an, ob es sich bei diesem Screen lediglich um ein Overlay handelt. Dies 
        /// sorgt dafür, dass der darunter liegende Screen weiterhin sichtbar bleibt und 
        /// gerendert wird.
        /// </summary>
        public bool IsOverlay { get; protected set; }

        /// <summary>
        /// Gibt an ob dieser Screen einen Eintrag im History Stack erhalten 
        /// soll. ist diese Eigenschaft auf false, wird er bei der Zurück-Navigation 
        /// übersprungen. Dies ist beispieslweise praktisch für Lade-Screens.
        /// </summary>
        public bool InHistory { get; protected set; }

        /// <summary>
        /// Gibt den Standard Mouse Mode für diesen Screen zurück. Er wird beim Navigieren 
        /// zu diesem Screen angewendet.
        /// </summary>
        public MouseMode DefaultMouseMode { get; protected set; }

        /// <summary>
        /// Gibt an ob dieser Screen der aktive Screen ist.
        /// </summary>
        public bool IsActiveScreen
        {
            get
            {
                return isActiveScreen;
            }
            internal set
            {
                if (isActiveScreen != value)
                {
                    PropertyEventArgs<bool> args = new PropertyEventArgs<bool>()
                    {
                        NewValue = value,
                        OldValue = isActiveScreen,
                    };

                    isActiveScreen = value;
                    OnIsActiveScreenChanged(args);
                    if (IsActiveScreenChanged != null)
                        IsActiveScreenChanged(this, args);
                }
            }
        }

        /// <summary>
        /// Gibt an ob dieser Screen im aktuellen Stack sichtbar ist.
        /// </summary>
        public bool IsVisibleScreen
        {
            get { return isVisibleScreen; }
            internal set
            {
                if (isVisibleScreen != value)
                {
                    PropertyEventArgs<bool> args = new PropertyEventArgs<bool>()
                    {
                        OldValue = isVisibleScreen,
                        NewValue = value,
                    };

                    isVisibleScreen = value;

                    OnIsVisibleScreenChanged(args);
                    if (IsVisibleScreenChanged != null)
                        IsVisibleScreenChanged(this, args);
                }
            }
        }

        /// <summary>
        /// Erzeugt einen neuen Screen.
        /// </summary>
        /// <param name="manager">Der verwendete <see cref="IScreenManager"/></param>
        public Screen(BaseScreenComponent manager) : base(manager)
        {
            Manager = manager;
            IsOverlay = false;
            InHistory = true;
            IsVisibleScreen = false;
            IsActiveScreen = false;
            DefaultMouseMode = MouseMode.Free;
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

        /// <summary>
        /// Signalisiert den Screenwechsel beim Navigationsaufruf an.
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnIsActiveScreenChanged(PropertyEventArgs<bool> args) { }

        /// <summary>
        /// Signalisiert den Sichtbarkeitswechsel des Screens.
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnIsVisibleScreenChanged(PropertyEventArgs<bool> args) { }

        /// <summary>
        /// Signalisiert die Änderung in der Eigenschaft IsActiveScreen.
        /// </summary>
        public event PropertyChangedDelegate<bool> IsActiveScreenChanged;

        /// <summary>
        /// Signalisiert die Änderung in der Eigenschaft IsVisibleScreen.
        /// </summary>
        public event PropertyChangedDelegate<bool> IsVisibleScreenChanged;
    }
}
