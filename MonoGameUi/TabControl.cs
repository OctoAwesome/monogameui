using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MonoGameUi
{
    public class TabControl : Control
    {
        /// <summary>
        /// Liste alle Pages
        /// </summary>
        public ItemCollection<TabPage> Pages { get; private set; }

        /// <summary>
        /// Content des TabControl
        /// </summary>
        private Control Content
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

        /// <summary>
        /// Die zur Darstellung benötigten Controls
        /// </summary>
        private StackPanel tabListStack;
        private Grid tabControlGrid;
        private ContentControl tabPage;

        /// <summary>
        /// Die nötigen Brushes
        /// </summary>
        public Brush tabActiveBrush;
        public Brush tabBrush ;
        public Brush tabPageBackground ;
        public Brush tabListBackground ;

        /// <summary>
        /// Die Brush für den aktiven Tab
        /// </summary>
        public Brush TabActiveBrush {
            get { return tabActiveBrush; }
            set
            {
                tabActiveBrush = value;
                if(tabListStack.Controls.Count > 0)
                    tabListStack.Controls.ElementAt(SelectedTabIndex).Background = tabActiveBrush;
            }
        }

        /// <summary>
        /// Brush für inaktive Tabs
        /// </summary>
        public Brush TabBrush
        {
            get { return tabBrush; }
            set
            {
                tabBrush = value;
                if (tabListStack.Controls.Count > 0)
                    foreach (Control c in tabListStack.Controls.Where(c => tabListStack.Controls.IndexOf(c) != SelectedTabIndex))
                        c.Background = TabBrush;
            }
        }

        /// <summary>
        /// Brush für den TabPage Background
        /// </summary>
        public Brush TabPageBackground
        {
            get { return tabPageBackground; }
            set
            {
                tabPageBackground = value;
                tabPage.Background = TabPageBackground;
            }
        }

        /// <summary>
        /// Brush für den Hintergrund der TabListe
        /// </summary>
        public Brush TabListBackground
        {
            get { return tabListBackground; }
            set
            {
                tabListBackground = value;
                tabListStack.Background = TabListBackground;
            }
        }

        /// <summary>
        /// Spacing zwischen Tabs
        /// </summary>
        private int tabSpacing;

        public int TabSpacing {
            get
            {
                return tabSpacing;
            }
            set
            {
                tabSpacing = value;
                foreach (Control tabLabel in tabListStack.Controls)
                    tabLabel.Margin = new Border(0, 0, tabSpacing, 0);
            }
        }

        /// <summary>
        /// Der Index des aktiven Tabs
        /// </summary>
        private int SelectedTabIndex = 0;

        /// <summary>
        /// Base Constructor
        /// </summary>
        /// <param name="manager">ScreenManager</param>
        public TabControl(BaseScreenComponent manager) : base(manager)
        { 
            Pages = new ItemCollection<TabPage>();
            Pages.OnInsert += OnInsert;
            Pages.OnRemove += OnRemove;

            tabControlGrid = new Grid(manager)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            tabControlGrid.Columns.Add(new ColumnDefinition() {ResizeMode = ResizeMode.Parts, Width = 1});
            tabControlGrid.Rows.Add(new RowDefinition() {ResizeMode = ResizeMode.Auto});
            tabControlGrid.Rows.Add(new RowDefinition() {ResizeMode = ResizeMode.Parts, Height =  1});
            Content = tabControlGrid;
            

            tabListStack = new StackPanel(manager);
            tabListStack.HorizontalAlignment = HorizontalAlignment.Stretch;
            tabListStack.Orientation = Orientation.Horizontal;
            tabListStack.Background = TabListBackground;
            tabControlGrid.AddControl(tabListStack, 0, 0);

            tabPage = new ContentControl(manager);
            tabPage.HorizontalAlignment = HorizontalAlignment.Stretch;
            tabPage.VerticalAlignment = VerticalAlignment.Stretch;
            tabPage.Background = TabPageBackground;
            tabControlGrid.AddControl(tabPage, 0, 1);

            ApplySkin(typeof(TabControl));
        }

        /// <summary>
        /// Wird aufgerufen wenn ein neues Element zu "Pages" hinzugefügt wird, erstellt einen neuen Eintrag in der TabList
        /// </summary>
        private void OnInsert(TabPage item, int index)
        {
            Label title = new Label(ScreenManager);
            title.Text = item.Title;
            title.Padding = Border.All(10);
            title.Background = TabBrush;
            title.Margin = new Border(0, 0, TabSpacing, 0);
            title.LeftMouseClick += (s, e) => SelectTab(Pages.IndexOf(item));
            title.CanFocus = true;
            title.TabStop = true;
            title.KeyDown += (s, e) => {
                if (e.Key == engenious.Input.Keys.Enter && title.Focused == TreeState.Active)
                    SelectTab(Pages.IndexOf(item));
            };
            tabListStack.Controls.Add(title);

            SelectTab(SelectedTabIndex);
        }

        /// <summary>
        /// Wird aufgerufen wenn ein Element aus "Pages" entfernt wird, entfernt den Eintrag in der TabList
        /// </summary>
        private void OnRemove(TabPage item, int index)
        {
            tabListStack.Controls.RemoveAt(index);                      //Entferne den Tab
            if (Pages.Count > 0)                                        //Nur fortfahren wenn noch Pages vorhanden
            {
                if (index >= tabListStack.Controls.Count)                //Wenn die letzte Page entfernt wird...
                    SelectedTabIndex = tabListStack.Controls.Count - 1;     //Setze den TabIndex auf die "neue" letzte
                else SelectedTabIndex = index;                          //Andernfalls, setze den TabIndex  auf den aktuellen index

                SelectTab(SelectedTabIndex);                            //Selektiere den Tab
            }
            tabListStack.InvalidateDimensions();                        //Zeichne den TabListStack neu
        }

        /// <summary>
        /// Selektieren eines Tabs mit Index
        /// </summary>
        public void SelectTab(int index)
        {
            tabListStack.Controls.ElementAt(SelectedTabIndex).Background = TabBrush;
            SelectedTabIndex = index;
            tabListStack.Controls.ElementAt(index).Background = TabActiveBrush;

            tabPage.Content = Pages.ElementAt(SelectedTabIndex);

            if(TabIndexChanged != null)
                TabIndexChanged.Invoke(this, Pages.ElementAt(index), SelectedTabIndex);
        }

        /// <summary>
        /// Selektieren eines Tabs mit Page
        /// </summary>
        public void SelectTab(TabPage page)
        {
            try
            {
                tabListStack.Controls.ElementAt(SelectedTabIndex).Background = TabBrush;
                SelectedTabIndex = Pages.IndexOf(page);
            }
            finally
            {
                tabListStack.Controls.ElementAt(SelectedTabIndex).Background = TabActiveBrush;
                tabPage.Content = Pages.ElementAt(SelectedTabIndex);

                if (TabIndexChanged != null)
                    TabIndexChanged.Invoke(this, page, SelectedTabIndex);
            }

        }

        /// <summary>
        /// Event wenn der TabIndex sich ändert
        /// </summary>
        public event SelectionChangedDelegate TabIndexChanged;

        public delegate void SelectionChangedDelegate(Control control, TabPage tab, int index);
    }
}
