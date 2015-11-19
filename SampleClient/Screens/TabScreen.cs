using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MonoGameUi;

namespace SampleClient.Screens
{
    class TabScreen : Screen
    {
        public TabScreen(IScreenManager manager) : base(manager)
        {
            //Create Tab Pages
            TabPage tabPage = new TabPage(manager, "Tab 1");
            TabPage tabPage2 = new TabPage(manager, "Tab 2");
            TabPage tabPage3 = new TabPage(manager, "Tab 3");

            //Create Tab Control & Add Pages
            TabControl tab = new TabControl(manager);
            tab.Pages.Add(tabPage);
            tab.Pages.Add(tabPage2);
            tab.Pages.Add(tabPage3);
            tab.VerticalAlignment = VerticalAlignment.Stretch;
            tab.HorizontalAlignment = HorizontalAlignment.Stretch;

            //Add Text to Page 1
            tabPage.Controls.Add(new Label(manager) { Text = "Content on Page 1" });

            //Add "Create Tab" to page 2
            Button createPage = Button.TextButton(manager, "Create Tab");
            createPage.LeftMouseClick += (s, e) =>
            {
                TabPage page = new TabPage(manager, "NEW TAB");
                page.Controls.Add(new Label(manager) { Text = "This is a new tab page" });
                tab.Pages.Add(page);
            };
            tabPage2.Controls.Add(createPage);

            //Add "Remove this page" to page 3
            Button removePage3 = Button.TextButton(manager, "Remove this Page");
            removePage3.LeftMouseClick += (s, e) =>
            {
                tab.Pages.Remove(tabPage3);
            };
            tabPage3.Controls.Add(removePage3);


            Controls.Add(tab);
        }
    }
}
