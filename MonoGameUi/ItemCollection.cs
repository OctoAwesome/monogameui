using System;
using System.Collections;
using System.Collections.Generic;

namespace MonoGameUi
{


    /// <summary>
    /// Erweiterte Liste für Controls
    /// </summary>
    public class ItemCollection<T> : IList<T> where T : class
    {
        public List<T> Items = new List<T>();

        public ItemCollection() { }

        public T this[int index]
        {
            get
            {
                lock (Items)
                {
                    return Items[index];
                }
            }

            set
            {
                throw new NotSupportedException();
            }
        }

        public int Count
        {
            get
            {
                lock (Items)
                {
                    return Items.Count;
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public virtual void Add(T item)
        {
            lock (Items)
            {
                if (item == null)
                    throw new ArgumentNullException("Item cant be null");

                if (Items.Contains(item))
                    throw new ArgumentException("Item is already part of this collection");

                // Control einfügen
                Items.Add(item);

                // Event werfen
                if (OnInsert != null)
                    OnInsert(item, Items.IndexOf(item));
            }
        }

        public virtual void Clear()
        {
            lock (Items)
            {
                var temp = Items.ToArray();

                Items.Clear();

                for (int i = 0; i < temp.Length; i++)
                    if (OnRemove != null)
                        OnRemove(temp[i], i);
            }
        }

        public bool Contains(T item)
        {
            lock (Items)
            {
                return Items.Contains(item);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (Items)
            {
                Items.CopyTo(array, arrayIndex);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (Items)
            {
                return Items.GetEnumerator();
            }
        }

        public int IndexOf(T item)
        {
            lock (Items)
            {
                return Items.IndexOf(item);
            }
        }

        public virtual void Insert(int index, T item)
        {
            lock (Items)
            {
                if (item == null)
                    throw new ArgumentNullException("Item cant be null");

                if (Items.Contains(item))
                    throw new ArgumentException("Item is already part of this collection");

                // Control einfügen
                Items.Insert(index, item);

                // Event werfen
                if (OnInsert != null)
                    OnInsert(item, Items.IndexOf(item));
            }
        }

        public virtual bool Remove(T item)
        {
            lock (Items)
            {
                if (item == null)
                    throw new ArgumentNullException("Item cant be null");

                if (!Items.Contains(item))
                    return false;

                // Control entfernen
                int index = Items.IndexOf(item);
                Items.Remove(item);

                // Event
                if (OnRemove != null)
                    OnRemove(item, index);

                return true;
            }
        }

        public virtual void RemoveAt(int index)
        {
            lock (Items)
            {
                if (index < 0 && index >= Items.Count)
                    throw new ArgumentOutOfRangeException("index");

                // Control entfernen
                T c = Items[index];
                Items.RemoveAt(index);

                // Event werfen
                if (OnRemove != null)
                    OnRemove(c, index);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (Items)
            {
                return Items.GetEnumerator();
            }
        }

        public event ItemCollectionDelegate<T> OnInsert;

        public event ItemCollectionDelegate<T> OnRemove;
    }

    public delegate void ItemCollectionDelegate<T>(T item, int index);
}
