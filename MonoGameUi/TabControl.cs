using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonoGameUi
{
    public class TabControl : ContentControl
    {
        public ItemCollection<TabPage> Pages { get; private set; }

        StackPanel tabControlStack, tabListStack;
        ContentControl tabPage;

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
            tabControlStack.Controls.Add(tabListStack);

            tabPage = new ContentControl(manager);
            tabPage.HorizontalAlignment = HorizontalAlignment.Stretch;
            tabPage.VerticalAlignment = VerticalAlignment.Stretch;
            tabPage.Background = new BorderBrush(Color.SlateGray);
            tabControlStack.Controls.Add(tabPage);
        }

        private void OnInsert(TabPage item, int index)
        {
            Label title = new Label(Manager);
            title.Text = item.Title;
            title.Padding = Border.All(10);
            title.Background = new BorderBrush(Color.Gray);
            title.LeftMouseClick += (s, e) => selectTab(index);
            tabListStack.Controls.Add(title);

            selectTab(SelectedTabIndex);
        }

        private void OnRemove(TabPage item, int index)
        {
            tabListStack.Controls.RemoveAt(index);
            if (Pages.Count > 0)
                SelectedTabIndex = 0;
                selectTab(0);
        }

        private void selectTab(int index)
        {
            tabListStack.Controls.ElementAt(SelectedTabIndex).Background = new BorderBrush(Color.Gray);
            SelectedTabIndex = index;
            tabListStack.Controls.ElementAt(index).Background = new BorderBrush(Color.LightGray);

            tabPage.Content = Pages.ElementAt(SelectedTabIndex);
        }

    }
}
