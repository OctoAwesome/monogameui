using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameUi
{
    /// <summary>
    /// Basisinterface für erweiterte Listen für Controls
    /// </summary>
    public interface IControlCollection : IList<Control>
    {
        /// <summary>
        /// Elemente in Z-Ordner (von vorne nach hinten)
        /// </summary>
        /// <returns></returns>
        Control[] InZOrder();

        /// <summary>
        /// Elemente in entgegengesetzter Z-Order (von hinten nach vorne)
        /// </summary>
        /// <returns></returns>
        Control[] AgainstZOrdner();
    }

    /// <summary>
    /// Erweiterte Liste für Controls
    /// </summary>
    internal class ControlCollection : ItemCollection<Control>, IControlCollection
    {
        private Control[] inZOrder = new Control[0];

        protected Control Owner { get; private set; }

        public ControlCollection(Control owner)
            : base()
        {
            Owner = owner;
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
                foreach (var control in this.Where(c => c.TabOrder >= item.TabOrder))
                    control.TabOrder++;

            // ZOrder einreihen
            item.ZOrderChanged += item_ZOrderChanged;

            base.Add(item);

            item.Parent = Owner;
            ReorderTab();
            ReorderZ(item);

        }

        public override void Clear()
        {
            var temp = this.ToArray();

            base.Clear();

            for (int i = 0; i < temp.Length; i++)
            {
                temp[i].SetFocus(null);
                temp[i].Parent = null;
                temp[i].ZOrderChanged -= item_ZOrderChanged;
            }

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
            foreach (var control in this.Where(c => c.TabOrder >= item.TabOrder))
                control.TabOrder++;

            // ZOrder einreihen
            item.ZOrderChanged += item_ZOrderChanged;

            base.Insert(index, item);

            item.Parent = Owner;
            ReorderTab();
            ReorderZ(item);
        }

        public override bool Remove(Control item)
        {
            if (base.Remove(item))
            {

                item.SetFocus(null);
                item.Parent = null;
                item.ZOrderChanged -= item_ZOrderChanged;

                ReorderTab();
                ReorderZ(null);
                return true;
            }

            return false;
        }

        public override void RemoveAt(int index)
        {
            // Fokus entfernen
            Control c = this[index];
            if (c != null) c.SetFocus(null);

            base.RemoveAt(index);

            c.Parent = null;
            c.ZOrderChanged -= item_ZOrderChanged;
            ReorderTab();
            ReorderZ(null);
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
            if (inZOrder == null) return;

            // Ein Control hat die Z-Order geändert -> neu sortieren
            ReorderZ(sender);
        }

        private void ReorderZ(Control control)
        {
            inZOrder = null;

            // Platz schaffen für das veränderte Control
            if (control != null)
                foreach (var c in this.Where(c => c != control && c.ZOrder >= control.ZOrder))
                    c.ZOrder++;

            // Taborder neu ermitteln
            int index = 1;
            foreach (var c in this.OrderBy(c => c.ZOrder))
                c.TabOrder = index++;

            inZOrder = this.OrderBy(c => c.ZOrder).ToArray();
        }

        /// <summary>
        /// Elemente in Z-Ordner (von vorne nach hinten)
        /// </summary>
        /// <returns></returns>
        public Control[] InZOrder()
        {
            return inZOrder.ToArray();
        }

        /// <summary>
        /// Elemente in entgegengesetzter Z-Order (von hinten nach vorne)
        /// </summary>
        /// <returns></returns>
        public Control[] AgainstZOrdner()
        {
            var localOrder = inZOrder.ToArray();
            Array.Reverse(localOrder);
            return localOrder;
        }
    }
}
