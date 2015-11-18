using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoGameUi
{
    public class TabPage : ContainerControl
    {
        public string Title;

        public TabPage(IScreenManager manager, string title) : base(manager)
        {
            Title = title;
        }
    }
}
