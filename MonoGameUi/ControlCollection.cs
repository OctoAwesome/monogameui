using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameUi
{
    /// <summary>
    /// Erweiterte Liste für Controls
    /// </summary>
    public class ControlCollection : ItemCollection<Control>
    {
        internal List<Control> InZOrder = new List<Control>();
        internal readonly ReverseEnumerable<Control> AgainstZOrder = new ReverseEnumerable<Control>();

        protected Control Owner { get; private set; }

        public ControlCollection(Control owner)
            : base()
        {
            Owner = owner;
            AgainstZOrder.BaseList = InZOrder;
        }

        public override void Add(Control item)
        {
            if (item == null)
                throw new ArgumentNullException("Item cant be null");

            if (item.Parent != null)
                throw new ArgumentException("Item is already part of a different collection");

            // Fokus-Management
            item.SetFocus(null);

            // Taborder einreihen
            if (item.TabOrder == 0)
                item.TabOrder = int.MaxValue;
            else
                foreach (var control in this)
                {
                    if (control.TabOrder >= item.TabOrder) control.TabOrder++;
                }

            InZOrder.Add(item);//TODO insertion sort
            // ZOrder einreihen
            item.ZOrderChanged += item_ZOrderChanged;

            base.Add(item);

            item.Parent = Owner;
            ReorderTab();
            ReorderZ(item);
            item.PathDirty = true;
            Owner.PathDirty = true;
        }

        public override void Clear()
        {
            //TODO verify?
            for (int i = 0; i < this.Count; i++)
            {
                this[i].SetFocus(null);
                this[i].Parent = null;
                this[i].ZOrderChanged -= item_ZOrderChanged;
                this[i].PathDirty = true;
            }
            base.Clear();
            InZOrder.Clear();

            Owner.PathDirty = true;

            ReorderZ(null);
        }

        public override void Insert(int index, Control item)
        {
            if (item == null)
                throw new ArgumentNullException("Item cant be null");

            if (item.Parent != null)
                throw new ArgumentException("Item is already part of a different collection");

            // Fokus-Management
            item.SetFocus(null);

            // TabOrder
            if (item.TabOrder == 0)
                item.TabOrder = index;
            foreach (var control in this)
            {
                if (control.TabOrder >= item.TabOrder) control.TabOrder++;
            }

            InZOrder.Add(item);//TODO: insert sort?

            // ZOrder einreihen
            item.ZOrderChanged += item_ZOrderChanged;

            base.Insert(index, item);

            item.Parent = Owner;
            ReorderTab();
            ReorderZ(item);
            item.PathDirty = true;
            Owner.PathDirty = true;
        }

        public override bool Remove(Control item)
        {
            if (base.Remove(item))
            {

                item.SetFocus(null);

                InZOrder.Remove(item);

                item.Parent = null;
                item.ZOrderChanged -= item_ZOrderChanged;

                ReorderTab();
                ReorderZ(null);

                item.PathDirty = true;
                Owner.PathDirty = true;
                return true;
            }

            return false;
        }

        public override void RemoveAt(int index)
        {
            // Fokus entfernen
            Control c = this[index];
            if (c != null)
            {
                c.SetFocus(null);
                base.RemoveAt(index);

                InZOrder.Remove(c);
                c.Parent = null;
                c.ZOrderChanged -= item_ZOrderChanged;
                ReorderTab();
                ReorderZ(null);
                c.PathDirty = true;
                Owner.PathDirty = true;
            }
        }

        private void ReorderTab()
        {
            // Taborder neu ermitteln
            int tab = 1;
            foreach (var control in this.OrderBy(c => c.TabOrder))
                control.TabOrder = tab++;
        }

        void item_ZOrderChanged(Control sender, PropertyEventArgs<int> args)
        {
            if (InZOrder == null || isDoingUpdate) return;

            // Ein Control hat die Z-Order geändert -> neu sortieren
            ReorderZ(sender);
        }

        private class ZOrderComparer : IComparer<Control>
        {
            public int Compare(Control x, Control y)
            {
                if (x == null || y == null)
                    return 0;
                return x.ZOrder.CompareTo(y.ZOrder);
            }
        }
        private readonly ZOrderComparer _zOrderComparer = new ZOrderComparer();
        private bool isDoingUpdate = false;
        private void ReorderZ(Control control)
        {
            isDoingUpdate = true;
            // Platz schaffen für das veränderte Control
            if (control != null)
                foreach (var c in this)
                {
                    if (c != control && c.ZOrder >= control.ZOrder) c.ZOrder++;
                }

            InZOrder.Sort(_zOrderComparer);
            // Taborder neu ermitteln
            int index = 1;
            foreach (var c in InZOrder)
                c.TabOrder = index++;

            AgainstZOrder.BaseList = InZOrder;

            isDoingUpdate = false;
        }
    }
}
