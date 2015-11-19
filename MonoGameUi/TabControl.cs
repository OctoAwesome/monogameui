using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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
        private StackPanel tabControlStack, tabListStack;
        private ContentControl tabPage;

        /// <summary>
        /// Die nötigen Brushes
        /// </summary>
        public Brush tabActiveBrush;
        public Brush tabBrush ;
        public Brush tabPageBackground ;
        public Brush tabListBackground ;

        public Brush TabActiveBrush {
            get { return tabActiveBrush; }
            set
            {
                tabActiveBrush = value;
                if(tabListStack.Controls.Count > 0)
                    tabListStack.Controls.ElementAt(SelectedTabIndex).Background = tabActiveBrush;
            }
        }
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
        public Brush TabPageBackground
        {
            get { return tabPageBackground; }
            set
            {
                tabPageBackground = value;
                tabPage.Background = TabPageBackground;
            }
        }

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


        IScreenManager Manager;

        int SelectedTabIndex = 0;

        public TabControl(IScreenManager manager) : base(manager)
        { 

            Manager = manager;

            Pages = new ItemCollection<TabPage>();
            Pages.OnInsert += OnInsert;
            Pages.OnRemove += OnRemove;

            tabControlStack = new StackPanel(manager);
            tabControlStack.HorizontalAlignment = HorizontalAlignment.Stretch;
            tabControlStack.VerticalAlignment = VerticalAlignment.Stretch;
            Content = tabControlStack;

            tabListStack = new StackPanel(manager);
            tabListStack.HorizontalAlignment = HorizontalAlignment.Stretch;
            tabListStack.Orientation = Orientation.Horizontal;
            tabListStack.Background = TabListBackground;
            tabControlStack.Controls.Add(tabListStack);

            tabPage = new ContentControl(manager);
            tabPage.HorizontalAlignment = HorizontalAlignment.Stretch;
            tabPage.VerticalAlignment = VerticalAlignment.Stretch;
            tabPage.Background = TabPageBackground;
            tabControlStack.Controls.Add(tabPage);

            tabSpacing = 1;

            ApplySkin(typeof(TabControl));
        }

        private void OnInsert(TabPage item, int index)
        {
            Label title = new Label(Manager);
            title.Text = item.Title;
            title.Padding = Border.All(10);
            title.Background = TabBrush;
            title.Margin = new Border(0, 0, TabSpacing, 0);
            title.LeftMouseClick += (s, e) => SelectTab(Pages.IndexOf(item));
            tabListStack.Controls.Add(title);

            SelectTab(SelectedTabIndex);
        }

        private void OnRemove(TabPage item, int index)
        {
            tabListStack.Controls.RemoveAt(index);                      //Entferne den Tab
            if (Pages.Count > 0)                                        //Nur fortfahren wenn noch Pages vorhanden
            {
                if (index > tabListStack.Controls.Count)                //Wenn die letzte Page entfernt wird...
                    SelectedTabIndex = tabListStack.Controls.Count;     //Setze den TabIndex auf die "neue" letzte
                else SelectedTabIndex = index;                          //Andernfalls, setze den TabIndex  auf den aktuellen index

                SelectTab(SelectedTabIndex);                            //Selektiere den Tab
            }
            tabListStack.InvalidateDimensions();                        //Zeichne den TabListStack neu
        }

        public void SelectTab(int index)
        {
            tabListStack.Controls.ElementAt(SelectedTabIndex).Background = TabBrush;
            SelectedTabIndex = index;
            tabListStack.Controls.ElementAt(index).Background = TabActiveBrush;

            tabPage.Content = Pages.ElementAt(SelectedTabIndex);

            if(TabIndexChanged != null)
                TabIndexChanged.Invoke(this, Pages.ElementAt(index), SelectedTabIndex);
        }

        public void SelectTab(TabPage page)
        {
            try
            {
                tabListStack.Controls.ElementAt(SelectedTabIndex).Background = TabBrush;
                SelectedTabIndex = Pages.IndexOf(page);
            }
            catch (Exception e) { }


            tabListStack.Controls.ElementAt(SelectedTabIndex).Background = TabActiveBrush;
            tabPage.Content = Pages.ElementAt(SelectedTabIndex);

            if (TabIndexChanged != null)
                TabIndexChanged.Invoke(this, page, SelectedTabIndex);
        }

        public event SelectionChangedDelegate TabIndexChanged;

        public delegate void SelectionChangedDelegate(Control control, TabPage tab, int index);
    }
}
