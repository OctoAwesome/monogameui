using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoGameUi
{
    public class TabPage : ContainerControl
    {
        public string Title;

        public TabPage(BaseScreenComponent manager, string title) : base(manager)
        {
            Title = title;
        }
    }
}
