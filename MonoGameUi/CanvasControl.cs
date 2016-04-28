using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using engenious;

namespace MonoGameUi
{
    public class CanvasControl : Control
    {
        private Dictionary<Control, Point> positions = new Dictionary<Control, Point>();

        public CanvasControl(IScreenManager manager, string style = "")
            : base(manager, style)
        {
            ApplySkin(typeof(CanvasControl));
        }

        public void AddControl(Control control, Point position)
        {
            Children.Add(control);
            positions[control] = position;
        }

        public void RemoveControl(Control control)
        {
            Children.Remove(control);
        }

        protected override void OnInsertControl(CollectionEventArgs args)
        {
            positions.Add(args.Control, Point.Zero);
            base.OnInsertControl(args);
        }

        protected override void OnRemoveControl(CollectionEventArgs args)
        {
            positions.Remove(args.Control);
            base.OnRemoveControl(args);
        }

        public override Point GetExpectedSize(Point available)
        {
            return available;
        }

        public override void SetActualSize(Point available)
        {
            if (!Visible)
            {
                SetDimension(Point.Zero, available);
                return;
            }

            SetDimension(available, available);

            // Auf andere Controls anwenden
            foreach (var child in Children)
            {
                child.SetActualSize(ActualClientSize);
                child.ActualPosition = positions[child];
            }

        }
    }
}
