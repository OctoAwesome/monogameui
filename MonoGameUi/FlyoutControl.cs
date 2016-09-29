using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoGameUi
{
    internal sealed class FlyoutControl : CanvasControl
    {
        internal FlyoutControl(BaseScreenComponent manager) : base(manager) {
            ApplySkin(typeof(FlyoutControl));
        }

        protected override void OnLeftMouseDown(MouseEventArgs args)
        {
            if (Hovered == TreeState.Active && Children.Count > 0)
                Children.Clear();
            base.OnLeftMouseDown(args);
        }

        protected override void OnRightMouseDown(MouseEventArgs args)
        {
            if (Hovered == TreeState.Active && Children.Count > 0)
                Children.Clear();
            base.OnRightMouseClick(args);
        }
    }
}
